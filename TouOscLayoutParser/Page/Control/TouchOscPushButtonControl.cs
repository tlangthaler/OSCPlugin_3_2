using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouchOscLayoutParser.Enumeration;
using TouchOscLayoutParser.Helper;

namespace TouchOscLayoutParser.Page.Control
{
    public class TouchOscPushButtonControl : TouchOscControl
    {

        public bool SendPress
        {
            get;
            set;
        }
        public bool SendRelease
        {
            get;
            set;
        }
        public bool LocalFeedbackOff
        {
            get;
            set;
        }
        public bool YPositionVelocity
        {
            get;
            set;
        }
        public bool InvertYVelocity
        {
            get;
            set;
        }

        public TouchOscPushButtonControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            this.SendPress = ParserHelper.GetBoolAttribute(controlElement,"sp",true);
            this.SendRelease = ParserHelper.GetBoolAttribute(controlElement,"sr",true);
            this.LocalFeedbackOff= ParserHelper.GetBoolAttribute(controlElement,"local_off",false);
            this.YPositionVelocity = ParserHelper.GetBoolAttribute(controlElement,"velocity",false);
            this.InvertYVelocity = ParserHelper.GetBoolAttribute(controlElement,"velocity_invert",false);
        }

    }
}
