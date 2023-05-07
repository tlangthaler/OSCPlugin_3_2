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
    class TouchOscMultiToggleControl : TouchOscControl,IMultiControl
    {
        public bool LocalFeedbak
        {
            get;
            set;
        }

        public bool EclusiveMode
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

        public TouchOscMultiToggleControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse MultiPush Attributes
            this.LocalFeedbak = ParserHelper.GetBoolAttribute(controlElement,"local_off",false);
            this.EclusiveMode = ParserHelper.GetBoolAttribute(controlElement,"ex_mode",false);
            this.NoOfControlsX = ParserHelper.GetIntAttribute(controlElement,"number_x",5);
            this.NoOfControlsY = ParserHelper.GetIntAttribute(controlElement,"number_y",5);

        }

    }
}
