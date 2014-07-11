using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
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
            using (var ctx = new DataproDB())
            {
                var qItem = ctx.TaskQueue.Where(q => q.StatusId == 1).OrderBy(q => q.Id).FirstOrDefault();
                if (qItem != null)
                {
                    //get the concrete implementation from the DI binding
                    IKernel kernel = new StandardKernel();
                    kernel.Load(Assembly.GetExecutingAssembly());
                    var task = kernel.Get<ITaskQueue>(qItem.Task.BindingName, new ConstructorArgument("taskQueueId", qItem.Id));

                    //run the task
                    var result = task.Run(qItem.Id);
                }
            }
        }
    }
}
