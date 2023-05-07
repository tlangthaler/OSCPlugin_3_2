using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouchOscLayoutParser.Helper;

namespace TouchOscLayoutParser.Communication
{
    public class OscBaseTag
    {
        public string TagName
        {
            get;
            set;
        }
        public double RangeFrom
        {
            get;
            set;
        }
        public double RangeTo
        {
            get;
            set;
        }
        public bool AutoTagAssignUsed
        {
            get;
            set;
        }

        public OscBaseTag(string TagName,double RangeFrom,double RangeTo,bool AutoTagAssignUsed)
        {
            this.TagName = TagName;
            this.RangeFrom = RangeFrom;
            this.RangeTo = RangeTo;
            this.AutoTagAssignUsed = AutoTagAssignUsed;
        }

        public OscBaseTag(ITouchOscLayoutElement element):this(AutogenerateTagName(element),0.0,1.0,true)
        {
        }
        public OscBaseTag(ITouchOscLayoutElement element, double RangeFrom, double RangeTo):this(AutogenerateTagName(element),RangeFrom,RangeTo,true)
        {
        }

        private static string AutogenerateTagName(ITouchOscLayoutElement element)
        {
            if (element.Parent is null)
            {
                return "";
            }
            else
            {
                return AutogenerateTagName(element.Parent) + "/" + element.Name;
            }
        }


        public static OscBaseTag Parse(XElement pageElement, ITouchOscLayoutElement element)
        {
            //check available Tag Properties
            if (pageElement.Attribute("osc_cs") != null && pageElement.Attribute("scalef") != null && pageElement.Attribute("scalet") != null)
            {
                return new OscBaseTag(ParserHelper.DecodeBase64(pageElement.Attribute("osc_cs").Value), double.Parse(pageElement.Attribute("scalef").Value, CultureInfo.InvariantCulture.NumberFormat), double.Parse(pageElement.Attribute("scalet").Value, CultureInfo.InvariantCulture.NumberFormat), false);
            }
            else if (pageElement.Attribute("osc_cs") != null)
            {
                return new OscBaseTag(ParserHelper.DecodeBase64(pageElement.Attribute("osc_cs").Value), 0, 0,false);
            }
            //Tagname not available --> Autocreate Tag
            OscBaseTag tag = new OscBaseTag(element);
            //Check Scale
            if (pageElement.Attribute("scalef") != null)
            {
                tag.RangeFrom = double.Parse(pageElement.Attribute("scalef").Value, CultureInfo.InvariantCulture.NumberFormat);
            }
            if (pageElement.Attribute("scalet") != null)
            {
                tag.RangeFrom = double.Parse(pageElement.Attribute("scalet").Value, CultureInfo.InvariantCulture.NumberFormat);
            }
            return tag;
        }
    }
}
