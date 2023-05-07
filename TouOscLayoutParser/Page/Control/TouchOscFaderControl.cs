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
    public class TouchOscFaderControl : TouchOscControl
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
        public TouchOscFaderControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse Fader Data
            this.Inverted = ParserHelper.GetBoolAttribute(controlElement,"inverted",false);
            this.Centered = ParserHelper.GetBoolAttribute(controlElement,"centered",false);
            this.Response = ParserHelper.GetResponse(controlElement.Attribute("response").Value);
        }

    }
}
