using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGO_Ghoster
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 10)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text + @"\csgo.exe"))
            {
                MessageBox.Show("Successfully found csgo path!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (new FileInfo(GhosterSettings.csgoPath_Path)).Directory.Create();
                File.WriteAllText(GhosterSettings.csgoPath_Path, textBox1.Text);
                this.Close();
            }else
            {
                MessageBox.Show("Failed to detect csgo path... Try again!","Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
