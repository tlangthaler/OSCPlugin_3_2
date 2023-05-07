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

    public class TouchOscRotaryControl : TouchOscControl
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

        public EResponse Response
        {
            get;
            set;
        }

        public bool NoRollover
        {
            get;
            set;
        }

        public TouchOscRotaryControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse Rotary Data
            this.Inverted = ParserHelper.GetBoolAttribute(controlElement,"inverted",false);
            this.Centered = ParserHelper.GetBoolAttribute(controlElement,"centered",false);
            this.NoRollover = ParserHelper.GetBoolAttribute(controlElement,"norollover",true);
            this.Response = ParserHelper.GetResponse(controlElement.Attribute("response").Value);
        }

    }
}
