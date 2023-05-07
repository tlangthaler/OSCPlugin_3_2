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
    [FriendlyName("Text")]
	public class OscTextRule : OscDeviceRule
	{
		private const string nolearn = "LearnMode not possible.";
		private string text;
		public override event EventHandler LearningFinished;
		public override string ControlType
		{
			get
			{
				return "Text";
			}
		}
		public string Text
		{
			get
			{
				return this.text;
			}
			private set
			{
				this.text = value;
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
		public override double Value
		{
			get
			{
				return 0;
			}
			set
			{
				this.Value = 0;
			}
		}
		public OscTextRule():base()
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
			if (i.hasValue<bool>("Text"))
			{
				this.Text = i.getValue<string>("Text");
			}
		}
		public override void Save(ManagedTreeItem i)
		{
			base.Save(i);
			i.setValue<string>("Text", this.Text);
		}
		protected override void Serialize(XElement item)
        {
        }

        protected override void Deserialize(XElement item)
        {
        }

		public override bool UpdateValue(object newValue)
		{
			if (newValue != null)
			{
				Text = newValue is double @double ? @double.ToString("0.##") : newValue.ToString();
				if (this.TagName != null && this.TagName.Length > 0)
				{
					base.OnSendMessage(new OscElement(this.TagName, new object[1] { this.Text }));
				}
				base.OnValueChanged(OscEChannelType.Default);
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
			this.OutputChannels.Add(new OscOutputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.STRING));
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
