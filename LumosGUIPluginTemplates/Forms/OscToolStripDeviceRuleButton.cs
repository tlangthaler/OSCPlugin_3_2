using LumosControls.Controls.ToolStrip;
using System;
namespace OSCGUIPlugin
{
    internal class OscToolStripDeviceRuleButton : LumosToolStripMenuItem
	{
		internal Type Type;
		internal OscDeviceRule CreateDeviceRule()
		{
			OscDeviceRule result;
			if (this.Type == null)
			{
				result = null;
			}
			else
			{
				result = (OscDeviceRule)Activator.CreateInstance(this.Type);
			}
			return result;
		}
	}
}
