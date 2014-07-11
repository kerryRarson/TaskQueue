using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SAC.TaskQueue
{
    public abstract class TaskQueueBase : ITaskQueue
    {
        private Dictionary<string, string> _parameters;
        private int _taskQueueId;
        private string _createdByUser;

        protected TaskQueueBase(int taskQueueId)
        {
            //deserialize the xml parameters from the db
            LoadTask(taskQueueId);
        }
        public abstract TaskResult Run(int taskQueueId);

        public void SetTaskStarted()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault();
            db.ExecuteNonQuery(CommandType.Text, "update app.taskqueue set starteddate = '"  + DateTime.Now.ToString() + "' where taskQueueId = " + this.TaskQueueId);
        }
        public void SetTaskComplete(TaskStatus status)
        {
            int statusId = (int)status;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault();
            db.ExecuteNonQuery(CommandType.Text, "update app.taskqueue set statusid = " + statusId + ", completedate = '" + DateTime.Now.ToString() + "' where taskQueueId = " + this.TaskQueueId);
        }
        public void LoadTask(int taskQueueId)
        {
            _taskQueueId = taskQueueId;

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            var db = factory.CreateDefault();
            using (IDataReader reader = db.ExecuteReader(CommandType.Text, "SELECT createdBy, parameters, taskId FROM app.taskQueue where taskQueueId = " + taskQueueId))
            {
                while (reader.Read())
                {
                    _parameters = deserializeParameters(reader.GetString(1));
                    _createdByUser = reader.GetString(0);
                    SetTaskStarted();
                }
            }
        }
        public int TaskQueueId
        {
            get { return _taskQueueId; }
        }
        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
        }
        public string CreatedByUser
        {
            get { return _createdByUser; }
        }
        private Dictionary<string, string> deserializeParameters(string xmlParams)
        {
            Dictionary<string, string> rtn = new Dictionary<string, string>();
            var xml = string.Format("<?xml version='1.0' encoding='UTF-8'?><root>{0}</root>", xmlParams);


            var xdoc = XDocument.Parse(xml);
            rtn = xdoc.Descendants("param")
                              .ToDictionary(d => (string)d.Attribute("name"),
                                            d => (string)d);

            return rtn;
        }

    }
}
