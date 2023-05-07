using org.dmxc.lumos.Kernel.Resource;
using System;
namespace OSCGUIPlugin
{
	public interface IOscSave
	{
		void Init(ManagedTreeItem i);
		void Save(ManagedTreeItem i);
	}
}
