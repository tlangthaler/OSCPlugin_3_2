using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouchOscLayoutParser.Enumeration;

namespace TouchOscLayoutParser.Helper
{
    public static class ParserHelper
    {
        public static string DecodeBase64(string base64String)
        {
            //Decode
            byte[] data = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(data);

        }

        public static string EncodeBase64(string value)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(data);
        }

        public static EColor GetColor(string colorName)
        {
            switch(colorName.ToLower())
            {
                case "gray":
                    return EColor.Gray;
                case "blue":
                    return EColor.Blue;
                case "brown":
                    return EColor.Brown;
                case "green":
                    return EColor.Green;
                case "orange":
                    return EColor.Orange;
                case "pink":
                    return EColor.Pink;
                case "purple":
                    return EColor.Purple;
                case "red":
                    return EColor.Red;
                case "yellow":
                    return EColor.Yellow;
                default:
                    return EColor.Gray;
            }

        }

        public static EResponse GetResponse(string responseName)
        {
            switch (responseName.ToLower())
            {
                case "absolute":
                    return EResponse.Absolute;
                default:
                    return EResponse.Relative;
            }

        }


        public static EControlType GetControlType(string controlType)
        {
            switch(controlType.ToLower())
            {
                case "faderh":
                case "faderv":
                    return EControlType.Fader;
                case "labelh":
                case "labelv":
                    return EControlType.Label;
                case "batteryh":
                case "batteryv":
                    return EControlType.Battery;
                case "push":
                    return EControlType.Push_Button;
                case "toggle":
                    return EControlType.Toggle_Button;
                case "led":
                    return EControlType.LED;
                case "xy":
                    return EControlType.XY_Pad;
                case "rotaryv":
                case "rotaryh":
                    return EControlType.Rotary;
                case "encoder":
                    return EControlType.Encoder;
                case "timeh":
                case "timev":
                    return EControlType.Time;
                case "multipush":
                    return EControlType.Multi_Push;
                case "multitoggle":
                    return EControlType.Multi_Toggle;
                case "multixy":
                    return EControlType.Multi_XY;
                case "multifaderv":
                case "multifaderh":
                    return EControlType.Multi_Fader;
                default: return EControlType.Label;
            }
        }
        public static EOrientation GetControlOrientation(string controlType)
        {
            switch (controlType.ToLower())
            {
                case "faderh":
                case "labelh":
                case "batteryh":
                case "rotaryh":
                case "multifaderh":
                    return EOrientation.horizontal;
                case "timeh":
                case "faderv":
                case "labelv":
                case "batteryv":
                case "rotaryv":
                case "timev":
                case "multifaderv":
                    return EOrientation.vertical;
                default: return EOrientation.vertical;
            }
        }

        public static bool GetBoolAttribute(XElement element,string name,bool defaultValue)
        {
            XAttribute attr = element.Attribute(name);
            if (attr != null)
            {
                return bool.Parse(attr.Value);
            }
            else
            {
                return defaultValue;
            }
        }
        public static int GetIntAttribute(XElement element, string name, int defaultValue)
        {
            XAttribute attr = element.Attribute(name);
            if (attr != null)
            {
                return int.Parse(attr.Value);
            }
            else
            {
                return defaultValue;
            }
        }
        public static string GetStringAttribute(XElement element, string name, string defaultValue)
        {
            XAttribute attr = element.Attribute(name);
            if (attr != null)
            {
                return attr.Value;
            }
            else
            {
                return defaultValue;
            }
        }

    }
}
