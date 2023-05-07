using Lumos.GUI.Input;
using Lumos.GUI.Input.v2;
using System;
using System.ComponentModel;
using System.Diagnostics;
namespace OSCGUIPlugin
{
	public class OSCInformation : IDisposable
	{
		public class RuleSetCollection : BindingList<OscRuleSet>
		{
			public event EventHandler<OSCInformation.RuleSetEventArgs> Added;
			public event EventHandler<OSCInformation.RuleSetEventArgs> Deleted;
			protected virtual void OnAdded(OscRuleSet rs)
			{
				if (this.Added != null)
				{
					Added(this, new OSCInformation.RuleSetEventArgs
					{
						Rs = rs
					});
				}
			}
			protected virtual void OnDeleted(OscRuleSet rs)
			{
				if (this.Deleted != null)
				{
                    Deleted(this, new OSCInformation.RuleSetEventArgs
					{
						Rs = rs
					});
				}
			}
			public new void Add(OscRuleSet r)
			{
				base.Add(r);
				this.OnAdded(r);
			}
			public new void Remove(OscRuleSet r)
			{
				base.Remove(r);
				this.OnDeleted(r);
			}
			public new void Clear()
			{
				foreach (OscRuleSet item in this)
				{
					this.OnDeleted(item);
				}
				base.Clear();
			}
		}
		public class RuleSetEventArgs : EventArgs
		{
			public OscRuleSet Rs
			{
				get;
				set;
			}
		}
		private bool disposed = false;
		public event EventHandler<OSCInformation.RuleSetEventArgs> RuleSetDeleted;
		public OSCInformation.RuleSetCollection RuleSets
		{
			get;
			set;
			
		}
		public OSCInformation()
		{
			OscContextManager.OSCInformation = this;
			this.RuleSets = new OSCInformation.RuleSetCollection();
			this.RuleSets.Added += new EventHandler<OSCInformation.RuleSetEventArgs>(this.OnRuleSetAdded);
			this.RuleSets.Deleted += new EventHandler<OSCInformation.RuleSetEventArgs>(this.OnRuleSetDeleted);
			RuleSets.ListChanged += new ListChangedEventHandler(OnRuleSetChanged);
		}

		private void OnRuleSetChanged(object o, ListChangedEventArgs rs)
        {
			if (OscContextManager.OscForm != null)
            {
				OscContextManager.OscForm.UpdateUi();
            }
        }
		private void OnRuleSetAdded(object o, OSCInformation.RuleSetEventArgs rs)
		{
			if (OscContextManager.OscForm != null)
			{
				OscContextManager.OscForm.UpdateUi();
			}

		}
		private void OnRuleSetDeleted(object o, OSCInformation.RuleSetEventArgs rs)
		{
			//RemoveAllIOChannels
			foreach (OscDeviceRule rule in (rs.Rs.Rules))
			{
				InputManager.getInstance().UnregisterSinks(rule.OutputChannels);
				InputManager.getInstance().UnregisterSources(rule.InputChannels);
			}

			if (OscContextManager.OscForm != null)
			{
				OscContextManager.OscForm.UpdateUi();
			}

			if (this.RuleSetDeleted != null)
			{
				RuleSetDeleted(this, rs);
			}
		}
		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.RuleSets.Clear();
				this.RuleSets.Added -= new EventHandler<OSCInformation.RuleSetEventArgs>(this.OnRuleSetAdded);
				this.RuleSets.Deleted -= new EventHandler<OSCInformation.RuleSetEventArgs>(this.OnRuleSetDeleted);
				this.RuleSets = null;
			}
		}
	}
}
