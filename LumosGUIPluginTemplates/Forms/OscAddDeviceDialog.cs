using Lumos.GUI.BaseWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using T = LumosLIB.Tools.I18n.DummyT;

namespace OSCGUIPlugin.Forms
{
    public partial class OscAddDeviceDialog : ToolWindow
    {
        public string HostName
        {
            get;
            private set;
        }
        public string IpAddress
        {
            get;
            private set;
        }
        public int Port
        {
            get;
            private set;
        }
        public OscAddDeviceDialog()
        {
            InitializeComponent();
            
        }

        public OscAddDeviceDialog(string HostName, string IpAddress, int Port):this()
        {
            //set internal properties
            this.HostName = HostName;
            this.hostNameTextBox.Text = HostName;
            this.IpAddress = IpAddress;
            this.ipAddrTextBox.Text = IpAddress;
            this.Port = Port;
            this.portTextBox.Text = Port.ToString();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //Validate inputs
            if (hostNameTextBox.Text == "")
            {
                //No hostname entered --> Show Messagebox
                MessageBox.Show(T._("No Hostname entered!"));
                return;
            }
            this.HostName = hostNameTextBox.Text;
            if (ipAddrTextBox.Text == "")
            {
                //No IPAddr entered --> Show Messagebox
                MessageBox.Show(T._("No IP-Address entered!"));
                return;
            }
            else
            {
                //Check IP-Adress Format
                string ipaddress = ipAddrTextBox.Text;
                bool ValidateIP = IPAddress.TryParse(ipaddress, out _);
                if (!ValidateIP)
                {
                    //No Valid IP-Adress
                    MessageBox.Show(T._("IP-Adress is not correct!"));
                    return;
                }
            }
            this.IpAddress = ipAddrTextBox.Text;
            if (portTextBox.Text=="")
            {
                // No Port
                MessageBox.Show(T._("No Port entered!"));
                return;
            }
            else 
            {
                bool ValidPort = int.TryParse(portTextBox.Text, out int port);
                if (!ValidPort)
                {
                    MessageBox.Show(T._("No valid Port entered"));
                    return;
                }
                this.Port = port;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
