using Lumos.GUI.Input.v2;
using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Resource;
using System;
using System.Globalization;
using System.Xml.Linq;
using TouchOscLayoutParser.Page.Control;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
    [FriendlyName("Encoder")]
	public class OscEncoderRule : OscDeviceRule
	{
        private static readonly NumberFormatInfo nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };
		private const string nolearn = "LearnMode disabled.";
		private const string learn1 = "Turn the encoder clockwise.";
		private double value;
		private bool touched;
		public override event EventHandler LearningFinished;
		public double Increment
		{
			get;
			set;
		}
		public override string ControlType
		{
			get
			{
				return "Encoder";
			}
		}
		public override double Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = Math.Min(Math.Max(value,MinValue),MaxValue);
			}
		}
		public bool Touched
		{
			get
			{
				return this.touched;
			}
			set
			{
				this.touched = value;
			}
		}

		public double MinValue
		{
			get;
			set;
		}
		public double MaxValue
		{
			get;
			set;
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
		public OscEncoderRule():base()
		{
			this.LearnStatus = nolearn;
		}
		public override void Init(ManagedTreeItem i)
		{
			base.Init(i);
			if (i.hasValue<double>("Increment"))
			{
				this.Increment = i.getValue<double>("Increment");
			}

			if (i.hasValue<double>("Value"))
			{
				this.Value = i.getValue<double>("Value");
			}
			if (i.hasValue<bool>("Touched"))
			{
				this.Touched = i.getValue<bool>("Touched");
			}
		}
		public override void Save(ManagedTreeItem i)
		{

            //ContextManager.log.Debug("Saving EncoderRule {0}, {1},  {2}", CWMessage.Data, CCWMessage.Data, Increment);
			base.Save(i);
			i.setValue<double>("Value", this.Value);
			i.setValue<double>("Increment", this.Increment);
			i.setValue<bool>("Touched", this.Touched);
		}

		protected override void Serialize(XElement item)
        {
			item.Add(new XAttribute("Increment", this.Increment.ToString(nfi)));
        }
        protected override void Deserialize(XElement item)
        {
            this.Increment = double.Parse(item.Attribute("Increment").Value, nfi);
        }
        public override void Process(OscElement m)
		{
			// Learn
			if (!this.TryLearnMessage(m))
			{
				if (m.Address.Equals(this.TagName))
				{
					if ((float)m.Args[0] == 1)
					{

						this.Value += this.Increment;
						base.OnValueChanged(OscEChannelType.Default);
					}
					if ((float)m.Args[0] == 0)
					{
						this.Value -= this.Increment;
						base.OnValueChanged(OscEChannelType.Default);
					}
				}
				if (m.Address.Equals(this.TagName + "/z"))
                {
					Touched = ((float)m.Args[0] > 0.5);
					base.OnValueChanged(OscEChannelType.Touched);
                }
			}
			//OscContextManager.Log.Debug("New Value {0}", this.Value);

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
				this.EndLearn();
				result = true;
			}
			return result;
		}

        public override bool UpdateValue(object newValue)
        {
            if (newValue is double @double && !this.Value.Equals(newValue))
            {
				this.Value = @double;
				base.OnValueChanged(OscEChannelType.Default);
				return true;
            }

			return false;
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
			this.InputChannels.Add(new OscInputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC));
			this.InputChannels.Add(new OscXYInputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC, OscEChannelType.Touched));
			//Add OscOutputChannel
			this.OutputChannels.Add(new OscOutputChannel(this.Name,this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.NUMERIC));
			this.OutputChannels.Add(new OscXYOutputChannel(this.Name, this, new ParameterCategory(this.RuleSet.Name, new ParameterCategory(this.Name)), EWellKnownInputType.COLOR, OscEChannelType.Color));

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
