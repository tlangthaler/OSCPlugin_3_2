using Lumos.GUI.Input.v2;
using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Resource;
using System;
using System.Xml.Linq;
using TouchOscLayoutParser.Page.Control;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
    [FriendlyName("LED")]
	public class OscLedRule : OscDeviceRule
	{
		private const string nolearn = "Learn not possible";
		public override event EventHandler LearningFinished;
		public override string ControlType
		{
			get
			{
				return "LED";
			}
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
				}
			}
		}
		public OscLedRule():base()
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
			this.LearnMode = false;
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
		public override void Process(OscElement m)
		{
			// No Input Possible
			// Learn
		}
		public override void Init(ManagedTreeItem i)
		{
			base.Init(i);
			if (i.hasValue<bool>("Value"))
			{
				this.Value = i.getValue<double>("Value");
			}
		}
		public override void Save(ManagedTreeItem i)
		{
			base.Save(i);
			i.setValue<double>("Value", this.Value);
		}
		protected override void Serialize(XElement item)
        {
        }

        protected override void Deserialize(XElement item)
        {
        }

        public override bool UpdateValue(object newValue)
        {
			if (newValue != null && !this.Value.Equals(newValue))
			{
				if (newValue is bool boolean)
				{
					this.Value = boolean ? 1.0 : 0.0;
				}
				else
				{
					this.Value = (double)newValue;
				}
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
			//InputManager.getInstance().UnregisterSources(this.InputChannels);
			this.InputChannels.Clear();
			InputManager.getInstance().UnregisterSinks(this.OutputChannels);
			this.OutputChannels.Clear();
			//Add OscInputChannel
			//this.InputChannels.Add(new OscInputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name), EWellKnownInputType.BOOL));
			//Add OscOutputChannel
			this.OutputChannels.Add(new OscOutputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC));
			this.OutputChannels.Add(new OscXYOutputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.COLOR, OscEChannelType.Color));

			//Register new Entries
			//InputManager.getInstance().RegisterSources(this.InputChannels);
			InputManager.getInstance().RegisterSinks(this.OutputChannels);

			return true;
		}

        protected override object GetChannelValue(OscEChannelType channelType)
        {
			return Value;
        }

        protected override void DecodeTouchOscControl(TouchOscControl item)
        {
        }
    }
}
