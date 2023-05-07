using System;
namespace OSCGUIPlugin
{
	public class OscValueChangedEventArgs : EventArgs
	{
		public object NewValue
		{
			get;
			set;
		}
		public OscEChannelType ChannelType
        {
			get;
			set;
        }
	}
}
