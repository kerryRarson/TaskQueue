using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAC.TaskQueue;

namespace SAC.COL.Tasks
{
    public class Slice : TaskQueueBase
    {
        //injected construct with the Id of the task to be run
        public Slice(int taskQueueId) : base(taskQueueId) { }

        public override TaskResult Run(int taskQueueId)
        {
            //actual slice implementation would go here.
            //search parameters from the user
            foreach (var param in Parameters)
	        {
		        System.Diagnostics.Debug.WriteLine(string.Format("{0} - {1}", param.Key, param.Value));
	        }

            string returnMessage = string.Format("Some message to user-{0} from the COL slice implementation would go here.", this.CreatedByUser);
            
            //mark it complete in the database
            base.SetTaskComplete(TaskQueue.TaskStatus.COMPLETE,returnMessage);
            return new TaskResult(SAC.TaskQueue.TaskStatus.COMPLETE, returnMessage);
        }

    }
}
