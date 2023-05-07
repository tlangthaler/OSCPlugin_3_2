using Lumos.GUI.Input.v2;
using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using System;
using System.Xml.Linq;
using TouchOscLayoutParser.Page.Control;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
    [FriendlyName("Fader/Rotary")]
    public class OscSliderRule : OscDeviceRule,IOscMultiItem
    {

 
        private const string nolearn = "LearnMode disabled.";
        private const string learn1 = "Move the fader.";



        public override event EventHandler LearningFinished;

        public override string ControlType
        {
            get { return "Fader"; }
        }

        private double value=0.0;
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
                touched = value;
            }
        }
        private bool isMultiTouchFader = false;
        public bool IsMultiTouchItem
        {
            get
            {
                return isMultiTouchFader;
            }
            set 
            {
                isMultiTouchFader = value;
                this.UpdateIOChannels();
            }
        }
        private string multiItemName = "";
        public string MultiItemName
        {
            get
            {
                return multiItemName;
            }
            set
            {
                multiItemName = value;
                this.UpdateIOChannels();
            }
        }
        public bool LearnMode
        {
            get;
            private set;
        }
        public OscSliderRule() : base()
        {
            this.LearnStatus = nolearn;
        }

        public override void Process(OscElement m)
        {
            // Learn
            if (!this.TryLearnMessage(m))
            {
                if (m.Address.Equals(this.TagName))
                {
                    this.Value = (float)m.Args[0];
                    base.OnValueChanged(OscEChannelType.Default);
                }
                if (m.Address.Equals(this.TagName + "/z"))
                {
                    this.Touched = (float)m.Args[0] > 0.5;
                    base.OnValueChanged(OscEChannelType.Touched);
                }
                if (IsMultiTouchItem)
                {
                    if (m.Address.Equals(this.TagName.Substring(0,this.TagName.LastIndexOf("/")) + "/z"))
                    {
                        this.Touched = (float)m.Args[0] > 0.5;
                        base.OnValueChanged(OscEChannelType.Touched);
                    }
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
            if(LearnMode)
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

            if (i.hasValue<double>("Value"))
            {
                this.Value = i.getValue<double>("Value");
            }
            if (i.hasValue<bool>("Touched"))
            {
                this.Touched = i.getValue<bool>("Touched");
            }
            if (i.hasValue<bool>("IsMultiTouchItem"))
            {
                this.IsMultiTouchItem = i.getValue<bool>("IsMultiTouchItem");
            }
            if (i.hasValue<string>("MultiItemName"))
            {
                this.MultiItemName = i.getValue<string>("MultiItemName");
            }

        }

        public override void Save(org.dmxc.lumos.Kernel.Resource.ManagedTreeItem i)
        {
            //ContextManager.log.Debug("Saving SliderRule {0}, {1},  {2}", SliderMessage.Data, MaximumBacktrack.Data, MinimumBacktrack.Data);
            base.Save(i);
            i.setValue<double>("Value", this.Value);
            i.setValue<bool>("Touched", this.Touched);
            i.setValue<bool>("IsMultiTouchItem", this.IsMultiTouchItem);
            i.setValue<string>("MultiItemName", this.MultiItemName);
        }

        protected override void Serialize(XElement item)
        {
            item.Add(new XAttribute("IsMultiTouchItem", this.IsMultiTouchItem));
            item.Add(new XAttribute("MultiItemName", this.MultiItemName));
        }

        protected override void Deserialize(XElement item)
        {
            this.IsMultiTouchItem = bool.Parse(item.Attribute("IsMultiTouchItem").Value);
            this.MultiItemName = item.Attribute("MultiItemName").Value;
        }

        public override bool UpdateValue(object newValue)
        {
            if (newValue != null && !this.Value.Equals(newValue))
            {
                this.Value = (double)newValue;
                base.OnValueChanged(OscEChannelType.Default);
            }
            if (this.TagName != null && this.TagName.Length > 0)
            {
                base.OnSendMessage(new OscElement(this.TagName, new object[1] { (float)this.Value }));
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
            //Generate GroupName
            string groupingName = this.Name;
            if (this.IsMultiTouchItem && this.MultiItemName != null && this.MultiItemName.Length > 0)
            {
                groupingName = this.MultiItemName;
            }

            //Add OscInputChannel
            this.InputChannels.Add(new OscInputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(groupingName)), EWellKnownInputType.NUMERIC));
            //Add OscOutputChannel
            this.OutputChannels.Add(new OscOutputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(groupingName)), EWellKnownInputType.NUMERIC));

            //Check if is first Element of Multitouch
            if ((this.IsMultiTouchItem && int.TryParse(this.TagName.Substring(this.TagName.LastIndexOf("/") + 1), out int result) && result == 1) || !this.IsMultiTouchItem)
            {
                this.InputChannels.Add(new OscXYInputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(groupingName)), EWellKnownInputType.BOOL, OscEChannelType.Touched));
                this.OutputChannels.Add(new OscXYOutputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(groupingName)), EWellKnownInputType.COLOR, OscEChannelType.Color));
            }

            //Register new Entries
            InputManager.getInstance().RegisterSources(this.InputChannels);
            InputManager.getInstance().RegisterSinks(this.OutputChannels);

            return true;

        }

        protected override object GetChannelValue(OscEChannelType channelType)
        {
            if (channelType == OscEChannelType.Touched)
            {
                return Touched;
            }

            return Value;
        }

        protected override void DecodeTouchOscControl(TouchOscControl item)
        {
            
        }
    }
}
