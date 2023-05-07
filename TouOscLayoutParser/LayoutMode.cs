using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchOscLayoutParser.Enumeration;

namespace TouchOscLayoutParser
{
    public class LayoutMode
    {
        public ELayoutMode Mode
        {
            get;
            set;
        }
        public int H
        {
            get;
            set;
        }
        public int W
        {
            get;
            set;
        }

        public LayoutMode(ELayoutMode Mode,int H, int W):this(Mode)
        {
            if (this.Mode == ELayoutMode.custom)
            {
                //Overwrite Coordinates
                this.H = H;
                this.W = W;
            }

        }

        public LayoutMode(int modeNumber,int H, int W):this(modeNumber)
        {
            if (this.Mode == ELayoutMode.custom)
            {
                //Overwrite Coordinates
                this.H = H;
                this.W = W;
            }
        }

        public LayoutMode(int modeNumber)
        {
            switch(modeNumber)
            {
                case 0:
                    {
                        this.Mode = ELayoutMode.iPhone_iPodTouch;
                        break;
                    }
                case 1:
                    {
                        this.Mode = ELayoutMode.iPad;
                        break;
                    }
                case 2:
                    {
                        this.Mode = ELayoutMode.iPhone_5;
                        break;
                    }
                default:
                    {
                        this.Mode = ELayoutMode.custom;
                        break;
                    }
            }
            ExtractCoordinates(this.Mode);
        }
        public LayoutMode(ELayoutMode Mode)
        {
            this.Mode = Mode;
            ExtractCoordinates(Mode);
        }

        private void ExtractCoordinates(ELayoutMode Mode)
        {
            switch (Mode)
            {
                case ELayoutMode.iPad:
                    {
                        this.H = 1024;
                        this.W = 768;
                        break;
                    }
                case ELayoutMode.iPad_Pro:
                    {
                        this.H = 1366;
                        this.W = 1024;
                        break;
                    }
                case ELayoutMode.iPhone_5:
                    {
                        this.H = 568;
                        this.W = 320;
                        break;
                    }
                case ELayoutMode.iPhone_6_6s:
                    {
                        this.H = 667;
                        this.W = 375;
                        break;
                    }
                case ELayoutMode.iPhone_6_6sPlus:
                    {
                        this.H = 736;
                        this.W = 414;
                        break;
                    }
                case ELayoutMode.iPhone_iPodTouch:
                    {
                        this.H = 480;
                        this.W = 320;
                        break;
                    }
                case ELayoutMode.custom:
                    {
                        this.H = 100;
                        this.W = 100;
                        break;
                    }
            }
        }
    }
}
