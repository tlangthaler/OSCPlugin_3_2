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
    public class TouchOscLabelControl : TouchOscControl
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

        public string Text
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public TouchOscLabelControl(string Name, ITouchOscLayoutElement Parent, EControlType ControlType, EOrientation Orientation) : base(Name, Parent, ControlType,Orientation)
        {
        }

        public override void Parse(XElement controlElement)
        {
            base.Parse(controlElement);
            //Parse Label Elements
            this.Outline = ParserHelper.GetBoolAttribute(controlElement,"outline",true);
            this.Background = ParserHelper.GetBoolAttribute(controlElement,"background",true);
            this.Text = ParserHelper.DecodeBase64(controlElement.Attribute("text").Value);
            this.Size = ParserHelper.GetIntAttribute(controlElement,"size",14);
        }

    }
}
