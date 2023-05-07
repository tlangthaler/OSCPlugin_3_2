using LumosLIB.Kernel.Log;
using System;
namespace OSCGUIPlugin
{
	internal static class OscContextManager 
	{
        private static OSCInformation info;
        private static OscDeviceInformation dev;
		private static OscAssemblyHelper asm;
		public static OSCInformation OSCInformation
		{
			get
			{
				return OscContextManager.info;
			}
			set
			{
				if (OscContextManager.info != null)
				{
					throw new Exception("OSCInformation already set");
				}
				OscContextManager.info = value;
			}
		}
		
		public static OscDeviceInformation DeviceInformation
		{
			get
			{
				return OscContextManager.dev;
			}
			set
			{
				if (OscContextManager.dev != null)
				{
					throw new Exception("DeviceInformation already set");
				}
				OscContextManager.dev = value;
			}
		}
		
		public static OscAssemblyHelper AssemblyHelper
		{
			get
			{
				return OscContextManager.asm;
			}
			set
			{
				if (OscContextManager.asm != null)
				{
					throw new Exception("AssemblyHelper already set");
				}
				OscContextManager.asm = value;
			}
		}
		public static OSCForm OscForm
		{
			get;
			set;
		}

        public static ILumosLog Log
        {
            get;
            set;
        }

        static OscContextManager()
        {
        }
	}

}
