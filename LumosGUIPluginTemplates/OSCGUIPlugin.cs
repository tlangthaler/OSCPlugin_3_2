using Lumos.GUI;
using Lumos.GUI.Connection;
using Lumos.GUI.Plugin;
using Lumos.GUI.Resource;
using Lumos.GUI.Run;
using LumosLIB.Kernel.Log;
using org.dmxc.lumos.Kernel.Messaging.Listener;
using org.dmxc.lumos.Kernel.Messaging.Message;
using org.dmxc.lumos.Kernel.Resource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using TouchOscLayoutParser;

namespace OSCGUIPlugin
{
    public class OSCGUIPlugin : GuiPluginBase,IMessageListener,IResourceProvider
    {
        public static readonly ILumosLog log = LumosLogger.getInstance(typeof(OSCGUIPlugin));
        const string PLUGIN_ID = "{979e5bb0-a1a2-4b9a-81f9-06434cbef490}";
        public const string PLUGIN_NAME = "OSCPlugin";
        private static readonly LumosResourceMetadata myMetaData = new LumosResourceMetadata("OSCSettings.xml", ELumosResourceType.MANAGED_TREE);
        private bool projectLoaded = false;
        private OSCForm form;
        private OscDeviceInformation devices;
        private OSCInformation osc;

        public OSCGUIPlugin() : base(PLUGIN_ID,PLUGIN_NAME)
        {
            OscContextManager.Log = log;
        }

        protected override void initializePlugin()
        {
            try
            {
                
                log.Debug("Initialize OSCPlugin!");
                new OscAssemblyHelper();
                this.devices = new OscDeviceInformation();
                this.osc = new OSCInformation();
                ResourceManager.getInstance().registerResourceProvider(this);

            }
            catch (Exception ex)
            {
                log.Error("Error initializing plugin...", ex);
            }
        }

        protected override void shutdownPlugin()
        {
            try
            {
                if (projectLoaded) Close();
                log.Debug("Shutdown OSCPlugin!");
                this.devices.Stop();
                ConnectionManager.getInstance().deregisterMessageListener(this, "KernelInputLayerManager", "LinkChanged");
                ConnectionManager.getInstance().deregisterMessageListener(this, "ExecutorManager", "OnExecutorChanged");
                WindowManager.getInstance().RemoveWindow(this.form);
                if (this.form.InvokeRequired)
                {
                    this.form.Invoke(new Action(this.form.Close));
                }
                else
                {
                    this.form.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error shutting down plugin...", ex);
            }
        }

        protected override void startupPlugin()
        {
            try
            {
                if (ConnectionManager.getInstance().Connected)
                    Load();
                log.Debug("Startup OSCPlugin!");
                this.form = new OSCForm();
                this.form.Import += HandleImport;
                this.form.Export += HandleExport;
                this.form.ImportTouchOscLayout += HandleTouchOscImport;
                this.devices.Start();
                WindowManager.getInstance().AddWindow(this.form);
            }
            catch (Exception ex)
            {
                log.Error("Error startup plugin...", ex);
            }
        }

        public void onMessage(IMessage message)
        {
            throw new NotImplementedException();
        }
        private void HandleCurrentExecutorPageChanged(object sender, EventArgs e)
        {
            log.Warn("ExecutorPage changed.");
        }

        private void HandleExport(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".xml",
                AddExtension = true,
                Filter = "OSC-Mapping|*.xml"
            };
            var dr = sfd.ShowDialog();
            if (dr != DialogResult.OK) return;
            var xelement = Serialize();
            xelement.Save(sfd.FileName);
        }

        private void HandleImport(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "OSC-Mapping|*.xml",
                Multiselect = false
            };
            var dr = ofd.ShowDialog();
            if (dr != DialogResult.OK) return;
            LoadFromXml(XElement.Load(ofd.FileName));
        }
        private void HandleTouchOscImport(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "TouchOSC-Layout|*.touchOSC",
                Multiselect = false
            };
            var dr = ofd.ShowDialog();
            if (dr != DialogResult.OK) return;
            try
            {
                LoadFromTouchOsc(TouchOscLayout.Parse(ofd.FileName),sender);
                MessageBox.Show("Import successful!");
            }
            catch (Exception ex)
            {
                log.Error("Error during import of TouchOSC File", ex, new object[0]);
                MessageBox.Show("Error During Import. \nSee Log-File for Details");
            }
        }

        public override void saveProject(LumosGUIIOContext context)
        {
            if (!ConnectionManager.getInstance().Connected) return;
            log.Debug("SaveProject in OSCPlugin");
            base.saveProject(context);
            this.Save();
        }
        public override void loadProject(LumosGUIIOContext context)
        {
            if (!ConnectionManager.getInstance().Connected) return;
            log.Debug("LoadProject in OSCPlugin");
            base.loadProject(context);
            this.Load();
        }
        public override void closeProject(LumosGUIIOContext context)
        {
            if (!ConnectionManager.getInstance().Connected) return;
            log.Debug("CloseProject in OSCPlugin");
            base.closeProject(context);
            this.Close();
        }
        public override void connectionClosing()
        {
            log.Debug("Connection closing in OSCPlugin...");

            base.connectionClosing();
        }
        public override void connectionEstablished()
        {
            log.Debug("Connection established in OSCPlugin...");

            if (ConnectionManager.getInstance().Connected)
                this.Load();

            base.connectionEstablished();

        }
        private void Close()
        {
            List<OscRuleSet> rules = new List<OscRuleSet>(this.osc.RuleSets);
            foreach (OscRuleSet item in rules)
            {
                this.osc.RuleSets.Remove(item);
            }
            projectLoaded = false;
        }
        private void Load()

        {
            if (ResourceManager.getInstance().existsResource(EResourceType.PROJECT, OSCGUIPlugin.myMetaData))
            {
                LumosResource r = ResourceManager.getInstance().loadResource(EResourceAccess.READ_WRITE, EResourceType.PROJECT, OSCGUIPlugin.myMetaData);
                ManagedTreeItem item = r.ManagedData;
                foreach (ManagedTreeItem mti in item.GetChildren("RuleSet"))
                {
                    OscRuleSet rs = OscRuleSet.Load(mti);
                    if (rs != null)
                    {
                        this.osc.RuleSets.Add(rs);
                    }
                }
                foreach(ManagedTreeItem mti in item.GetChildren("OutDevice"))
                {
                    OscOutput output = OscOutput.Load(mti);
                    if (output != null)
                    {
                        this.devices.OutputDevices.Add(output);
                    }
                }
                projectLoaded = true;
            }
        }
        private void Save()
        {
            log.Debug("Save called!");
            ManagedTreeItem _midi = new ManagedTreeItem("OSCSettings");
            foreach (OscRuleSet item in this.osc.RuleSets)
            {
                ManagedTreeItem rs = new ManagedTreeItem("RuleSet");
                item.Save(rs);
                _midi.AddChild(rs);
            }
            foreach(OscOutput output in this.devices.OutputDevices)
            {
                if (!output.Autodetected)
                {
                    ManagedTreeItem rs = new ManagedTreeItem("OutDevice");
                    output.Save(rs);
                    _midi.AddChild(rs);
                }
            }
            log.Debug("Creating resource");
            LumosResource res = new LumosResource(OSCGUIPlugin.myMetaData.Name, _midi);
            log.Debug("Resource created: {0}", res.ManagedData.Children.Count);
            ResourceManager.getInstance().saveResource(EResourceType.PROJECT, res);


            log.Debug("Resource saved");
        }

        public XElement Serialize()
        {
            var xElement = new XElement("OSCSettings");
            foreach (OscRuleSet item in this.osc.RuleSets)
            {
                xElement.Add(item.Serialize());
            }

            return xElement;
        }

        public void LoadFromXml(XElement element)
        {
            foreach (var item in element.Elements("RuleSet"))
            {
                var rs = OscRuleSet.LoadFromXml(item);
                this.osc.RuleSets.Add(rs);
            }
        }
        public void LoadFromTouchOsc(TouchOscLayout layout,object sender)
        {
            //Check if Ruleset is selected
            if (sender != null && sender is OscRuleSet ruleset)
            {
                //RuleSet is selected --> Ask User if Ruleset should be updated
                DialogResult res = MessageBox.Show("Ruleset '" + ruleset.Name + "' is selected. \nShould this ruleset be updated with the information from the layout?\nOtherwise a new one will be created!", "Update Selected", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    //Do Update
                    ruleset.UpdateFromTouchOsc(layout);
                    return;
                }
            }
            //Check if ruleset with same name available
            OscRuleSet found = null;
            foreach(OscRuleSet set in this.osc.RuleSets)
            {
                if (set.Name.Equals(layout.Name))
                {
                    //found
                    found = set;
                }
            }
            if (found != null)
            {
                //Ask User if Found Ruleset should be updated
                DialogResult res = MessageBox.Show("Ruleset '" + found.Name + "' with the same name as the layout was found. \nShould this ruleset be updated with the information from the layout?\nOtherwise a new one will be created!", "Update Found", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    //Do update
                    found.UpdateFromTouchOsc(layout);
                    return;
                }

            }
            var rs = OscRuleSet.LoadFromTouchOsc(layout);
            this.osc.RuleSets.Add(rs);
        }
        protected override void DisposePlugin(bool disposing)
        {
            if (this.osc != null)
            {
                this.osc.Dispose();
            }
            if (this.devices != null)
            {
                this.devices.Dispose();
            }
            base.DisposePlugin(disposing);
        }

        public bool existsResource(EResourceDataType type, string name)
        {
            if (type == EResourceDataType.SYMBOL)
            {
                if (name.Equals("OSC") || name.Equals("OSC_16") || name.Equals("OSC_32"))
                    return true;
            }
            return false;
        }

        public ReadOnlyCollection<LumosDataMetadata> allResources(EResourceDataType type)
        {
            if (type == EResourceDataType.SYMBOL)
            {
                List<LumosDataMetadata> ret = new List<LumosDataMetadata>()
                {
                    new LumosDataMetadata("OSC"),
                    new LumosDataMetadata("OSC_16"),
                    new LumosDataMetadata("OSC_32"),
                };
                return ret.AsReadOnly();
            }

            return null;
        }

        public byte[] loadResource(EResourceDataType type, string name)
        {
            if (type == EResourceDataType.SYMBOL)
            {
                switch (name)
                {
                    case "OSC":
                    case "OSC_32":
                        return IconToBytes(Properties.OSCGUIPluginResource.OSC_32);

                    case "3DxIcon_16":
                        return IconToBytes(Properties.OSCGUIPluginResource.OSC_16);
                }
            }

            return null;
        }
        private static byte[] IconToBytes(Icon icon)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
