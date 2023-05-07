using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCGUIPlugin
{
    class OscXYInputChannel : OscInputChannel
    {
        readonly OscEChannelType channelType;     
        public OscXYInputChannel(string DisplayName,OscDeviceRule rule, ParameterCategory category, EWellKnownInputType type,OscEChannelType channelType):base(DisplayName + "_" + channelType,rule,category,type)
        {
            this.channelType = channelType;
            rule.ValueChanged += ValueChanged;
        }

        protected override void ValueChanged(object sender, OscValueChangedEventArgs e)
        {
            if (this.channelType == e.ChannelType)
            {
                this.OnValueProduced(e.NewValue);
            }
        }

    }
}
