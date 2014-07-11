using System;

namespace SAC.TaskQueue.Data.Model
{
    public class TaskQueue
    {
        public TaskQueue() { }

        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Parameters { get; set; }
        public int StatusId { get; set; }
        public string StatusMessage { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Task Task { get; set; }
        
    }
}
