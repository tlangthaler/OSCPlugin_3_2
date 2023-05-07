using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouchOscLayoutParser.Enumeration;
using TouchOscLayoutParser.Page;

namespace TouchOscLayoutParser
{
    public class TouchOscLayout:ITouchOscLayoutElement
    {
        private const string TempSubDir = "TouchOscParser";
        private readonly List<TouchOscPage> pages = new List<TouchOscPage>();
        public EOrientation Orientation
        {
            get;
            set;
        }

        public LayoutMode Mode
        {
            get;
            set;
        }
        
        public int Version
        {
            get;
            set;
        }

        public TouchOscLayout(string Name):base(Name,null)
        {
        }

        public List<TouchOscPage> Pages => pages;

        public override List<ITouchOscLayoutElement> Children
        {
            get
            {
                return (List<ITouchOscLayoutElement>)pages.Cast<ITouchOscLayoutElement>().ToList();
            }
        }

        public static TouchOscLayout Parse(string FileName)
        {
            //Extract Name from FileName
            TouchOscLayout layout = new TouchOscLayout(FileName.Substring(FileName.LastIndexOf("\\")+1, FileName.LastIndexOf(".")- FileName.LastIndexOf("\\")-1));
            //Extract and load
            //Check Temp Target Directory
            string targetDir = FindTempTargetDirectory();
            ZipFile.ExtractToDirectory(FileName, targetDir);

            //LoadXML
            XElement touchOscLayout = XElement.Load(targetDir + "\\index.xml");
            // Extract Layout Attributes
            //Orientation
            layout.Orientation = ExtractOrientation(touchOscLayout.Attribute("orientation").Value);
            //mode
            int mode = int.Parse(touchOscLayout.Attribute("mode").Value);
            XAttribute hAttribute = touchOscLayout.Attribute("h");
            XAttribute wAttribute = touchOscLayout.Attribute("w");
            if (mode == 3 && hAttribute != null && wAttribute != null)
            {
                layout.Mode = new LayoutMode(mode, int.Parse(hAttribute.Value), int.Parse(wAttribute.Value));
            }
            else
            {
                layout.Mode = new LayoutMode(mode);
            }
            //version
            layout.Version = int.Parse(touchOscLayout.Attribute("version").Value);

            //Parse Pages if available
            if (touchOscLayout.HasElements)
            {
                foreach (XElement element in touchOscLayout.Elements())
                {
                    if (element.Name.LocalName.Equals("tabpage"))
                    {
                        layout.Pages.Add(TouchOscPage.Parse(element,layout));
                    }
                }
            }
            return layout;
        }


        private static EOrientation ExtractOrientation(string orientation)
        {
            if (orientation.Equals("vertical"))
            {
                return EOrientation.vertical;
            }
            else
            {
                return EOrientation.horizontal;
            }
        }
        private static string FindTempTargetDirectory()
        {
            int index = 0;
            string target = Path.GetTempPath() + TempSubDir + index;
            while (Directory.Exists(target))
            {
                index++;
                target = Path.GetTempPath() + TempSubDir + index;
            }
            return target;
        }


    }
}
