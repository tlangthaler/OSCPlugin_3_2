using Lumos.GUI;
using Lumos.GUI.Input.v2;
using LumosLIB.Tools;
using org.dmxc.lumos.Kernel.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using TouchOscLayoutParser;
using TouchOscLayoutParser.Page;
using TouchOscLayoutParser.Page.Control;
using WeifenLuo.WinFormsUI.Docking;
namespace OSCGUIPlugin
{
    public class OscRuleSet : IDisposable
	{
		public class RuleEventArgs : EventArgs
		{
			public OscDeviceRule Rule
			{
				get;
				set;
			}
        }
		private string name;
		public BindingList<OscDeviceRule> Rules;
		private readonly OscRuleSetEditForm editWindow;
		public event EventHandler NameChanged;
		public event EventHandler<OscRuleSet.RuleEventArgs> RuleAdded;
		public event EventHandler<OscRuleSet.RuleEventArgs> RuleDeleted;
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				if (this.NameChanged != null)
				{
					NameChanged(this, null);
				}
			}
		}
		public int RuleCount
		{
			get
			{
				return this.Rules.Count;
			}
		}
		public string GUID
		{
			get;
			private set;
		}
		public static OscRuleSet Load(ManagedTreeItem mti)
		{
			OscRuleSet ret = new OscRuleSet(false);
			Pair<string, object> nameAt = mti.Attributes.FirstOrDefault((Pair<string, object> i) => i.Left == "Name");
			ret.Name = ((nameAt != null) ? ((string)nameAt.Right) : "");
			if (mti.hasValue<string>("GUID"))
			{
				ret.GUID = mti.getValue<string>("GUID");
			}
			else
			{
				ret.GUID = Guid.NewGuid().ToString();
			}
			foreach (ManagedTreeItem item in mti.GetChildren("Rule"))
			{
                    if (OscDeviceRule.Load(ret, item) == null)
                    {
                        OscContextManager.Log.Warn("Failed to load DeviceRule in RuleSet {0}, Invalid Cast and/or constructor missing!", ret.Name);
                    }
			}
			//Do Update of all IO Channels after complete Ruleset loaded
			foreach(OscDeviceRule rule in ret.Rules)
            {
				rule.UpdateIOChannels();
            }
			return ret;
		}

		public void Save(ManagedTreeItem mti)
		{
            OscContextManager.Log.Debug("Saving RuleSet {0}", Name);
			mti.setValue<string>("Name", this.Name);
			mti.setValue<string>("GUID", this.GUID);
			foreach (OscDeviceRule rule in this.Rules)
			{
				ManagedTreeItem mtir = new ManagedTreeItem("Rule");
				rule.Save(mtir);
				mti.AddChild(mtir);
			}
		}
		protected void OnSendMessage(object s, OSCEventArgs m)
		{
			//Send to all registered output Devices
			foreach (OscOutput outputDevice in OscContextManager.DeviceInformation.OutputDevices)
            {
				outputDevice.OutputDevice.Send(m.m);
            }

		}
		public OscRuleSet() : this(true)
		{
		}
		private OscRuleSet(bool genGuid)
		{
			this.Rules = new BindingList<OscDeviceRule>();
			this.Name = "New RuleSet";
			this.editWindow = new OscRuleSetEditForm(this);
			if (genGuid)
			{
				this.GUID = Guid.NewGuid().ToString();
			}
			//Connect to Input 
			foreach (OscInput inputDevice in OscContextManager.DeviceInformation.InputDevices)
			{
				inputDevice.MessageReceived += new EventHandler<OSCEventArgs>(this.HandleMessageReceived);
			}

		}
		private void HandleMessageReceived(object s, OSCEventArgs e)
		{
			foreach (OscDeviceRule item in this.Rules)
			{
				item.Process(e.m);
			}
		}
		public OscDeviceRule CreateRule(string t)
		{
            var type = OscContextManager.AssemblyHelper.DeviceRuleTypes.FirstOrDefault(j => j.FullName == t);
            if (type == null) return null;
            var obj = Activator.CreateInstance(type) as OscDeviceRule;
			//OscContextManager.Log.Debug("Instance created", new object[0]);

			obj.RuleSet = this;
			//OscContextManager.Log.Debug("RuleSet added", new object[0]);


			return obj;
		}
		public void AddRule(OscDeviceRule r)
		{
			//OscContextManager.Log.Debug("Add Rule to Ruleset started", new object[0]);

			this.Rules.Add(r);
			r.OSCMessageSend += new EventHandler<OSCEventArgs>(this.OnSendMessage);
			this.OnRuleAdded(r);
			if (OscContextManager.OscForm != null)
			{
				OscContextManager.OscForm.UpdateUi();
			}

		}
		public void DeleteRule(OscDeviceRule r)
		{
			//OscContextManager.Log.Debug("Delete Rule started", new object[0]);

			this.Rules.Remove(r);
			r.OSCMessageSend -= new EventHandler<OSCEventArgs>(this.OnSendMessage);
			this.OnRuleDeleted(r);
			if (OscContextManager.OscForm != null)
			{
				OscContextManager.OscForm.UpdateUi();
			}

		}
		public void OpenEditWindow()
		{
			WindowManager.getInstance().ShowWindow(this.editWindow, DockState.Float);
		}
		public void Dispose()
		{
			this.editWindow.Dispose();
		}
		protected void OnRuleAdded(OscDeviceRule r)
		{
			//Update IO Channels
			r.UpdateIOChannels();
			if (this.RuleAdded != null)
			{
				RuleAdded(this, new OscRuleSet.RuleEventArgs
				{
					Rule = r
				});
			}
		}
		protected void OnRuleDeleted(OscDeviceRule r)
		{
			//RemoveAllIOChannels
			InputManager.getInstance().UnregisterSinks(r.OutputChannels);
			InputManager.getInstance().UnregisterSources(r.InputChannels);
			if (this.RuleDeleted != null)
			{
				RuleDeleted(this, new OscRuleSet.RuleEventArgs
				{
					Rule = r
				});
			}
		}

        public XElement Serialize()
        {
            var xElement = new XElement("RuleSet");
            xElement.Add(new XAttribute("Name", this.name));
            foreach (var rule in this.Rules)
            {
                xElement.Add(rule.Serialize());
            }
            return xElement;
            throw new NotImplementedException();
        }

        public static OscRuleSet LoadFromXml(XElement element)
        {
            var ruleset = new OscRuleSet(true)
            {
                name = element.Attribute("Name").Value
            };
            foreach (var item in element.Elements("Rule"))
            {
                ruleset.AddRule(OscDeviceRule.LoadFromXml(item,ruleset));
            }
            return ruleset;
        }

		public static OscRuleSet LoadFromTouchOsc(TouchOscLayout layout)
		{
			OscContextManager.Log.Debug("Import OSC Layout in RuleSet started");
			var ruleset = new OscRuleSet(true)
			{
				name = layout.Name
			};
			foreach(TouchOscPage page in layout.Pages)
            {
				OscContextManager.Log.Debug("Run through page: " + page.Name);
				foreach (TouchOscControl control in page.Controls)
                {
                    OscContextManager.Log.Debug("Run through control: " + control.Name);
					CreateRule(ruleset,control);
                }
            }
			return ruleset;
		}

        private static void CreateRule(OscRuleSet ruleset,TouchOscControl control)
        {
            if (control is IMultiControl control1)
            {
                if (control1.NoOfControlsX > 0)
                {
                    for (int x = 1; x <= control1.NoOfControlsX; x++)
                    {
                        OscDeviceRule rule;
                        if (control1.NoOfControlsY > 0)
                        {
                            for (int y = 1; y <= control1.NoOfControlsY; y++)
                            {
                                rule = OscDeviceRule.LoadFromTouchOsc(control, x, y, ruleset);
                                if (rule != null)
                                {
									if (rule is IOscMultiItem rule1)
                                    {
										rule1.IsMultiTouchItem = true;
										rule1.MultiItemName = control.Name;
                                    }
									ruleset.AddRule(rule);
                                }
                            }
                        }
                        else
                        {
                            rule = OscDeviceRule.LoadFromTouchOsc(control, x, 0, ruleset);
                            if (rule != null)
                            {
								if (rule is IOscMultiItem rule1)
								{
									rule1.IsMultiTouchItem = true;
									rule1.MultiItemName = control.Name;
								}
								ruleset.AddRule(rule);
                            }
                        }
                    }
                }
            }
            else
            {
                OscDeviceRule rule = OscDeviceRule.LoadFromTouchOsc(control, 0, 0, ruleset);
                if (rule != null)
                {
                    ruleset.AddRule(rule);
                }
            }
        }

        public bool UpdateFromTouchOsc(TouchOscLayout layout)
		{
			OscContextManager.Log.Debug("Update OSC Layout in RuleSet started");
			// First find missing rules
			foreach (TouchOscPage page in layout.Pages)
			{
				OscContextManager.Log.Debug("Run through page: " + page.Name);
				foreach (TouchOscControl control in page.Controls)
				{
					OscContextManager.Log.Debug("Run through control: " + control.Name);
					OscDeviceRule found = null;
					if (control is IMultiControl control1)
					{
						OscContextManager.Log.Debug("Control is Multicontrol");
						if (control1.NoOfControlsX > 0)
						{
							for (int x = 1; x <= control1.NoOfControlsX; x++)
							{
								if (control1.NoOfControlsY > 0)
								{
									for (int y = 1; y <= control1.NoOfControlsY; y++)
									{
										foreach (OscDeviceRule rule in this.Rules)
										{
											if (NameEquals(control, rule, x, y))
											{
												found = rule;
												OscContextManager.Log.Debug("Found rule: " + found.Name + " X-Position: " + x + " Y-Position: " + y);
											}
										}
										CheckFoundRule(found, control, x, y,true);
										found = null;
									}
								}
								else
								{
									// Multicontrol only x
									foreach (OscDeviceRule rule in this.Rules)
									{
										if (NameEquals(control, rule, x, 0))
										{
											found = rule;
											OscContextManager.Log.Debug("Found rule: " + found.Name + " X-Position: " + x + " Y-Position: " + 0);
										}
									}
									CheckFoundRule(found, control, x, 0,true);
									found = null;
								}
							}
						}
					}
					else
					{
						OscContextManager.Log.Debug("Control is No-Multicontrol");
						//No Multicontrol
						foreach (OscDeviceRule rule in this.Rules)
						{
							if (NameEquals(control,rule,0,0))
                            {
								found = rule;
                            }
						}
						CheckFoundRule(found, control, 0, 0,false);
					}
				}
			}
			//Find rules, which are no more available in Layout
			List<OscDeviceRule> toDeleteList = new List<OscDeviceRule>();
			foreach (OscDeviceRule rule in this.Rules)
			{
				bool found = false;
				foreach (TouchOscPage page in layout.Pages)
				{
					foreach (TouchOscControl control in page.Controls)
					{
						if (NameEquals(control,rule))
                        {
							found = true;
                        }
					}
				}
				if (!found)
                {
					//Control for this rule was not found --> Remove from set
					toDeleteList.Add(rule);
                }
			}
			foreach(OscDeviceRule rule in toDeleteList)
            {
				this.DeleteRule(rule);
            }

			//Finally update all IOChannels
			foreach(OscDeviceRule rule in Rules)
            {
				rule.UpdateIOChannels();
            }
			return true;
		}

		private bool NameEquals(TouchOscControl control, OscDeviceRule rule,int xPosition,int yPosition)
        {
			string nameExtension = "";
			if (xPosition > 0)
			{
				nameExtension = "_" + xPosition;
			}
			if (yPosition > 0)
			{
				nameExtension = nameExtension + "_" + yPosition;
			}
			if (rule.Name.Equals(control.Name + nameExtension))
            {
				return true;
            }
			return false;
        }
		private bool NameEquals(TouchOscControl control, OscDeviceRule rule)
		{
			if (rule.Name.StartsWith(control.Name))
            {
				//Basically the same --> Check if Nulticontrol and if rule Name within Range
				if (control is IMultiControl control1)
                {
					int index1 = rule.Name.IndexOf("_", control.Name.Length);
					int index2 = rule.Name.IndexOf("_", index1 + 1);
					if (index1 > 0)
					{
						if (control1.NoOfControlsY == 0)
						{
							//Only 1 extension number for x --> Get from rule Namestring
							if (index2 > 0)
                            {
								return false;
                            }
							else
                            {
                                return int.TryParse(rule.Name.Substring(index1 + 1), out int xPosition) && xPosition != 0 && xPosition <= control1.NoOfControlsX;
                            }
						}
						if (index2 > 0)
                        {
                            bool xvalid = int.TryParse(rule.Name.Substring(index1 + 1, index2 - index1-1), out int xPosition);
                            bool yvalid = int.TryParse(rule.Name.Substring(index2 + 1), out int yPosition);
							return xvalid & yvalid & xPosition <= control1.NoOfControlsX & yPosition <= control1.NoOfControlsY;
                        }
						else
                        {
							return false;
                        }
					}
                    else
                    {
						return false;
                    }
                }
				else
                {
					//No Multicontrol. Name must be equal
					return rule.Name.Equals(control.Name) ;
                }
            }
			return false;
		}

		private void CheckFoundRule(OscDeviceRule found, TouchOscControl control,int xPositionFound, int yPositionFound, bool isMultiTouch)
        {
			if (found != null)
			{
				// Ruleset found --> Check RuleType
				Type type = OscContextManager.AssemblyHelper.DeviceRuleTypes.FirstOrDefault(j => j.FullName == OscContextManager.AssemblyHelper.GetMappedTypeName(control.ControlType));
				if (type == null) return;
				if (type.FullName.Equals(found.GetType().FullName))
				{
					//correct type, do update
					if (!found.UpdateFromTouchOsc(control, xPositionFound, yPositionFound,isMultiTouch)) return;
				}
				else
				{
					//Wrong type --> Delete and recreate
					this.DeleteRule(found);
					OscDeviceRule rule = OscDeviceRule.LoadFromTouchOsc(control, xPositionFound, yPositionFound, this);
					if (rule != null)
					{
						if (rule is IOscMultiItem rule1 && isMultiTouch)
						{
							rule1.IsMultiTouchItem = true;
							rule1.MultiItemName = control.Name;
						}

						this.AddRule(rule);
					}
				}
			}
			else
			{
				//not found --> Create
				OscDeviceRule rule = OscDeviceRule.LoadFromTouchOsc(control, xPositionFound, yPositionFound, this);
				if (rule != null)
				{
					this.AddRule(rule);
				}
			}
		}
	}
}
