using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Naming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCGUIPlugin
{
    class OscInputChannel : AbstractInputSource
    {
        private readonly EWellKnownInputType type;
        public OscInputChannel(string DisplayName,OscDeviceRule rule, ParameterCategory category,EWellKnownInputType type) : base(rule.GUID + DisplayName + "I", DisplayName, new ParameterCategory("OSC",category))
        {
            this.type = type;
            rule.ValueChanged += ValueChanged;
            
        }

        public override EWellKnownInputType AutoGraphIOType => this.type;

        public override object Min => 0;

        public override object Max => 1;

        protected virtual void ValueChanged(object sender, OscValueChangedEventArgs e)
        {
            if (e.ChannelType == OscEChannelType.Default || e.ChannelType == OscEChannelType.Object)
            {
                this.OnValueProduced(e.NewValue);
            }
        }

    }
}
