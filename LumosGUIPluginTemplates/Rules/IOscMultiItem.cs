using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCGUIPlugin
{
    public interface IOscMultiItem
    {
        bool IsMultiTouchItem
        {
            get;
            set;
        }
        string MultiItemName
        {
            get;
            set;
        }
    }
}
