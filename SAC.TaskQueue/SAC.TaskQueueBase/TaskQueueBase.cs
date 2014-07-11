using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SAC.TaskQueue.Data;
using SAC.TaskQueue.Data.Model;

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
            using (var ctx = new DataproDB())
            {
                var qTask = ctx.TaskQueue.Where(t => t.Id == this.TaskQueueId).FirstOrDefault();
                qTask.StartedDate = DateTime.Now;
                ctx.SaveChanges();
            }
        }
        public void SetTaskComplete(TaskStatus status, string message)
        {
            int statusId = (int)status;
            using (var ctx = new DataproDB())
            {
                var qTask = ctx.TaskQueue.Where(t => t.Id == this.TaskQueueId).FirstOrDefault();
                qTask.StatusId = statusId;
                qTask.CompleteDate = DateTime.Now;
                qTask.StatusMessage = message;
                ctx.SaveChanges();
            }
        }
        public void LoadTask(int taskQueueId)
        {
            _taskQueueId = taskQueueId;

            using (var ctx = new DataproDB())
            {
                var qTask = ctx.TaskQueue.Where(t => t.Id == this.TaskQueueId).FirstOrDefault();
                _parameters = deserializeParameters(qTask.Parameters);
                _createdByUser = qTask.CreatedBy;
                SetTaskStarted();
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
