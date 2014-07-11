using System;

namespace SAC.TaskQueue.Data.Model
{
    public class Task
    {
        public Task() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public string BindingName { get; set; }
        public bool IsActive { get; set; }
    }
}
