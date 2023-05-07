using Lumos.GUI.BaseWindow;
using Lumos.GUI.Controls.DataGrid;
using LumosControls.Controls;
using LumosControls.Controls.ToolStrip;
using OSCGUIPlugin.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using T = LumosLIB.Tools.I18n.DummyT;

namespace OSCGUIPlugin
{
    public class OSCForm : ToolWindow, IDisposable
	{
		private LumosMenuStrip menuStrip1;
        private LumosMenuStrip menuStrip2;
		private SplitContainer splitContainer;
		private LumosGroupBox devicesGrp;
		private LumosDataGridView devicesGrid;
		private LumosGroupBox rulesGrp;
		private LumosDataGridView rulesGrid;
		private LumosToolStripMenuItem addRuleSetToolStripMenuItem;
        private LumosToolStripMenuItem deleteRuleSetToolStripMenuItem;
        private LumosToolStripMenuItem toolsToolStripMenuItem;
        private LumosToolStripMenuItem exportRuleSetsToolStripMenuItem;
        private LumosToolStripMenuItem importRuleSetsToolStripMenuItem;
        private LumosToolStripMenuItem updateDevicesToolStripMenuItem;
        private LumosToolStripMenuItem addDeviceToolStripMenuItem;
        private LumosToolStripMenuItem deleteDevicesToolStripMenuItem;
        private LumosToolStripMenuItem importTouchOscLayout;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private LumosToolStripMenuItem infoToolStripMenuItem;
		public OSCForm()
		{
			this.InitializeComponent();
            this.MenuIconKey = "OSC";
			this.Text = T._("OSC-Management");
			OscContextManager.OscForm = this;
			this.rulesGrid.DataSource = null;
			base.Shown += new EventHandler(this.HandleFormShown);
		}
		private void HandleFormShown(object o, EventArgs e)
		{
			this.devicesGrid.DataSource = null;
            
			if (OscContextManager.DeviceInformation != null)
			{
				this.devicesGrid.DataSource = OscContextManager.DeviceInformation.Devices;
			}
			this.rulesGrid.DataSource = null;
			if (OscContextManager.OSCInformation != null)
			{
				this.rulesGrid.DataSource = OscContextManager.OSCInformation.RuleSets;
			}
			this.rulesGrid.KeyDown += new KeyEventHandler(this.HandleKeyDown);
			this.rulesGrid.CellDoubleClick += new DataGridViewCellEventHandler(this.HandleRulesDoubleClick);
			this.addRuleSetToolStripMenuItem.Click += new EventHandler(this.HandleAddRuleClick);
			this.deleteRuleSetToolStripMenuItem.Click += new EventHandler(this.HandleDeleteRuleClick);
			this.infoToolStripMenuItem.Click += new EventHandler(this.HandleInfoClick);
		}
		private void HandleKeyDown(object s, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode != Keys.Delete)
			{
				if (keyCode == Keys.F5)
				{
					this.AddNew();
				}
			}
			else
			{
				this.DeleteSelected();
			}
		}
		private void HandleRulesDoubleClick(object s, DataGridViewCellEventArgs e)
		{
            if (e.RowIndex == -1)
                return;
			OscRuleSet rs = OscContextManager.OSCInformation.RuleSets[e.RowIndex];
			rs.OpenEditWindow();
		}
		private void HandleAddRuleClick(object s, EventArgs e)
		{
			this.AddNew();
		}
		private void HandleDeleteRuleClick(object s, EventArgs e)
		{
			this.DeleteSelected();
		}
		private void HandleInfoClick(object s, EventArgs e)
		{
			MessageBox.Show(T._("Keybindings:\nF5 - Add RuleSet\ndel - Delete selected RuleSet"), T._("Keybindings"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		private void AddNew()
		{
			OscContextManager.OSCInformation.RuleSets.Add(new OscRuleSet());
		}
		private void DeleteSelected()
		{
			if (this.rulesGrid.SelectedRows.Count == 1)
			{
				OscRuleSet rs = (OscRuleSet)this.rulesGrid.SelectedRows[0].DataBoundItem;
				OscContextManager.OSCInformation.RuleSets.Remove(rs);
				rs.Dispose();
			}
		}
		public new void Dispose()
		{
			this.addRuleSetToolStripMenuItem.Click -= new EventHandler(this.HandleAddRuleClick);
			this.deleteRuleSetToolStripMenuItem.Click -= new EventHandler(this.HandleDeleteRuleClick);
			this.infoToolStripMenuItem.Click -= new EventHandler(this.HandleInfoClick);
			this.rulesGrid.KeyDown -= new KeyEventHandler(this.HandleKeyDown);
			this.rulesGrid.CellDoubleClick -= new DataGridViewCellEventHandler(this.HandleRulesDoubleClick);
			this.rulesGrid.DataSource = null;
			base.Dispose();
		}
		public void UpdateUi()
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new Action(this.UpdateUi));
			}
			else
			{
                this.devicesGrid.DataSource = null;

                if (OscContextManager.DeviceInformation != null)
                {
                    this.devicesGrid.DataSource = OscContextManager.DeviceInformation.Devices;
                }
                this.rulesGrid.DataSource = null;
                if (OscContextManager.OSCInformation != null)
                {
                    this.rulesGrid.DataSource = OscContextManager.OSCInformation.RuleSets;
                }

                this.rulesGrid.Refresh();
				this.devicesGrid.Refresh();
                this.Update();
			}
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            this.menuStrip1 = new LumosControls.Controls.LumosMenuStrip();
            this.updateDevicesToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.addDeviceToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.deleteDevicesToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.menuStrip2 = new LumosControls.Controls.LumosMenuStrip();
            this.addRuleSetToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.deleteRuleSetToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.infoToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.toolsToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.exportRuleSetsToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.importRuleSetsToolStripMenuItem = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.importTouchOscLayout = new LumosControls.Controls.ToolStrip.LumosToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.devicesGrp = new LumosControls.Controls.LumosGroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.devicesGrid = new Lumos.GUI.Controls.DataGrid.LumosDataGridView();
            this.rulesGrp = new LumosControls.Controls.LumosGroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.rulesGrid = new Lumos.GUI.Controls.DataGrid.LumosDataGridView();
            this.menuStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.devicesGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.devicesGrid)).BeginInit();
            this.rulesGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rulesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateDevicesToolStripMenuItem,
            this.addDeviceToolStripMenuItem,
            this.deleteDevicesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(766, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // updateDevicesToolStripMenuItem
            // 
            this.updateDevicesToolStripMenuItem.Name = "updateDevicesToolStripMenuItem";
            this.updateDevicesToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
            this.updateDevicesToolStripMenuItem.Text = T._("Update Devices");
            this.updateDevicesToolStripMenuItem.Click += new System.EventHandler(this.UpdateDevicesToolStripMenuItem_Click);
            // 
            // addDeviceToolStripMenuItem
            // 
            this.addDeviceToolStripMenuItem.Name = "addDeviceToolStripMenuItem";
            this.addDeviceToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
            this.addDeviceToolStripMenuItem.Text = T._("Add/Edit Device");
            this.addDeviceToolStripMenuItem.Click += new System.EventHandler(this.AddDevicesToolStripMenuItem_Click);
            // 
            // deleteDevicesToolStripMenuItem
            // 
            this.deleteDevicesToolStripMenuItem.Name = "deleteDevicesToolStripMenuItem";
            this.deleteDevicesToolStripMenuItem.Size = new System.Drawing.Size(163, 20);
            this.deleteDevicesToolStripMenuItem.Text = T._("Delete Selected Device");
            this.deleteDevicesToolStripMenuItem.Click += new System.EventHandler(this.DeleteDevicesToolStripMenuItem_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRuleSetToolStripMenuItem,
            this.deleteRuleSetToolStripMenuItem,
            this.infoToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(766, 24);
            this.menuStrip2.TabIndex = 0;
            this.menuStrip2.Text = "menuStrip1";
            // 
            // addRuleSetToolStripMenuItem
            // 
            this.addRuleSetToolStripMenuItem.Name = "addRuleSetToolStripMenuItem";
            this.addRuleSetToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.addRuleSetToolStripMenuItem.Text = T._("Add Rule Set");
            // 
            // deleteRuleSetToolStripMenuItem
            // 
            this.deleteRuleSetToolStripMenuItem.Name = "deleteRuleSetToolStripMenuItem";
            this.deleteRuleSetToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.deleteRuleSetToolStripMenuItem.Text = T._("Delete Rule Set");
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.infoToolStripMenuItem.Text = T._("Info");
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportRuleSetsToolStripMenuItem,
            this.importRuleSetsToolStripMenuItem,
            this.importTouchOscLayout});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.toolsToolStripMenuItem.Text = T._("Tools");
            // 
            // exportRuleSetsToolStripMenuItem
            // 
            this.exportRuleSetsToolStripMenuItem.Name = "exportRuleSetsToolStripMenuItem";
            this.exportRuleSetsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.exportRuleSetsToolStripMenuItem.Text = T._("Export RuleSets");
            this.exportRuleSetsToolStripMenuItem.Click += new System.EventHandler(this.ExportRuleSetsToolStripMenuItem_Click);
            // 
            // importRuleSetsToolStripMenuItem
            // 
            this.importRuleSetsToolStripMenuItem.Name = "importRuleSetsToolStripMenuItem";
            this.importRuleSetsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.importRuleSetsToolStripMenuItem.Text = T._("Import RuleSets");
            this.importRuleSetsToolStripMenuItem.Click += new System.EventHandler(this.ImportRuleSetsToolStripMenuItem_Click);
            // 
            // importTouchOscLayout
            // 
            this.importTouchOscLayout.Name = "importTouchOscLayout";
            this.importTouchOscLayout.Size = new System.Drawing.Size(225, 22);
            this.importTouchOscLayout.Text = T._("Import TouchOSC Layout");
            this.importTouchOscLayout.Click += new System.EventHandler(this.ImportTouchOscLayoutToolStripMenuItem_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.devicesGrp);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rulesGrp);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainer.Size = new System.Drawing.Size(790, 559);
            this.splitContainer.SplitterDistance = 251;
            this.splitContainer.TabIndex = 1;
            // 
            // devicesGrp
            // 
            this.devicesGrp.Controls.Add(this.splitContainer1);
            this.devicesGrp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.devicesGrp.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.devicesGrp.Location = new System.Drawing.Point(5, 5);
            this.devicesGrp.Name = "devicesGrp";
            this.devicesGrp.Padding = new System.Windows.Forms.Padding(7);
            this.devicesGrp.Size = new System.Drawing.Size(780, 241);
            this.devicesGrp.TabIndex = 0;
            this.devicesGrp.TabStop = false;
            this.devicesGrp.Text = T._("Devices");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(7, 22);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.devicesGrid);
            this.splitContainer1.Size = new System.Drawing.Size(766, 212);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 1;
            // 
            // devicesGrid
            // 
            this.devicesGrid.AllowUserToAddRows = false;
            this.devicesGrid.AllowUserToDeleteRows = false;
            this.devicesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.devicesGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.devicesGrid.CellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.devicesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.devicesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.devicesGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.devicesGrid.Location = new System.Drawing.Point(0, 0);
            this.devicesGrid.Name = "devicesGrid";
            this.devicesGrid.ReadOnly = true;
            this.devicesGrid.RubberColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.devicesGrid.ShowCellToolTips = false;
            this.devicesGrid.Size = new System.Drawing.Size(766, 183);
            this.devicesGrid.TabIndex = 0;
            this.devicesGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DevicesGrid_CellContentDoubleClick);
            this.devicesGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DevicesGrid_KeyDown);
            // 
            // rulesGrp
            // 
            this.rulesGrp.Controls.Add(this.splitContainer2);
            this.rulesGrp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rulesGrp.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.rulesGrp.Location = new System.Drawing.Point(5, 5);
            this.rulesGrp.Name = "rulesGrp";
            this.rulesGrp.Padding = new System.Windows.Forms.Padding(7);
            this.rulesGrp.Size = new System.Drawing.Size(780, 294);
            this.rulesGrp.TabIndex = 0;
            this.rulesGrp.TabStop = false;
            this.rulesGrp.Text = T._("Loaded Rule Sets");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(7, 22);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.menuStrip2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rulesGrid);
            this.splitContainer2.Size = new System.Drawing.Size(766, 265);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 1;
            // 
            // rulesGrid
            // 
            this.rulesGrid.AllowUserToAddRows = false;
            this.rulesGrid.AllowUserToDeleteRows = false;
            this.rulesGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rulesGrid.CellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rulesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rulesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rulesGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rulesGrid.Location = new System.Drawing.Point(0, 0);
            this.rulesGrid.Name = "rulesGrid";
            this.rulesGrid.RubberColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rulesGrid.ShowCellToolTips = false;
            this.rulesGrid.Size = new System.Drawing.Size(766, 236);
            this.rulesGrid.TabIndex = 1;
            // 
            // OSCForm
            // 
            this.ClientSize = new System.Drawing.Size(790, 559);
            this.Controls.Add(this.splitContainer);
            this.DoubleBuffered = true;
            this.Icon = global::OSCGUIPlugin.Properties.OSCGUIPluginResource.OSC_16;
            this.Location = new System.Drawing.Point(0, 0);
            this.MainFormMenu = LumosLIB.GUI.Windows.MenuType.Settings;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "OSCForm";
            this.TabText = T._("OSC-Management");
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.devicesGrp.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.devicesGrid)).EndInit();
            this.rulesGrp.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rulesGrid)).EndInit();
            this.ResumeLayout(false);

		}

        private void ExportRuleSetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export?.Invoke(null, null);
        }


        public event EventHandler Export;
        public event EventHandler Import;
        public event EventHandler ImportTouchOscLayout;

        private void ImportRuleSetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Import?.Invoke(null, null);
        }
        private void ImportTouchOscLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check is Row Selected
            OscRuleSet obj;
            if (this.rulesGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selected = this.rulesGrid.SelectedRows[0];
                obj = OscContextManager.OSCInformation.RuleSets[selected.Index];
            }
            else
            {
                obj = null;
            }

            ImportTouchOscLayout?.Invoke(obj, null);
        }

        private void UpdateDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OscContextManager.DeviceInformation.DeviceUpdate();
            this.devicesGrid.DataSource = typeof(System.Collections.Generic.List<OscDev>);
            this.devicesGrid.DataSource = OscContextManager.DeviceInformation.Devices;
            
            this.UpdateUi();
        }

        private void AddDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if 1 entry is selected for Editing
            OscAddDeviceDialog diag;
            OscDev dev = null;
            if (this.devicesGrid.SelectedRows.Count == 1)
            {
                dev = (OscDev)this.devicesGrid.SelectedRows[0].DataBoundItem;
            }

            if (dev is OscOutput devout && !devout.Autodetected)
            {
                diag = new OscAddDeviceDialog(dev.HostName, dev.IPAdress, dev.Port);
            }
            else
            {
                diag = new OscAddDeviceDialog();
                dev = null;
            }
            //Ask for Hostname, IPAdres, Port
            DialogResult res = diag.ShowDialog();
            if (res == DialogResult.OK)
            {
                //OscContextManager.Log.Debug("New Device Information: " + diag.HostName + " " + diag.IpAddress + " " + diag.Port);
                if (dev != null && dev is OscOutput devout1)
                {
                    devout1.UpdateDevice(diag.HostName, diag.IpAddress, diag.Port);
                }
                else
                {
                    OscContextManager.DeviceInformation.AddDevice(diag.IpAddress, diag.HostName, diag.Port, false);
                }
                this.devicesGrid.DataSource = typeof(System.Collections.Generic.List<OscDev>);
                this.devicesGrid.DataSource = OscContextManager.DeviceInformation.Devices;

                this.UpdateUi();
            }
        }

        private void DeleteDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.devicesGrid.SelectedRows.Count == 1)
            {
                OscDev dev = (OscDev)this.devicesGrid.SelectedRows[0].DataBoundItem;
                OscContextManager.DeviceInformation.DeleteDevice(dev.IPAdress, dev.HostName, dev.Port);
                this.devicesGrid.DataSource = typeof(System.Collections.Generic.List<OscDev>);
                this.devicesGrid.DataSource = OscContextManager.DeviceInformation.Devices;
                this.UpdateUi();
            }
        }

        private void DevicesGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Edit selected row
           OscDev dev = (OscDev)this.devicesGrid.Rows[e.RowIndex].DataBoundItem;
            if (dev is OscOutput devout && !devout.Autodetected)
            {
                OscAddDeviceDialog diag = new OscAddDeviceDialog(dev.HostName, dev.IPAdress, dev.Port);
                //Ask for Hostname, IPAdres, Port
                DialogResult res = diag.ShowDialog();
                if (res == DialogResult.OK)
                {
                    devout.UpdateDevice(diag.HostName, diag.IpAddress, diag.Port);
                    this.devicesGrid.DataSource = typeof(System.Collections.Generic.List<OscDev>);
                    this.devicesGrid.DataSource = OscContextManager.DeviceInformation.Devices;
                    this.UpdateUi();
                }
            }

        }

        private void DevicesGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (keyCode != Keys.Delete)
            {
                if (keyCode == Keys.F5)
                {
                    //Add/Edit Device
                    this.AddDevicesToolStripMenuItem_Click(sender, e);
                }
            }
            else
            {
                //Delete Device
                if (this.devicesGrid.SelectedRows.Count == 1)
                {
                    OscDev dev = (OscDev)this.devicesGrid.SelectedRows[0].DataBoundItem;
                    OscContextManager.DeviceInformation.DeleteDevice(dev.IPAdress, dev.HostName, dev.Port);
                    this.devicesGrid.DataSource = typeof(System.Collections.Generic.List<OscDev>);
                    this.devicesGrid.DataSource = OscContextManager.DeviceInformation.Devices;
                    this.UpdateUi();
                }
            }

        }
    }
}
