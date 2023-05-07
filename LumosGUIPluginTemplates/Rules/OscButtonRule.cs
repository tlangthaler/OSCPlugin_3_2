using Lumos.GUI.Input.v2;
using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Resource;
using org.dmxc.lumos.Kernel.SceneList;
using System;
using System.Xml.Linq;
using TouchOscLayoutParser.Page.Control;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
    [FriendlyName("Button")]
	public class OscButtonRule : OscDeviceRule, IOscMultiItem
	{
		private const string nolearn = "LearnMode disabled.";
		private const string learn1 = "Press the button now.";
		private bool state;
		private bool isToggle;
		private float treshold = (float)0.5;
		public override event EventHandler LearningFinished;
		public override string ControlType
		{
			get
			{
				return "Button";
			}
		}
		public bool State
		{
			get
			{
				return this.state;
			}
			private set
			{
				this.state = value;
			}
		}
		public float Treshold
		{
			get { return this.treshold; }
			set { this.treshold = value; }
		}
		public bool IsToggle
		{
			get { return this.isToggle; }
			set { this.isToggle = value; }
		}
		public override string LearnStatus
		{
			get;
			protected set;
		}
		public bool LearnMode
		{
			get;
			private set;
		}
		public override double Value
		{
			get
			{
				return (double)(this.State ? 1 : 0);
			}
			set
			{
				this.State = (value >= 0.5);
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

		public OscButtonRule():base()
		{
			this.LearnStatus = nolearn;
		}
		public override void BeginLearn()
		{
			this.LearnMode = true;
			this.LearnStatus = learn1;
		}
		public override void CancelLearn()
		{
			this.EndLearn();
		}
		private void EndLearn()
		{
			this.LearnMode = false;
			this.LearnStatus = nolearn;
			if (this.LearningFinished != null)
			{
				LearningFinished(this, EventArgs.Empty);
			}
		}
		public override bool TryLearnMessage(OscElement m)
		{
			bool result;
			if (!this.LearnMode)
			{
				result = false;
			}
			else
			{
					this.TagName = m.Address;
					this.Treshold = ((float)m.Args[0]/2);
					this.EndLearn();
				result = true;
			}
			return result;
		}
		public override void Process(OscElement m)
		{
			// Learn
			if (!this.TryLearnMessage(m))
			{
				if (m.Address.Equals(this.TagName))
				{
					//Is Message for this Button
					//Translate to bool
					bool newValue = ((float)m.Args[0] > this.Treshold);
					if (this.IsToggle)
					{
						if (newValue)
						{
							//Toggle Button on True Message
							this.State = !this.State;
							base.OnValueChanged(OscEChannelType.Default);
						}
					}
					else
					{
						if (this.State != newValue)
						{
							this.State = newValue;
							base.OnValueChanged(OscEChannelType.Default);
						}
					}
				}
			}
		}
		public override void Init(ManagedTreeItem i)
		{
			base.Init(i);
			if (i.hasValue<byte>("Treshold"))
			{
				this.Treshold = i.getValue<float>("Treshold");
			}
			if (i.hasValue<bool>("IsToggle"))
			{
				this.IsToggle = i.getValue<bool>("IsToggle");
			}
			if (i.hasValue<bool>("State"))
			{
				this.State = i.getValue<bool>("State");
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
		public override void Save(ManagedTreeItem i)
		{
			base.Save(i);
			i.setValue<bool>("State", this.State);
			i.setValue<float>("Treshold", this.Treshold);
			i.setValue<bool>("IsToggle", this.IsToggle);
			i.setValue<bool>("IsMultiTouchItem", this.IsMultiTouchItem);
			i.setValue<string>("MultiItemName", this.MultiItemName);
		}
		protected override void Serialize(XElement item)
        {
            item.Add(new XAttribute("Treshold", this.Treshold));
            item.Add(new XAttribute("IsToggle", this.IsToggle));
			item.Add(new XAttribute("IsMultiTouchItem", this.IsMultiTouchItem));
			item.Add(new XAttribute("MultiItemName", this.MultiItemName));

		}

		protected override void Deserialize(XElement item)
        {
            this.Treshold = byte.Parse(item.Attribute("Treshold").Value);
            this.IsToggle = bool.Parse(item.Attribute("IsToggle").Value);
			this.IsMultiTouchItem = bool.Parse(item.Attribute("IsMultiTouchItem").Value);
			this.MultiItemName = item.Attribute("MultiItemName").Value;
		}

		public override bool UpdateValue(object newValue)
        {
			if (newValue != null)
			{
				
				bool boolean = false;

				if (newValue is bool boolvalue)
				{
					boolean = boolvalue;
				}
				if (newValue is double doublevalue)
				{
					if (doublevalue > 0)
					{
						boolean = true;
					}
					else
					{
						boolean = false;
					}
				}
				if (newValue is int intvalue)
				{
					if (intvalue > 0)
					{
						boolean = true;
					}
					else
					{
						boolean = false;
					}
				}
				if (newValue is ESceneListState listvalue)
                {
					switch(listvalue)
                    {
						case ESceneListState.PAUSED:
						case ESceneListState.STOPPED:
                            {
								boolean = false;
								break;
                            }
						case ESceneListState.RUNNING:
                            {
								boolean = true;
								break;
                            }
						default:
                            {
								boolean = true;
								break;
                            }
                    }
                }
				if (this.State != boolean)
					{
						this.State = boolean;
						base.OnValueChanged(OscEChannelType.Default);
					}

				if (this.State)
				{
					if (this.TagName != null && this.TagName.Length > 0)
					{
						base.OnSendMessage(new OscElement(this.TagName, new object[1] { (float)1.0 }));
					}
				}
				else
				{
					if (this.TagName != null && this.TagName.Length > 0)
					{
						base.OnSendMessage(new OscElement(this.TagName, new object[1] { (float)0.0 }));
					}
				}
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
			this.InputChannels.Add(new OscInputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(groupingName)), EWellKnownInputType.BOOL));
			//Add OscOutputChannel
			this.OutputChannels.Add(new OscOutputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(groupingName)), EWellKnownInputType.BOOL));
			if (IsFirstMultiItemOrSingleItem())
			{
				this.OutputChannels.Add(new OscXYOutputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(groupingName)), EWellKnownInputType.COLOR, OscEChannelType.Color));
			}

			//Register new Entries
			InputManager.getInstance().RegisterSources(this.InputChannels);
			InputManager.getInstance().RegisterSinks(this.OutputChannels);

			return true;
		}

		private bool IsFirstMultiItemOrSingleItem()
        {
			if (this.IsMultiTouchItem)
			{
				// Button muss have 2 end numbers
				int lastindex = this.TagName.LastIndexOf("/");
				if (lastindex >0)
                {
					//Search second last index
					int secondindex = this.TagName.Substring(0, lastindex - 1).LastIndexOf("/");
					if (secondindex > 0)
                    {
						//2 found -- Check if 1 between
						if (int.TryParse(this.TagName.Substring(lastindex + 1),out int result) && result == 1 && int.TryParse(this.TagName.Substring(secondindex+1,lastindex - secondindex - 1),out int result1) && result1 == 1)
                        {
							return true;
                        }
                    }
                }
				return false;
			}
			else
            {
				// Not a MultiItem
				return true;
            }

		}

        protected override object GetChannelValue(OscEChannelType channelType)
        {
			return Value;
        }

        protected override void DecodeTouchOscControl(TouchOscControl item)
        {
			this.treshold = (float)((item.OscTag.RangeTo - item.OscTag.RangeFrom) / 2);
        }
    }
}
