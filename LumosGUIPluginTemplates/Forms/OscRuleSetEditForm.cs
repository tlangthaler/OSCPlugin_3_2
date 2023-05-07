using Lumos.GUI.BaseWindow;
using Lumos.GUI.Controls.DataGrid;
using LumosControls.Controls;
using LumosControls.Controls.ToolStrip;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Lumos.GUI.Windows;
using T = LumosLIB.Tools.I18n.DummyT;
namespace OSCGUIPlugin
{
    public class OscRuleSetEditForm : ToolWindow
	{
		private LumosMenuStrip menuStrip1;
		private LumosToolStripMenuItem rulesToolStripMenuItem;
		private LumosToolStripMenuItem neuToolStripMenuItem;
		private LumosToolStripMenuItem deleteToolStripMenuItem;
		private LumosDataGridView ruleSetGrid;
		private LumosToolStripMenuItem beginLearnToolStrip;
		private SplitContainer splitContainer1;
		private PropertyGrid ruleInfo;
		private LumosToolStripMenuItem cancelLearnToolStripMenuItem;
		private readonly OscRuleSet rules;
		private readonly BindingSource bs;
		private bool learning;
		private IOscLearnable currentlearn;
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            this.menuStrip1 = new LumosControls.Controls.LumosMenuStrip();
            this.rulesToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.neuToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.deleteToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.beginLearnToolStrip = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.cancelLearnToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.ruleSetGrid = new Lumos.GUI.Controls.DataGrid.LumosDataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ruleInfo = new System.Windows.Forms.PropertyGrid();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ruleSetGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rulesToolStripMenuItem,
            this.beginLearnToolStrip,
            this.cancelLearnToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(556, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // rulesToolStripMenuItem
            // 
            this.rulesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.rulesToolStripMenuItem.Name = "rulesToolStripMenuItem";
            this.rulesToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.rulesToolStripMenuItem.Text = T._("Rules");
            // 
            // neuToolStripMenuItem
            // 
            this.neuToolStripMenuItem.Name = "neuToolStripMenuItem";
            this.neuToolStripMenuItem.ShowShortcutKeys = false;
            this.neuToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.neuToolStripMenuItem.Text = T._("New");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItem.Text = T._("Delete");
            // 
            // beginLearnToolStrip
            // 
            this.beginLearnToolStrip.Name = "beginLearnToolStrip";
            this.beginLearnToolStrip.Size = new System.Drawing.Size(88, 20);
            this.beginLearnToolStrip.Text = T._("Begin learn");
            // 
            // cancelLearnToolStripMenuItem
            // 
            this.cancelLearnToolStripMenuItem.Name = "cancelLearnToolStripMenuItem";
            this.cancelLearnToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.cancelLearnToolStripMenuItem.Text = T._("Cancel learn");
            // 
            // ruleSetGrid
            // 
            this.ruleSetGrid.AllowUserToAddRows = false;
            this.ruleSetGrid.AllowUserToDeleteRows = false;
            this.ruleSetGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ruleSetGrid.CellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ruleSetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ruleSetGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleSetGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ruleSetGrid.Location = new System.Drawing.Point(0, 0);
            this.ruleSetGrid.MultiSelect = false;
            this.ruleSetGrid.Name = "ruleSetGrid";
            this.ruleSetGrid.RubberColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ruleSetGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ruleSetGrid.ShowCellToolTips = false;
            this.ruleSetGrid.Size = new System.Drawing.Size(316, 387);
            this.ruleSetGrid.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ruleSetGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ruleInfo);
            this.splitContainer1.Size = new System.Drawing.Size(556, 387);
            this.splitContainer1.SplitterDistance = 316;
            this.splitContainer1.TabIndex = 2;
            // 
            // ruleInfo
            // 
            this.ruleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleInfo.Location = new System.Drawing.Point(0, 0);
            this.ruleInfo.Name = "ruleInfo";
            this.ruleInfo.Size = new System.Drawing.Size(236, 387);
            this.ruleInfo.TabIndex = 0;
            // 
            // OscRuleSetEditForm
            // 
            this.ClientSize = new System.Drawing.Size(556, 411);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = global::OSCGUIPlugin.Properties.OSCGUIPluginResource.OSC_16;
            this.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "OscRuleSetEditForm";
            this.TabText = T._("Osc-Rule-Management");
            this.Text = T._("Osc-Rule-Management");
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OscRuleSetEditForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ruleSetGrid)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		public OscRuleSetEditForm(OscRuleSet s)
		{
			this.rules = s;
            this.Text = T._("Edit rule set: ") + this.rules.Name;
            base.Name = T._("Edit rule set: ") + this.rules.Name;
			this.bs = new BindingSource
			{
				DataSource = this.rules.Rules
			};

			this.InitializeComponent();
			this.MenuIconKey = "OSC";
			this.ruleSetGrid.DataSource = this.bs;
			this.ruleSetGrid.SelectionChanged += new EventHandler(this.HandleSelectionChanged);
			this.ruleSetGrid.KeyDown += new KeyEventHandler(this.HandleKeyDown);
			
            //befüllen der Listbox
            OscAssemblyHelper h = OscContextManager.AssemblyHelper;
			foreach (Type item in h.DeviceRuleTypes)
			{
                OscToolStripDeviceRuleButton tsbtn = new OscToolStripDeviceRuleButton
                {
                    Name = item.Name,
                    Text = h.GetFriendlyName(item)
                };
                tsbtn.Click += new EventHandler(this.HandleAddRule);
				tsbtn.Type = item;
				this.neuToolStripMenuItem.DropDownItems.Add(tsbtn);
			}

			this.ruleInfo.SelectedObject = null;
			this.beginLearnToolStrip.Click += new EventHandler(this.HandleBeginLearnClick);
			this.deleteToolStripMenuItem.Click += new EventHandler(this.HandleDeleteClick);
			this.cancelLearnToolStripMenuItem.Click += new EventHandler(this.HandleCancelLearnClick);
			this.cancelLearnToolStripMenuItem.Enabled = false;
			this.ruleInfo.KeyDown += new KeyEventHandler(this.HandleKeyDown);
		}
		private void HandleKeyDown(object s, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode != Keys.Delete)
			{
				if (keyCode != Keys.C)
				{
					if (keyCode != Keys.L)
					{
						int dec = (int)e.KeyCode;
						if (dec >= 96 && dec <= 105)
						{
							int id = dec - 96;
							if (e.Control)
							{
								e.Handled = true;
								this.HandleAddRule(id);
							}
						}
						if (dec >= 48 && dec <= 57)
						{
							int id = dec - 48;
							if (e.Control)
							{
								this.HandleAddRule(id);
								e.Handled = true;
							}
						}
					}
					else
					{
						if (e.Control && !this.learning)
						{
							this.BeginLearn();
							e.Handled = true;
						}
					}
				}
				else
				{
					if (e.Control && this.learning)
					{
						this.CancelLearn();
						e.Handled = true;
					}
				}
			}
			else
			{
				this.DeleteSelected();
			}
		}
		private void HandleDeleteClick(object s, EventArgs e)
		{
			this.DeleteSelected();
		}
		private void HandleCancelLearnClick(object s, EventArgs e)
		{
			//log.Debug("HandleCancelLearnClick");

			this.CancelLearn();
		}
		private void HandleAddRule(object o, EventArgs e)
		{
			this.AddRule((o as OscToolStripDeviceRuleButton).Type);
		}
		private void HandleAddRule(int id)
		{
			ToolStripItem type = (this.neuToolStripMenuItem.DropDownItems.Count > id) ? this.neuToolStripMenuItem.DropDownItems[id] : null;
			if (type != null)
			{
				this.AddRule((type as OscToolStripDeviceRuleButton).Type);
			}
		}
		private void HandleBeginLearnClick(object s, EventArgs e)
		{
			//log.Debug("HandleBeginLearnClick");

			this.BeginLearn();
		}
		private void HandleLearnFinished(object s, EventArgs e)
		{
			//log.Debug("HandleLearnFinished");
			((IOscLearnable)s).LearningFinished -= new EventHandler(this.HandleLearnFinished);
			this.learning = false;
			this.currentlearn = null;
			if (base.InvokeRequired)
			{
				base.Invoke(new Action(this.UpdateUIPostLearn));
			}
			else
			{
				this.UpdateUIPostLearn();
			}
			//this.rules.InputUsed = false;
		}
		private void UpdateUIPostLearn()
		{
			this.beginLearnToolStrip.Enabled = true;
			this.cancelLearnToolStripMenuItem.Enabled = false;
			this.ruleSetGrid.Refresh();
			this.ruleInfo.Refresh();
		}
		private void HandleSelectionChanged(object s, EventArgs e)
		{
			if (this.ruleSetGrid.SelectedRows.Count > 0)
			{
				DataGridViewRow selected = this.ruleSetGrid.SelectedRows[0];
				OscDeviceRule obj = this.rules.Rules[selected.Index];
				this.ruleInfo.SelectedObject = obj;
				this.ruleInfo.ExpandAllGridItems();
				
			}
			else
			{
				this.ruleInfo.SelectedObject = null;
			}
		}
		private void CancelLearn()
		{
			//log.Debug("CancelLearn");

			this.currentlearn.CancelLearn();
			this.currentlearn = null;
		}
		private void DeleteSelected()
		{
			if (this.ruleSetGrid.SelectedRows.Count == 1)
			{
				OscDeviceRule rs = (OscDeviceRule)this.ruleSetGrid.SelectedRows[0].DataBoundItem;
				this.rules.DeleteRule(rs);
				if (this.ruleInfo.SelectedObject == rs)
				{
					this.ruleInfo.SelectedObject = null;
				}
			}
		}
		private void AddRule(Type t)
		{
			OscContextManager.Log.Debug("Execute AddRule Button", new object[0]);

			OscDeviceRule r = this.rules.CreateRule(t.FullName);
			if (r != null)
			{
				this.rules.AddRule(r);
			}
		}
		private void BeginLearn()
		{
			//log.Debug("BeginLearn");

			if (this.ruleSetGrid.SelectedRows.Count > 0)
			{
				
				if (!this.learning)
				{
					this.learning = true;
					this.beginLearnToolStrip.Enabled = false;
					this.cancelLearnToolStripMenuItem.Enabled = true;
					DataGridViewRow selected = this.ruleSetGrid.SelectedRows[0];
					OscDeviceRule obj = this.rules.Rules[selected.Index];
					obj.LearningFinished += new EventHandler(this.HandleLearnFinished);
					obj.BeginLearn();
					this.currentlearn = obj;
					this.ruleSetGrid.Refresh();
					this.ruleInfo.Refresh();
				}
			}
		}

        private void OscRuleSetEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
			OscContextManager.OscForm.UpdateUi();
        }
    }
}
