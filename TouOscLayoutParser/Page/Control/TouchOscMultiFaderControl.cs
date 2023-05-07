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
    public class TouchOscMultiFaderControl : TouchOscControl,IMultiControl
    {
        public bool Inverted
        {
            get;
            set;
        }

        public bool Centered
        {
            get;
            set;
        }

        public int Faders
        {
            get;
            set;
        }
        public int NoOfControlsX
        {
            get {
                return Faders;
                }
            set { Faders = value; }
        }
        public int NoOfControlsY { get { return 0; } set { } }

        public TouchOscMultiFaderControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse MultiFaderAttributes
            this.Inverted = ParserHelper.GetBoolAttribute(controlElement,"inverted",false);
            this.Centered = ParserHelper.GetBoolAttribute(controlElement,"centered",false);
            this.Faders = ParserHelper.GetIntAttribute(controlElement,"number",5);
        }

    }
}
