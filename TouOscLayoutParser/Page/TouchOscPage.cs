using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouchOscLayoutParser.Communication;
using TouchOscLayoutParser.Enumeration;
using TouchOscLayoutParser.Helper;
using TouchOscLayoutParser.Page.Control;

namespace TouchOscLayoutParser.Page
{
    public class TouchOscPage:ITouchOscLayoutElement
    {
        private readonly List<TouchOscControl> controls = new List<TouchOscControl>();
        public EColor InactiveColor
        {
            get;
            set;
        }
        
        public EColor ActiveColor
        {
            get;
            set;
        }

        public string InactiveText
        {
            get;
            set;
        }

        public string ActiveText
        {
            get;
            set;
        }

        public int InactiveSize
        {
            get;
            set;
        }

        public int ActiveSize
        {
            get;
            set;
        }

        public bool InactiveOutline
        {
            get;
            set;
        }
        public bool ActiveOutline
        {
            get;
            set;
        }
        public bool InactiveBackground
        {
            get;
            set;
        }
        public bool ActiveBackground
        {
            get;
            set;
        }
        public OscBaseTag OscTag
        {
            get;
            set;
        }

        public List<TouchOscControl> Controls => controls;

        public override List<ITouchOscLayoutElement> Children
        {
            get
            {
                return (List<ITouchOscLayoutElement>)controls.Cast<ITouchOscLayoutElement>().ToList();
            }
        }

        public TouchOscPage(string Name,ITouchOscLayoutElement Parent):base(Name,Parent)
        {
        }


        public static TouchOscPage Parse(XElement pageElement,ITouchOscLayoutElement Parent)
        {
            //Get Page Name and create it
            TouchOscPage page = new TouchOscPage(ParserHelper.DecodeBase64(pageElement.Attribute("name").Value), Parent)
            {
                //ActiveBackground
                ActiveBackground = ParserHelper.GetBoolAttribute(pageElement,"la_b",false),
                //InActiveBackground
                InactiveBackground = ParserHelper.GetBoolAttribute(pageElement,"li_b",false),
                //ActiveOutline
                ActiveOutline = ParserHelper.GetBoolAttribute(pageElement,"la_o",false),
                //InactiveOutline
                InactiveOutline = ParserHelper.GetBoolAttribute(pageElement,"li_o",false),
                //ActiveSize
                ActiveSize = ParserHelper.GetIntAttribute(pageElement,"la_s",14),
                //InactiveSize
                InactiveSize = ParserHelper.GetIntAttribute(pageElement,"li_s",14),
                //ActiveColor
                ActiveColor = ParserHelper.GetColor(ParserHelper.GetStringAttribute(pageElement,"la_c","gray")),
                //InactiveColor
                InactiveColor = ParserHelper.GetColor(ParserHelper.GetStringAttribute(pageElement,"li_c","gray")),
                //ActiveText
                ActiveText = ParserHelper.DecodeBase64(ParserHelper.GetStringAttribute(pageElement,"la_t","")),
                            //InactiveText
                InactiveText = ParserHelper.DecodeBase64(ParserHelper.GetStringAttribute(pageElement,"li_t",""))

            };
            //OscTag
            page.OscTag = OscBaseTag.Parse(pageElement, page);
            //Add Controls
            if (pageElement.HasElements)
            {
                foreach (XElement control in pageElement.Elements())
                {
                    page.Controls.Add(TouchOscControl.Parse(control, page));
                }
            }
            return page;
        }
    }
}
