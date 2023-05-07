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
    public class TouchOscToggleButtonControl : TouchOscControl
    {
        public bool LocalFeedbackOff
        {
            get;
            set;
        }
        public TouchOscToggleButtonControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            this.LocalFeedbackOff = ParserHelper.GetBoolAttribute(controlElement,"local_off",false);
        }

    }
}
