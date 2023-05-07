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
    class OscOutputChannel : AbstractInputSink
    {
        protected OscDeviceRule rule;
        private readonly EWellKnownInputType type;
        public OscOutputChannel(string DisplayName,OscDeviceRule rule, ParameterCategory category, EWellKnownInputType type) : base(rule.GUID + DisplayName + "O", DisplayName, new ParameterCategory("OSC",category))
        {
            this.rule = rule;
            this.type = type;
        }

        public override EWellKnownInputType AutoGraphIOType => this.type;

        public override object Min => 0;

        public override object Max => 1;

        public override bool UpdateValue(object newValue)
        {
            return rule.UpdateValue(newValue);
        }
    }
}
