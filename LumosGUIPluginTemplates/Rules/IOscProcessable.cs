using System;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
	public interface IOscProcessable
    {
        event EventHandler<OSCEventArgs> OSCMessageSend;
		double Value
		{
			get;
		}
		void Process(OscElement m);
		bool UpdateValue(object newValue);
		bool UpdateIOChannels();
	}
}
