using Lumos.GUI.Input.v2;
using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.PropertyType;
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
    [FriendlyName("X/Y")]
    public class OscXYRule : OscDeviceRule
    {
        private const string nolearn = "LearnMode disabled.";
        private const string learn1 = "Move the fader.";

        public override string ControlType
        {
            get
            {
                return "XY";
            }
        }

        private double value = 0.0;
        public override double Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value != this.value)
                {
                    this.value = value;
                    SendActualValues();
                }
            }
        }

        private double valuex = 0.0;
        public double ValueX
        {
            get
            {
                return valuex;
            }
            set
            {
                if (value != this.valuex)
                {
                    this.valuex = value;
                    SendActualValues();
                }
            }
        }
        private double valuey = 0.0;
        public double ValueY
        {
            get
            {
                return valuey;
            }
            set
            {
                if (value != this.valuey)
                {
                    this.valuey = value;
                    SendActualValues();
                }
            }
        }
        private bool touched = false;
        public bool Touched
        {
            get
            {
                return touched;
            }
            set 
            {
                this.touched = value;
            }
        }
        public bool LearnMode
        {
            get;
            private set;
        }

        public override event EventHandler LearningFinished;

        public OscXYRule() : base()
        {
            this.LearnStatus = nolearn;
        }

        public void SendActualValues()
        {
                        if (this.TagName != null && this.TagName.Length > 0)
            {
                base.OnSendMessage(new OscElement(this.TagName, new object[2] { (float)this.ValueY, (float)this.ValueX }));
            }
        }

        public override void Process(OscElement m)
        {
            // Learn
            if (!this.TryLearnMessage(m))
            {
                if (m.Address.Equals(this.TagName))
                {
                    //Decode X/Y Value
                    if (m.Args.Length == 2)
                    { 
                    this.ValueY = (float)m.Args[0];
                    this.ValueX = (float)m.Args[1];
                        base.OnValueChanged(OscEChannelType.X);
                        base.OnValueChanged(OscEChannelType.Y);
                        base.OnValueChanged(OscEChannelType.XY);
                    }
                }
                if (m.Address.Equals(this.TagName + "/z"))
                {
                    this.Touched = (float)m.Args[0] > 0.5;
                    base.OnValueChanged(OscEChannelType.Touched);
                }
            }
        }

        public override void BeginLearn()
        {
            LearnMode = true;
            LearnStatus = learn1;
        }

        public override string LearnStatus
        {
            get;
            protected set;
        }

        public override void CancelLearn()
        {
            LearnMode = false;
            LearnStatus = nolearn;
            if (this.LearningFinished != null)
            {
                LearningFinished(this, EventArgs.Empty);
            }

        }

        public override bool TryLearnMessage(OscElement m)
        {
            if (LearnMode)
            {
                this.TagName = m.Address;
                LearnMode = false;
                LearnStatus = nolearn;
                if (this.LearningFinished != null)
                {
                    LearningFinished(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        public override void Init(org.dmxc.lumos.Kernel.Resource.ManagedTreeItem i)
        {
            base.Init(i);

            if (i.hasValue<double>("ValueX"))
            {
                this.ValueX = i.getValue<double>("ValueX");
            }
            if (i.hasValue<double>("ValueY"))
            {
                this.ValueY = i.getValue<double>("ValueY");
            }
            if (i.hasValue<bool>("Touched"))
            {
                this.Touched = i.getValue<bool>("Touched");
            }
        }

        public override void Save(org.dmxc.lumos.Kernel.Resource.ManagedTreeItem i)
        {
            //ContextManager.log.Debug("Saving SliderRule {0}, {1},  {2}", SliderMessage.Data, MaximumBacktrack.Data, MinimumBacktrack.Data);
            base.Save(i);
            i.setValue<double>("ValueX", this.Value);
            i.setValue<double>("ValueY", this.Value);
            i.setValue<bool>("Touched", this.Touched);
        }

        protected override void Serialize(System.Xml.Linq.XElement item)
        {
        }
        protected override void Deserialize(XElement item)
        {
        }


        public override bool UpdateValue(object newValue)
        {
            // Not Used in this case --> 2 different Methods for X/Y
            return false;
        }
        public bool UpdateValueX(object newValue)
        {
            if (newValue != null && !this.ValueX.Equals(newValue))
            {
                this.ValueX = (double)newValue;
                base.OnValueChanged(OscEChannelType.X);
            }
            SendActualValues();
            return true;
        }

        public bool UpdateValueY(object newValue)
        {
            if (newValue != null && !this.ValueY.Equals(newValue))
            {
                this.ValueY = (double)newValue;
                base.OnValueChanged(OscEChannelType.Y);
            }
            SendActualValues();
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
            this.InputChannels.Add(new OscXYInputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name,new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC, OscEChannelType.X));
            this.InputChannels.Add(new OscXYInputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC, OscEChannelType.Y));
            this.InputChannels.Add(new OscXYInputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.UNKNOWN, OscEChannelType.XY));
            this.InputChannels.Add(new OscXYInputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.BOOL, OscEChannelType.Touched));
            //Add OscOutputChannel
            this.OutputChannels.Add(new OscXYOutputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC, OscEChannelType.X));
            this.OutputChannels.Add(new OscXYOutputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC, OscEChannelType.Y));
            this.OutputChannels.Add(new OscXYOutputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.COLOR, OscEChannelType.Color));

            //Register new Entries
            InputManager.getInstance().RegisterSources(this.InputChannels);
            InputManager.getInstance().RegisterSinks(this.OutputChannels);
            
            return true;

        }

        protected override object GetChannelValue(OscEChannelType channelType)
        {
            switch(channelType)
            {
                case OscEChannelType.X:
                    return ValueX;
                case OscEChannelType.Y:
                    return ValueY;
                case OscEChannelType.XY:
                    return new Position(ValueX,ValueY);
                case OscEChannelType.Touched:
                    return Touched;
                default:
                    return Value;
            }
        }

        protected override void DecodeTouchOscControl(TouchOscControl item)
        {
        }
    }
}
