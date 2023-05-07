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
    public class TouchOscXYControl : TouchOscControl
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
        public TouchOscXYControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }


        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            this.Outline = ParserHelper.GetBoolAttribute(controlElement,"outline",true);
            this.Background = ParserHelper.GetBoolAttribute(controlElement,"background",true);
            this.InvertX = ParserHelper.GetBoolAttribute(controlElement,"inverted_x",false);
            this.InvertY = ParserHelper.GetBoolAttribute(controlElement,"inverted_y",false);
            this.SendReversed = ParserHelper.GetBoolAttribute(controlElement,"rev_xy",false);
        }

    }
}
