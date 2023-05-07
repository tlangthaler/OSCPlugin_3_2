using LumosLIB.Kernel.Log;
using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
	public class OscDeviceInformation : IDisposable
	{
		private static readonly ILumosLog log = LumosLogger.getInstance(typeof(OscDeviceInformation));
		private bool disposed;
        readonly ServiceDiscovery sd = new ServiceDiscovery();
        ServiceProfile service;
        public BindingList<OscInput> InputDevices
        {
			get;
			private set;
		}
		public BindingList<OscOutput> OutputDevices
		{
			get;
			private set;
		}
		public List<OscDev> Devices
		{
			get
			{
				return this.InputDevices.Cast<OscDev>().Concat(this.OutputDevices.Cast<OscDev>()).ToList<OscDev>();
			}
		}
		public void Start()
		{
			foreach (OscInput d in this.InputDevices)
			{
				d.Start();
                //Activate Service Publisher
                service = new ServiceProfile(d.DeviceID.hostname + " - DmxC3", "_osc._udp", (ushort)d.DeviceID.port);
                sd.Advertise(service);

                //Discover other Clients
                sd.ServiceInstanceDiscovered += (s, serviceName) =>
                {
                    ServiceInstanceDiscoveryEventArgs args = (ServiceInstanceDiscoveryEventArgs)serviceName;
                    if (args.ServiceInstanceName.ToString().Contains("_osc._udp"))
                    {
                        string ipadr = "";
                        string hostname = "";
                        int port = 0;
                        foreach (ResourceRecord record in args.Message.AdditionalRecords)
                        {
                            if (record is ARecord record1)
                            {
                                ipadr = record1.Address.ToString();
                                hostname = record1.Name.ToString();

                            }
                            if (record is SRVRecord record2)
                            {
                                port = record2.Port;
                            }
                        }
                        // Check necessary creation


                        AddDevice(ipadr, hostname, port,true);
                    }

                };
                sd.QueryServiceInstances("_osc._udp");
            }
            //Publish OSC Device

		}

        public void AddDevice(string ipadr, string hostname, int port,bool autodetected)
        {
            if (port > 0 && ipadr.Length > 0)
            {
                //Check writer already available or is it the local InputDevice
                bool found = false;
                foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    IPInterfaceProperties ipProps = netInterface.GetIPProperties();
                    foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                    {
                        foreach (OscInput d in this.InputDevices)
                        {

                            //log.Debug("Local found IP-Adress: " + addr.Address.ToString(), new object[0]);
                            if (addr.Address.ToString().Equals(ipadr) && port == d.DeviceID.port)
                            {
                                found = true;
                            }
                        }
                    }
                }
                foreach (OscOutput output in this.OutputDevices)
                {
                    if (output.DeviceID.hostname.Equals(hostname) && output.DeviceID.ipaddress.Equals(ipadr) && output.DeviceID.port == port)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    this.OutputDevices.Add(new OscOutput(hostname, ipadr, port, autodetected));
                    OscContextManager.OscForm.UpdateUi();
                }
            }
        }
        public void DeleteDevice(string ipadr,string hostname,int port)
        {
            OscOutput toDelete = null;
            foreach(OscOutput output in this.OutputDevices)
            {
                if (output.DeviceID.ipaddress.Equals(ipadr) && output.DeviceID.hostname.Equals(hostname) && output.DeviceID.port == port)
                {
                    toDelete = output;
                }
            }
            if (toDelete != null)
            {
                toDelete.Dispose();
                this.OutputDevices.Remove(toDelete);
            }
        }

        public void Stop()
		{
			foreach (OscInput d in this.InputDevices)
			{
				d.Stop();
			}
		}

        private void DeviceAdd()
        {
            this.InputDevices = new BindingList<OscInput>();
            this.OutputDevices = new BindingList<OscOutput>();

            //Create Input Device
            OscInput input = new OscInput();
            this.InputDevices.Add(input);



        }

        private void DeviceDispose()
        {
            foreach (OscInput item in this.InputDevices)
            {
                try
                {
                    item.Dispose();
                }
                catch (Exception e)
                {
                    OscDeviceInformation.log.Warn("OSC-In Device could not be disposed", e, new object[0]);
                }
            }
            foreach (OscOutput item2 in this.OutputDevices)
            {
                try
                {
                    item2.Dispose();
                }
                catch (Exception e)
                {
                    OscDeviceInformation.log.Warn("OSC-Out Device could not be disposed", e, new object[0]);
                }
            }

            this.InputDevices = null;
            this.OutputDevices = null;
        }

        public void DeviceUpdate()
        {
            sd.QueryServiceInstances("_osc._udp");

        }

        public OscDeviceInformation()
		{
			OscContextManager.DeviceInformation = this;
            this.DeviceAdd();
		}
		public void Dispose()
		{
			Monitor.Enter(this);
			try
			{
				if (!this.disposed)
				{
					this.disposed = true;

                    this.DeviceDispose();
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
		}
	}
}
