using LumosLIB.Kernel;
using LumosLIB.Kernel.Scene.Fanning;
using org.dmxc.lumos.Kernel.Input.v2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCGUIPlugin
{
    class OscXYOutputChannel:OscOutputChannel
    {
        readonly OscEChannelType channelType;
        public OscXYOutputChannel(string DisplayName, OscDeviceRule rule, ParameterCategory category, EWellKnownInputType type, OscEChannelType channelType) : base(DisplayName + "_" + channelType, rule, category, type)
        {
            this.channelType = channelType;
        }

        public override bool UpdateValue(object newValue)
        {
            if (rule is OscXYRule rule1)
            {
                switch (channelType)
                {
                    case OscEChannelType.X:
                        {
                            return rule1.UpdateValueX(newValue);
                        }
                    case OscEChannelType.Y:
                        {
                            return rule1.UpdateValueY(newValue);
                        }
                }
            }
            if (this.channelType == OscEChannelType.Color)
            {
                //Conversion to OscEColor at this point
                return rule.UpdateValueColor(GetColor(newValue));
            }
            return true;
        }

        private OscEColor GetColor(object value)
        {
            //allowed is string
            if (value is string stringvalue)
            {
                //Check possible RGB Formats ("0;0;0 or 0,0,0")
                if (stringvalue.IndexOf(";") > 0)
                {
                    string[] rgb = stringvalue.Split(';');
                    if (rgb.Length == 3)
                    {
                        //Convert 
                        if (int.TryParse(rgb[0],out int rvalue) && int.TryParse(rgb[1], out int gvalue) && int.TryParse(rgb[2], out int bvalue))
                        {
                            Color foundcol = Color.FromArgb(rvalue, gvalue, bvalue);
                            var colorLookup = Enum.GetValues(typeof(KnownColor))
                       .Cast<KnownColor>()
                       .Select(Color.FromKnownColor)
                       .ToLookup(c => c.ToArgb());

                            // There are some colours with multiple entries...
                            foreach (var namedColor in colorLookup[foundcol.ToArgb()])
                            {
                                //Multiple entries possible --> Take first where not "unknown"
                                OscContextManager.Log.Debug("Found Named Color: " + namedColor.Name);
                                OscEColor col1 = GetColor(namedColor.Name);
                                if (col1 != OscEColor.Unknown)
                                {
                                    return col1;
                                }
                            }
                        }
                    }
                }
                if (stringvalue.IndexOf(",") > 0)
                {
                    string[] rgb = stringvalue.Split(',');
                    if (rgb.Length == 3)
                    {
                        //Convert 
                        if (int.TryParse(rgb[0], out int rvalue) && int.TryParse(rgb[1], out int gvalue) && int.TryParse(rgb[2], out int bvalue))
                        {
                            Color foundcol = Color.FromArgb(rvalue, gvalue, bvalue);
                            var colorLookup = Enum.GetValues(typeof(KnownColor))
                       .Cast<KnownColor>()
                       .Select(Color.FromKnownColor)
                       .ToLookup(c => c.ToArgb());

                            // There are some colours with multiple entries...
                            foreach (var namedColor in colorLookup[foundcol.ToArgb()])
                            {
                                //Multiple entries possible --> Take first where not "unknown"
                                OscContextManager.Log.Debug("Found Named Color: " + namedColor.Name);
                                OscEColor col1 = GetColor(namedColor.Name);
                                if (col1 != OscEColor.Unknown)
                                {
                                    return col1;
                                }
                            }
                        }
                    }
                }
                //is named color
                OscEColor col = GetColor(stringvalue);
                return col == OscEColor.Unknown ? OscEColor.Gray : col;
            }
            if (value is Color colorvalue)
            {
                var colorLookup = Enum.GetValues(typeof(KnownColor))
                       .Cast<KnownColor>()
                       .Select(Color.FromKnownColor)
                       .ToLookup(c => c.ToArgb());

                // There are some colours with multiple entries...
                foreach (var namedColor in colorLookup[colorvalue.ToArgb()])
                {
                    //Multiple entries possible --> Take first where not "unknown"
                    OscContextManager.Log.Debug("Found Named Color: " + namedColor.Name);
                    OscEColor col = GetColor(namedColor.Name);
                    if (col != OscEColor.Unknown)
                    {
                        return col;
                    }
                }
            }
            if (value is Byte || value is Double)
            {
                switch ((double)value)
                {
                    case 1: return OscEColor.Red;
                    case 2: return OscEColor.Green;
                    case 3: return OscEColor.Blue;
                    case 4: return OscEColor.Yellow;
                    case 5: return OscEColor.Purple;
                    case 6: return OscEColor.Gray;
                    case 7: return OscEColor.Orange;
                    case 8: return OscEColor.Brown;
                    case 9: return OscEColor.Pink;
                    default: return OscEColor.Gray;
                }
            }
            return OscEColor.Gray;
        }
        private OscEColor GetColor(string colorname)
        {
            switch (colorname.ToLower())
            {
                case "red": return OscEColor.Red;
                case "green": return OscEColor.Green;
                case "blue": return OscEColor.Blue;
                case "yellow": return OscEColor.Yellow;
                case "purple": return OscEColor.Purple;
                case "gray": return OscEColor.Gray;
                case "orange": return OscEColor.Orange;
                case "brown": return OscEColor.Brown;
                case "pink": return OscEColor.Pink;
                default: return OscEColor.Unknown;
            }
        }

    }
}
