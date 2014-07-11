using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace SAC.TaskQueue
{
    public class DownloadMLBFiles : TaskQueueBase
    {
        //injected construct with the Id of the task to be run
        public DownloadMLBFiles(int taskQueueId) : base(taskQueueId) { }

        public override TaskResult Run(int taskQueueId)
        {
            List<string> mlbFiles = new List<string>() { "http://mlb.com/lookup/named.cur_bio.bam", "http://mlb.com/lookup/named.cur_hitting.bam", "http://mlb.com/lookup/named.cur_pitching.bam", "http://mlb.com/lookup/named.cur_fielding.bam" };
            
            //build the collection of threaded tasks
            List<Task> tasks = new List<Task>();
            foreach (var xmlFeed in mlbFiles)
            {
                tasks.Add(downloadFileAsync(xmlFeed));
            }
            //kick them all off & wait until they've all completed.
            Task<TaskResult> rtn = DownloadFilesAsync(tasks);
            //await Task.WhenAll(tasks.ToArray());

            //Console.WriteLine("{0} would be emailed {1}", base.CreatedByUser, rtn.Message);
            //
            base.SetTaskComplete(rtn.Result.Status, rtn.Result.Message);
            return rtn.Result;
        }

        private async Task<TaskResult> DownloadFilesAsync(List<Task> tasks)
        {

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            await Task.WhenAll(tasks.ToArray());
            sw.Stop();

            TaskResult rtn = new TaskResult(TaskStatus.COMPLETE, string.Format("{0} files downloaded in {1}", tasks.Count, sw.Elapsed));
            return rtn;
            
        }
        /// <summary>
        /// uses WebClient to return a Task object that will be used to download the passed in url
        /// </summary>
        /// <param name="url">The URL of the file to download</param>
        /// <returns>a Task object that returns a string</returns>
        static async Task<string> downloadFileAsync(string xmlFeed)
        {
            var rtn = xmlFeed;
            string xml = await new WebClient().DownloadStringTaskAsync(new Uri(xmlFeed));
            System.Diagnostics.Debug.WriteLine(xml);
            return xml;
        }
    }
}
