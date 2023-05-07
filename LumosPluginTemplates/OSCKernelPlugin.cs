using LumosLIB.Kernel.Log;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Plugin;

namespace OSCPlugin
{
    public class OSCKernelPlugin : KernelPluginBase
    {
        private static readonly ILumosLog Log = LumosLogger.getInstance(typeof(OSCKernelPlugin));
        public OSCKernelPlugin() : base("{adcb0a51-f828-49c4-8615-18bd0e010b76}", "OSCKernelPlugin") //GUID bitte unbedingt gegen einen eigenen tauschen, um überschneidungen zu entgehen!!!!!!!!!!!!!!!!
        {
        }

        protected override void initializePlugin()
        {
            Log.Info("initialize");
            
        }

        protected override void shutdownPlugin()
        {
            Log.Info("is shutting down!");
        }

        protected override void startupPlugin()
        {
            Log.Info("is starting up!");
            
        }
    }
}