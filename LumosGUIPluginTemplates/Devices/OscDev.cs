using System;
using System.ComponentModel;

namespace OSCGUIPlugin
{
	public abstract class OscDev
	{
		public OscEDeviceType DeviceType
		{
            get { return DeviceID.t; }
		}

		public string HostName
        {
            get { return DeviceID.hostname; }
        }

		public string IPAdress
		{
			get { return DeviceID.ipaddress; }
		}

		public int Port
		{
			get { return DeviceID.port; }
		}


		[Browsable(false)]
		public OscDeviceId DeviceID
		{
			get;
			protected set;
		}
		[Browsable(false)]
		public string DeviceName
		{
			get;
			protected set;
		}

		public bool Autodetected
		{
			get;
			protected set;
		}

	}
}
