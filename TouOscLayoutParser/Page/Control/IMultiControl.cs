using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchOscLayoutParser.Page.Control
{
    public interface IMultiControl
    {
        int NoOfControlsX
        {
            get;
            set;
        }

        int NoOfControlsY
        {
            get;
            set;
        }

    }
}
