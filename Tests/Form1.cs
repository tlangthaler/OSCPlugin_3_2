using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TouchOscLayoutParser;

namespace Tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //Open Select File Dialog
            var ofd = new OpenFileDialog
            {
                Filter = "OSC-Layout|*.touchOSC",
                Multiselect = false
            };
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _ = TouchOscLayout.Parse(ofd.FileName);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            /*
            
            string rule = "MeinControl_Test_1_2";
            string control = "MeinControl_Test";
            int NoOfControlsX = 5;
            int NoOfControlsY = 5;

            int index1 = rule.IndexOf("_", control.Length);
            int index2 = rule.IndexOf("_",index1+1);
            string test = rule.Substring(index2 + 1);
            
            bool xvalid = int.TryParse(rule.Substring(index1 + 1, index2 - index1 - 1), out int xPosition);
            bool yvalid = int.TryParse(rule.Substring(index2 + 1), out int yPosition);
            bool result =  xvalid & yvalid & xPosition <= NoOfControlsX & yPosition <= NoOfControlsY;
         */   
        }
    }
}
