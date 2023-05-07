using org.dmxc.lumos.Kernel.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using TouchOscLayoutParser.Enumeration;
using TouchOscLayoutParser.Page.Control;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
    public abstract class OscDeviceRule : IOscLearnable, IOscProcessable, IOscSave
    {
        private string name = "MyTestRule"; //set name to "" to ensure it won't get null
        private string tagname = "";
        private OscEColor color = OscEColor.Gray;
        private OscRuleSet ruleSet;

        public event EventHandler NameChanged;
        public event EventHandler<OscValueChangedEventArgs> ValueChanged;
        public abstract event EventHandler LearningFinished;
        public event EventHandler<OSCEventArgs> OSCMessageSend;
        private readonly List<OscInputChannel> inputChannels = new List<OscInputChannel>();
        private readonly List<OscOutputChannel> outputChannels = new List<OscOutputChannel>();
        protected abstract object GetChannelValue(OscEChannelType channelType);

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.UpdateIOChannels();
                if (this.NameChanged != null)
                {
                    NameChanged(this, null);
                }
            }
        }
        public abstract string ControlType
        {
            get;
        }
        public string TagName
        {
            get
            {
                return this.tagname;
            }
            set
            {
                this.tagname = value;
            }
        }
        public abstract double Value
        {
            get;
            set;
        }
        public abstract string LearnStatus
        {
            get;
            protected set;
        }
        public OscEColor Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }
        [Browsable(false)]
        public string GUID
        {
            get;
            set;
        }
        [Browsable(false)]
        public OscRuleSet RuleSet
        {
            get
            {
                return this.ruleSet;
            }
            set
            {
                this.ruleSet = value;
                this.ruleSet.NameChanged += (s, e) => this.UpdateIOChannels();

            }
        }

        internal List<OscOutputChannel> OutputChannels => outputChannels;
        internal List<OscInputChannel> InputChannels => inputChannels;

        public OscDeviceRule()
        {
            this.GUID = Guid.NewGuid().ToString();
        }
        protected void OnValueChanged(OscEChannelType actChannelType)
        {
            if (this.ValueChanged != null)
            {
                switch (actChannelType)
                {
                    case OscEChannelType.X:
                    case OscEChannelType.Y:
                    case OscEChannelType.XY:
                    case OscEChannelType.Touched:
                    case OscEChannelType.Object:
                        {
                            this.ValueChanged(this, new OscValueChangedEventArgs
                            {
                                NewValue = this.GetChannelValue(actChannelType),
                                ChannelType = actChannelType
                            });
                            break;
                        }
                    default:
                        {
                            this.ValueChanged(this, new OscValueChangedEventArgs
                            {
                                NewValue = this.Value,
                                ChannelType = actChannelType
                            });
                            break;
                        }
                }
            }
        }
        public bool UpdateValueColor(OscEColor newValue)
        {
            if (this.Color != newValue)
            {
                this.Color = newValue;
                //Send new Color to Component
                            if (this.TagName != null && this.TagName.Length > 0)
                {
                    this.OnSendMessage(new OscElement(this.TagName + "/color", new object[1] { this.Color.ToString().ToLower() }));
                }
            }
            return true;
        }

        public static OscDeviceRule Load(OscRuleSet r, ManagedTreeItem item)
        {
            OscDeviceRule result;
            if (!item.hasValue<string>("Type") && !item.hasValue<Type>("Type"))
            {
                result = null;
            }
            else
            {
                string type;
                if (item.hasValue<string>("Type"))
                    type = item.getValue<string>("Type");
                else
                    type = item.getValue<Type>("Type").FullName; //Backward compat.
                OscDeviceRule o = r.CreateRule(type);
                if (o == null)
                {
                    result = null;
                }
                else
                {
                    o.LoadGUID(item);
                    r.AddRule(o);
                    o.Init(item);
                    result = o;
                }
            }
            return result;
        }
        public abstract void BeginLearn();
        public abstract void CancelLearn();
        private void LoadGUID(ManagedTreeItem i)
        {
            if (i.hasValue<string>("GUID"))
            {
                this.GUID = i.getValue<string>("GUID");
            }
        }
        public virtual void Init(ManagedTreeItem i)
        {
            if (i.hasValue<string>("Name"))
            {
                this.Name = i.getValue<string>("Name");
            }
            if (i.hasValue<string>("TagName"))
            {
                this.TagName = i.getValue<string>("TagName");
            }
            if (i.hasValue<OscEColor>("Color"))
            {
                this.Color = GetColor(i.getValue<string>("Color"));
            }
        }
        public virtual void Save(ManagedTreeItem i)
        {
            Type type = base.GetType();
            i.setValue<string>("Type", type.FullName);
            i.setValue<string>("Name", this.Name);
            i.setValue<string>("GUID", this.GUID);
            i.setValue<string>("TagName", this.TagName);
            i.setValue<string>("Color", this.Color.ToString());
        }

        public XElement Serialize()
        {
            var xElement = new XElement("Rule");
            var type = GetType();
            xElement.Add(new XElement("Type", type.FullName), new XAttribute("Name", this.Name), new XAttribute("TagName", this.TagName), new XAttribute("Color", this.Color.ToString()));
            Serialize(xElement);
            return xElement;
        }

        protected abstract void Serialize(XElement item);

        protected abstract void Deserialize(XElement item);
        protected abstract void DecodeTouchOscControl(TouchOscControl item);
        private static OscEColor GetColor(string colorname)
        {
            switch(colorname)
            {
                case "Red": return OscEColor.Red;
                case "Green": return OscEColor.Green;
                case "Blue": return OscEColor.Blue;
                case "Yellow": return OscEColor.Yellow;
                case "Purple": return OscEColor.Purple;
                case "Gray": return OscEColor.Gray;
                case "Orange": return OscEColor.Orange;
                case "Brown": return OscEColor.Brown;
                case "Pink": return OscEColor.Pink;
                default: return OscEColor.Gray;
            }
        }
        private static OscEColor GetColor(EColor color)
        {
            switch (color)
            {
                case EColor.Red: return OscEColor.Red;
                case EColor.Green: return OscEColor.Green;
                case EColor.Blue: return OscEColor.Blue;
                case EColor.Yellow: return OscEColor.Yellow;
                case EColor.Purple: return OscEColor.Purple;
                case EColor.Gray: return OscEColor.Gray;
                case EColor.Orange: return OscEColor.Orange;
                case EColor.Brown: return OscEColor.Brown;
                case EColor.Pink: return OscEColor.Pink;
                default: return OscEColor.Gray;
            }
        }

        public static OscDeviceRule LoadFromXml(XElement item,OscRuleSet ruleSet)
        {
            var type = OscContextManager.AssemblyHelper.DeviceRuleTypes.FirstOrDefault(j => j.FullName == item.Element("Type").Value);
            if (type == null) return null;
            var obj = Activator.CreateInstance(type) as OscDeviceRule;
            obj.Deserialize(item);
            obj.RuleSet = ruleSet;
            obj.name = item.Attribute("Name").Value;
            obj.TagName = item.Attribute("TagName").Value;
            obj.Color = GetColor(item.Attribute("Color").Value);
            return obj;
        }
        public static OscDeviceRule LoadFromTouchOsc(TouchOscControl item,int xPosition,int yPosition,OscRuleSet ruleSet)
        {
            OscContextManager.Log.Debug("try Creating rule for control: " + item.Name + " xPosition: " + xPosition + " yPosition: " + yPosition);

            string nameExtension = "";
            string tagExtension = "";
            if (xPosition > 0)
            {
                nameExtension = "_" + xPosition;
                tagExtension = "/" + xPosition;
            }
            if (yPosition>0)
            {
                nameExtension = nameExtension +  "_" + yPosition;
                tagExtension = tagExtension + "/" + yPosition;
            }
            Type type = OscContextManager.AssemblyHelper.DeviceRuleTypes.FirstOrDefault(j => j.FullName == OscContextManager.AssemblyHelper.GetMappedTypeName(item.ControlType));
            if (type == null) return null;
            var obj = Activator.CreateInstance(type) as OscDeviceRule;
            obj.RuleSet = ruleSet;
            obj.DecodeTouchOscControl(item);
            obj.name = item.Name + nameExtension;
            obj.TagName = item.OscTag.TagName + tagExtension;
            obj.Color = GetColor(item.Color);
            return obj;
        }

        public bool UpdateFromTouchOsc(TouchOscControl item, int xPosition, int yPosition,bool isMultiItem)
        {
            string tagExtension = "";
            if (xPosition > 0)
            {
                tagExtension = "/" + xPosition;
            }
            if (yPosition > 0)
            {
                tagExtension = tagExtension + "/" + yPosition;
            }
            this.DecodeTouchOscControl(item);
            this.TagName = item.OscTag.TagName + tagExtension;
            this.Color = GetColor(item.Color);
            if (this is IOscMultiItem multi)
            {
                multi.IsMultiTouchItem = isMultiItem;
                multi.MultiItemName = item.Name;
            }
            this.UpdateIOChannels();
            return true;
        }

        internal void OnSendMessage(OscElement msg)
        {
            if (this.OSCMessageSend != null)
            {
                OSCMessageSend(this, new OSCEventArgs{ m = msg});
            }
        }

        public abstract bool TryLearnMessage(OscElement m);

        public abstract void Process(OscElement m);
        public abstract bool UpdateValue(object newValue);
        public abstract bool UpdateIOChannels();
    }
}
