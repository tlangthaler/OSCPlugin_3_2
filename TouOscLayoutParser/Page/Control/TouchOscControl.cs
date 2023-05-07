using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouchOscLayoutParser.Communication;
using TouchOscLayoutParser.Enumeration;
using TouchOscLayoutParser.Helper;

namespace TouchOscLayoutParser.Page.Control
{
    public class TouchOscControl : ITouchOscLayoutElement,IParsable
    {
        public override List<ITouchOscLayoutElement> Children { get { return null; } }

        public TouchOscControl(string Name,ITouchOscLayoutElement Parent,EControlType ControlType,EOrientation Orientation) : base(Name,Parent)
        {
            this.ControlType = ControlType;
            this.Orientation = Orientation;
        }

        public OscBaseTag OscTag
        {
            get;
            set;
        }

        public EControlType ControlType 
        {
            get;
            protected set;
        }

        public EColor Color
        {
            get;
            set;
        }

        public ControlDimension Dimension
        {
            get;
            set;
        }

        public EOrientation Orientation
        { 
            get;
            set;
        }


        public static TouchOscControl Parse(XElement controlElement, ITouchOscLayoutElement Parent)
        {
            //Get ControlType to create the correct
            EControlType type = ParserHelper.GetControlType(controlElement.Attribute("type").Value);
            string name = ParserHelper.DecodeBase64(controlElement.Attribute("name").Value);
            EOrientation orientation = ParserHelper.GetControlOrientation(controlElement.Attribute("type").Value);
            //Create Control Instance
            TouchOscControl control;
            switch (type)
            {
                case EControlType.Battery:
                    {
                        control = new TouchOscBatteryControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Encoder:
                    {
                        control = new TouchOscEncoderControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Fader:
                    {
                        control = new TouchOscFaderControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Label:
                    {
                        control = new TouchOscLabelControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.LED:
                    {
                        control = new TouchOscLEDControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Multi_Fader:
                    {
                        control = new TouchOscMultiFaderControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Multi_Push:
                    {
                        control = new TouchOscMultiPushControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Multi_Toggle:
                    {
                        control = new TouchOscMultiToggleControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Multi_XY:
                    {
                        control = new TouchOscMultiXYControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Push_Button:
                    {
                        control = new TouchOscPushButtonControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Rotary:
                    {
                        control = new TouchOscRotaryControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Time:
                    {
                        control = new TouchOscTimeControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.Toggle_Button:
                    {
                        control = new TouchOscToggleButtonControl(name, Parent, type, orientation);
                        break;
                    }
                case EControlType.XY_Pad:
                    {
                        control = new TouchOscXYControl(name, Parent, type, orientation);
                        break;
                    }
                default:
                    {
                        control = new TouchOscLabelControl(name, Parent, type, orientation);
                        break;
                    }
            }
            //Parse everything else
            control.Parse(controlElement);
            return control;
        }

        public virtual void Parse(XElement controlElement)
        {
            //Parse OscTag
            this.OscTag = OscBaseTag.Parse(controlElement, this);
            this.Color = ParserHelper.GetColor(controlElement.Attribute("color").Value);
            this.Dimension = ControlDimension.Parse(controlElement);
        }
    }
}
