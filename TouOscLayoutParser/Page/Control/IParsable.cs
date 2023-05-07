using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TouchOscLayoutParser.Page.Control
{
    public interface IParsable
    {
        void Parse(XElement controlElement);

    }
}
