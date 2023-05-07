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
    public class TouchOscTimeControl : TouchOscControl
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
        public bool ShowSeconds
        {
            get;
            set;
        }
        public int Size
        {
            get;
            set;
        }
        public TouchOscTimeControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            this.Outline = ParserHelper.GetBoolAttribute(controlElement,"outline",true);
            this.Background = ParserHelper.GetBoolAttribute(controlElement,"background",true);
            this.ShowSeconds = ParserHelper.GetBoolAttribute(controlElement,"seconds",false);
            this.Size = int.Parse(controlElement.Attribute("size").Value);
        }

    }
}
