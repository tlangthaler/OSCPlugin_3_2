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
    public class TouchOscBatteryControl : TouchOscControl
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

        public int Size
        {
            get;
            set;
        }

        public TouchOscBatteryControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse BatteryTags
            this.Outline = ParserHelper.GetBoolAttribute(controlElement,"outline",true);
            this.Background = ParserHelper.GetBoolAttribute(controlElement,"background",true);
            this.Size = ParserHelper.GetIntAttribute(controlElement,"size",14);
        }
    }
}
