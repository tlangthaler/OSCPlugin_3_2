using Lumos.GUI.AssemblyScan;
using org.dmxc.lumos.Kernel.AssemblyScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TouchOscLayoutParser.Enumeration;

namespace OSCGUIPlugin
{
	public class OscAssemblyHelper:IAssemblyListener
	{
		public List<Type> DeviceRuleTypes = new List<Type>();

        public Type[] ListeningTypes => DeviceRuleTypes.ToArray();

        public OscAssemblyHelper()
        {
            AssemblyManager.getInstance().registerAssemblyListener(this);
			OscContextManager.AssemblyHelper = this;
            //foreach (var t in GetType().Assembly.GetTypes())
              //  scanNewType(t,EAssemblyType.PLUGIN);
		}

        public string GetFriendlyName(Type t)
        {
            var attr = t.GetCustomAttributes(true);
            if(attr.Length > 0 && attr.Any(j => j is FriendlyNameAttribute))
            {
                return (attr.First(j => j is FriendlyNameAttribute) as FriendlyNameAttribute).Name;
            }
            return t.Name;
        }

        public void scanNewType(Type t, EAssemblyType type)
        {
            OscContextManager.Log.Debug("Scanning type {0}", t.FullName);
            if (t.IsClass && !t.IsAbstract && typeof(OscDeviceRule).IsAssignableFrom(t))
            {
                this.DeviceRuleTypes.Add(t);
            }
        }

        public void typeRemoved(Type t, EAssemblyType type)
        {
            DeviceRuleTypes.RemoveAll(j => j == t);
        }

        public string GetMappedTypeName(EControlType type)
        {
            switch (type)
            {
                case EControlType.Multi_Push:
                case EControlType.Multi_Toggle:
                case EControlType.Push_Button:
                case EControlType.Toggle_Button:
                    return "OSCGUIPlugin.OscButtonRule";
                case EControlType.Encoder:
                    return "OSCGUIPlugin.OscEncoderRule";
                case EControlType.LED:
                    return "OSCGUIPlugin.OscLedRule";
                case EControlType.Fader:
                case EControlType.Rotary:
                case EControlType.Multi_Fader:
                    return "OSCGUIPlugin.OscSliderRule";
                case EControlType.Label:
                    return "OSCGUIPlugin.OscTextRule";
                case EControlType.XY_Pad:
                case EControlType.Multi_XY:
                    return "OSCGUIPlugin.OscXYRule";
                default:
                    return "-";
            }
        }
    }


}
