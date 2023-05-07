using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSCGUIPlugin
{
    public class FriendlyNameAttribute : Attribute
    {
        public string Name { get; set; }
        public FriendlyNameAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
