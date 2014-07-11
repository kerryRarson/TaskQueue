using Ninject.Modules;
using Ninject;

namespace SAC.TaskQueue
{
    public class ninjectBindings : NinjectModule
    {
        public override void Load(){
            //TODO #2 bind the new task name to the concrete implementation
            Bind<ITaskQueue>().To<COL.Tasks.Slice>().Named(BindingId.COL_SLICE);
            Bind<ITaskQueue>().To<DownloadMLBFiles>().Named(BindingId.DOWNLOAD_MLB_XML_FILES);
        }
    }
}
