using System;
using System.Collections.Generic;

namespace SAC.TaskQueue
{
    public interface ITaskQueue
    {
        TaskResult Run(int taskQueueId);
    }

    public class TaskResult
    {
        private readonly TaskStatus _status;
        private readonly string _msg;
        public TaskResult(TaskStatus status, string message)
        {
            _status = status;
            _msg = message;
        }
        public TaskStatus Status { get { return _status; } }
        public string Message { get { return _msg; } }
    }

    public static class BindingId
    {
        public const string COL_SLICE = "COLSlice";
        public const string DOWNLOAD_MLB_XML_FILES = "DownloadMLBxmlFiles";
    }
    public enum TaskStatus
    {
        NOT_STARTED = 1,
        RUNNING = 2,
        COMPLETE = 3,
        FAILED = 4
    }
}
