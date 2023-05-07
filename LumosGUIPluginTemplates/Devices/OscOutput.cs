
using org.dmxc.lumos.Kernel.Resource;
using System;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
    public class OscOutput : OscDev, IDisposable
    {
		public UdpWriter OutputDevice
		{
			get;
			private set;
		}
		public OscOutput(string hostname, string ipaddress, int port,bool autodetected)
		{
            base.DeviceID = new OscDeviceId
            {
                hostname = hostname,
                ipaddress = ipaddress,
                port = port,
                t = OscEDeviceType.Out
            };
            base.DeviceName = base.DeviceID.ToString();
            base.Autodetected = autodetected;
			this.OutputDevice = new UdpWriter(ipaddress,port);
		}

        public void Dispose()
        {
			this.OutputDevice.Dispose();
        }

		public void Save(ManagedTreeItem mti)
		{
			OscContextManager.Log.Debug("Saving OutputDevice {0}", DeviceName);
			mti.setValue<string>("DeviceName", this.DeviceName);
			mti.setValue<string>("HostName", this.HostName);
			mti.setValue<string>("IpAddress", this.IPAdress);
			mti.setValue<int>("Port", this.Port);
		}
		public static OscOutput Load(ManagedTreeItem mti)
		{
			if (mti.hasValue<string>("HostName"))
			{
				string hostname = mti.getValue<string>("HostName");
				if (mti.hasValue<string>("IpAddress"))
				{
					string ipadr = mti.getValue<string>("IpAddress");
					if (mti.hasValue<string>("IpAddress"))
					{
						int port = mti.getValue<int>("Port");
						return new OscOutput(hostname, ipadr, port, false);
					}
				}
			}
			return null;
		}

		public void UpdateDevice(string HostName, string IpAddress, int Port)
        {
			this.DeviceID.hostname = HostName;
			this.DeviceID.ipaddress = IpAddress;
			this.DeviceID.port = Port;
			base.DeviceName = base.DeviceID.ToString();
			//Stop existing UdpWriter
			this.OutputDevice.Dispose();
			this.OutputDevice = new UdpWriter(IpAddress, Port);

		}

	}
}
