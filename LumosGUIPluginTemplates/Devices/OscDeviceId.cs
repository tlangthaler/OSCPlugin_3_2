using System;
namespace OSCGUIPlugin
{
	public class OscDeviceId : IEquatable<OscDeviceId>
	{
		public OscEDeviceType t;
		public int port;
		public string hostname;
		public string ipaddress;
		public override string ToString()
		{
			return this.t.ToString() + "/" + this.port + "/" + this.hostname + "(" + this.ipaddress + ")";
		}
		
		public bool Equals(OscDeviceId other)
		{
			return this.port == other.port && this.t == other.t && this.hostname.Equals(other.hostname) && this.ipaddress.Equals(other.ipaddress);
		}
	}
}
