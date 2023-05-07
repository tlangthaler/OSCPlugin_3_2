using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
	public class OscInput : OscDev, IDisposable
	{
		private bool LearnMode;
		public event EventHandler<OSCEventArgs> MessageReceived;
		public event EventHandler<OSCEventArgs> LearnMessage;
		public UdpReader InputDevice
		{
			get;
			private set;
		}
		private Timer timer;

		public OscInput()
		{
            base.DeviceID = new OscDeviceId
            {
                ipaddress = "All",
                hostname = Dns.GetHostName(),
                t = OscEDeviceType.In
            };
            base.DeviceName = base.DeviceID.ToString();
			base.Autodetected = true;
			//Search Free Port

		}
		public void Start()
		{
			base.DeviceID.port = GetAvailablePort(8000);
			this.InputDevice = new UdpReader(this.DeviceID.port * -1);
			//Set and Create Timer
			this.timer = new Timer(1);
			this.timer.Elapsed += (Object source, ElapsedEventArgs e) => { OscMessageReceived(); };
			this.timer.AutoReset = false;
			this.timer.Enabled = true;
			this.timer.Start();


		}
		public void Stop()
		{
			this.timer.Stop();
		}

		private void OscMessageReceived()
		{
			timer.Stop();
			//Message Buffer per Tag
			Dictionary<string, OscElement> oscBuffer = new Dictionary<string, OscElement>();
			//Receive Messages and fill buffer
			OscMessage message = InputDevice.Receive();
			while (message != null)
			{ 
				try
				{
					if (message != null)
					{
						if (message is OscElement element)
                        {
							oscBuffer[element.Address] = element;
                        }
                    }
				}
				catch (Exception ex)
				{
					OscContextManager.Log.Error("Error processing incoming osc message", ex);
				}
				message = InputDevice.Receive();
			}
			ExecuteOscElementBuffer(oscBuffer);

			timer.Start();

		}

        private void ExecuteOscElementBuffer(Dictionary<string,OscElement> oscBuffer)
        {
			foreach (OscElement element in oscBuffer.Values)
			{
				if (this.LearnMode)
				{
					if (this.LearnMessage != null)
					{
						LearnMessage(this, new OSCEventArgs
						{
							m = element
						});
					}
				}
				else
				{
					if (this.MessageReceived != null)
					{
						MessageReceived(this, new OSCEventArgs
						{
							m = element
						});
					}
				}
			}
        }

        public void EnterLearnMode()
		{
			this.LearnMode = true;
		}
		public void LeaveLearnMode()
		{
			this.LearnMode = false;
		}
		public void Dispose()
		{
			this.timer.Dispose();
			this.InputDevice.Dispose();
		}


		public static int GetAvailablePort(int startingPort)
		{
			IPEndPoint[] endPoints;
			List<int> portArray = new List<int>();

			IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

			//getting active connections
			TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
			portArray.AddRange(from n in connections
							   where n.LocalEndPoint.Port >= startingPort
							   select n.LocalEndPoint.Port);

			//getting active tcp listners - WCF service listening in tcp
			endPoints = properties.GetActiveTcpListeners();
			portArray.AddRange(from n in endPoints
							   where n.Port >= startingPort
							   select n.Port);

			//getting active udp listeners
			endPoints = properties.GetActiveUdpListeners();
			portArray.AddRange(from n in endPoints
							   where n.Port >= startingPort
							   select n.Port);

			portArray.Sort();

			for (int i = startingPort; i < UInt16.MaxValue; i++)
				if (!portArray.Contains(i))
					return i;

			return 0;
		}
	}
}
