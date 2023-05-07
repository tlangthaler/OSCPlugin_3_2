using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TouchOscLayoutParser.Page.Control
{
    public class ControlDimension
    {
        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }

        public ControlDimension(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public static ControlDimension Parse(XElement controlElement)
        {
            return new ControlDimension(int.Parse(controlElement.Attribute("x").Value), int.Parse(controlElement.Attribute("y").Value), int.Parse(controlElement.Attribute("w").Value), int.Parse(controlElement.Attribute("h").Value));
        }

    }
}
