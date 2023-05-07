using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchOscLayoutParser
{
    public abstract class ITouchOscLayoutElement
    {
        public string Name
        {
            get;
            protected set;
        }
        public ITouchOscLayoutElement Parent
        {
            get;
            protected set;
        }
        public abstract List<ITouchOscLayoutElement> Children
        {
            get;
        }
        
        public ITouchOscLayoutElement(string Name,ITouchOscLayoutElement Parent)
        {
            this.Name = Name;
            this.Parent = Parent;
        }
    }
}
