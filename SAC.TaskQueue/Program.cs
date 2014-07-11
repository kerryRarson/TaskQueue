using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using log4net;
using Ninject;
using Ninject.Parameters;
using SAC.TaskQueue.Data;
using SAC.TaskQueue.Data.Model;

namespace SAC.TaskQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialize the log
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug(String.Format("starting."));

            try
            {


                using (var ctx = new DataproDB())
                {
                    var qItem = ctx.TaskQueue.Where(q => q.StatusId == 1).OrderBy(q => q.Id).FirstOrDefault();
                    if (qItem != null)
                    {
                        log.Debug(String.Format("running TaskQueue Id - '{0}'", qItem.Id));
                        //get the concrete implementation from the DI binding
                        IKernel kernel = new StandardKernel();
                        kernel.Load(Assembly.GetExecutingAssembly());
                        log.Debug(string.Format("getting binding {0} from ninject kernel", qItem.Task.BindingName));
                        var task = kernel.Get<ITaskQueue>(qItem.Task.BindingName, new ConstructorArgument("taskQueueId", qItem.Id));
                        log.Debug(String.Format("type '{0}' returned from kernel", task.GetType().Name));
                        //run the task
                        var result = task.Run(qItem.Id);
                    }
                }
            }
            catch (Exception err)
            {
                log.Error(err.ToString());
            }
            log.Debug("Ending");
        }
    }
}
