using Lumos.GUI.MIDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OSCGUIPlugin.Midi
{
    [FriendlyName("MidiClock")]
    public class MidiClockRule : DeviceRule
    {

        public MidiClockRule():base()
        {
            this.UseBacktrack = false;
        }
        ButtonInputChannel input = null;
        public override string ControlType
        {
            get
            {
                return "MidiClock";
            } 
        }
        private bool state;
        public bool State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        public override double Value 
        { 
            get
            {
                return State ? 1 : 0;
            }
            set
            {
                if (value < 0.5)
                {
                    State = false;
                }
                else
                {
                    State = true;
                }

            }
        }
        public override string LearnStatus 
        { 
            get 
            { 
                return "Learn not possible"; 
            } 
            protected set
            {
                LearnStatus = "Learn not possible";
            }
        }

        public override event EventHandler LearningFinished;

        public override void BeginLearn()
        {
            LearningFinished(this, new EventArgs());
            return;
        }

        public override void CancelLearn()
        {
            return;
        }

        public override MidiInputChannel GetInputChannel(RuleSet parent)
        {
            if (input == null)
            {
                input = new ButtonInputChannel(parent, this);
            }
            return input;
        }

        public override MidiOutputChannel GetOutputChannel(RuleSet parent)
        {
            return null;
        }

        private int counter = 0;
        public override void Process(MidiMessage m)
        {
            if (m.Channel == 1 && m.Message == 248)
            {
                counter++;
                //Clock triggered --> invert State
                if (counter >= 12)
                {
                    State = !State;
                    base.OnValueChanged();
                    counter = 0;
                }
            }
        }

 
        public override bool TryLearnMessage(MidiMessage m)
        {
            return true;
        }

        public override void UpdateBacktrack()
        {
            //No Backtrack;
        }

        protected override void Deserialize(XElement item)
        {
            return;
        }

        protected override void Serialize(XElement item)
        {
            return;
        }
    }
}
