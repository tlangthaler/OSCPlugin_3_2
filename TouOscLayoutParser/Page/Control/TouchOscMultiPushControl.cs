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
    class TouchOscMultiPushControl : TouchOscControl,IMultiControl
    {
        public bool LocalFeedbak
        {
            get;
            set;
        }

        public int NoOfControlsX
        {
            get;
            set;
        }

        public int NoOfControlsY
        {
            get;
            set;
        }

        public TouchOscMultiPushControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse MultiPush Attributes
            this.LocalFeedbak = ParserHelper.GetBoolAttribute(controlElement,"local_off",false);
            this.NoOfControlsX = ParserHelper.GetIntAttribute(controlElement,"number_x",5);
            this.NoOfControlsY = ParserHelper.GetIntAttribute(controlElement,"number_y",5);
        }

    }
}
