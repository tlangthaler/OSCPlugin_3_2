using Lumos.GUI.Input.v2;
using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouchOscLayoutParser.Page.Control;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
    [FriendlyName("Virtual")]
    public class OscVirtualRule : OscDeviceRule
    {
        private const string nolearn = "LearnMode not possible.";

        public override string ControlType
        {
            get
            {
                return "Virtual";
            }
        }
        public override double Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string LearnStatus
        {
            get;
            protected set;
        }

        private object value;
        public object ObjectValue
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }

        }
        public OscVirtualRule() : base()
        {
            this.LearnStatus = nolearn;
        }
        public override void BeginLearn()
        {
            // Learn Mode not possible --> End again
            this.EndLearn();

        }
        public override void CancelLearn()
        {
            this.EndLearn();
        }
        private void EndLearn()
        {
            this.LearnStatus = nolearn;
            if (this.LearningFinished != null)
            {
                LearningFinished(this, EventArgs.Empty);
            }
        }
        public override bool TryLearnMessage(OscElement m)
        {
            // not possible
            return false;
        }

        public override event EventHandler LearningFinished;

        public override void Init(ManagedTreeItem i)
        {
            base.Init(i);
        }
        public override void Save(ManagedTreeItem i)
        {
            base.Save(i);
        }
        protected override void Serialize(XElement item)
        {
        }

        protected override void Deserialize(XElement item)
        {
        }

        public override bool UpdateValue(object newValue)
        {
            if (newValue != null && !newValue.Equals(this.ObjectValue))
            {
                this.ObjectValue = newValue;
                base.OnValueChanged(OscEChannelType.Object);
            }
            return true;
        }


        public override bool UpdateIOChannels()
        {
            //OscContextManager.log.Debug("UpdateIOChannels executed", new object[0]);
            //Remove Old Entries
            //Remove InputChannels
            InputManager.getInstance().UnregisterSources(this.InputChannels);
            this.InputChannels.Clear();
            InputManager.getInstance().UnregisterSinks(this.OutputChannels);
            this.OutputChannels.Clear();
            //Add OscInputChannel
            this.InputChannels.Add(new OscInputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.UNKNOWN));
            //Add OscOutputChannel
            this.OutputChannels.Add(new OscOutputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.UNKNOWN));

            //Register new Entries
            InputManager.getInstance().RegisterSources(this.InputChannels);
            InputManager.getInstance().RegisterSinks(this.OutputChannels);

            return true;
        }

        protected override object GetChannelValue(OscEChannelType channelType)
        {
            return ObjectValue;
        }

        protected override void DecodeTouchOscControl(TouchOscControl item)
        {
        }
        public override void Process(OscElement m)
        {
            //Nothing to do, becaus no Input from OSC, only from Output.
        }
    }
}
