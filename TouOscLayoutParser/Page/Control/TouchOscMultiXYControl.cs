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
    public class TouchOscMultiXYControl : TouchOscControl, IMultiControl
    {
        public bool Outline
        {
            get;
            set;
        }

        public bool Background
        {
            get;
            set;
        }
        public bool InvertX
        {
            get;
            set;
        }
        public bool InvertY
        {
            get;
            set;
        }
        public bool SendReversed
        {
            get;
            set;
        }
        public int NoOfControlsX { get { return 1; } set { } }
        public int NoOfControlsY
        {
            get { return 0; }
            set { }
        }

        public TouchOscMultiXYControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse MultiXY Attributes
            this.Outline = ParserHelper.GetBoolAttribute(controlElement,"outline",true);
            this.Background = ParserHelper.GetBoolAttribute(controlElement,"background",true);
            this.InvertX = ParserHelper.GetBoolAttribute(controlElement,"inverted_x",false);
            this.InvertY = ParserHelper.GetBoolAttribute(controlElement,"inverted_y",false);
            this.SendReversed = ParserHelper.GetBoolAttribute(controlElement,"rev_xy",false);
        }

    }
}
