using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSGO_Ghoster
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void applySettingsButton_Click(object sender, EventArgs e)
        {
            string path = Form1.Ghoster.settings_Path;
            bool settingsError = false;

            if (File.Exists(path)) // Delete old settings
            {
                File.Delete(path);
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                // Checkboxes
                if (checkBox5.Checked)  // Add debugging
                {
                    sw.WriteLine("debug");
                }
                if (checkBox4.Checked)  // Add status
                {
                    sw.WriteLine("hideStatus");
                }
                if (checkBox2.Checked)  // Add transparent
                {
                    sw.WriteLine("transparent");
                }


                if (checkBox1.Checked)
                {
                    sw.WriteLine("playerNames");
                }
                if (checkBox6.Checked)
                {
                    sw.WriteLine("playerWeapons");
                }
                if (checkBox1.Checked)
                {
                    sw.WriteLine("playerHeight");
                }
                if (checkBox3.Checked)
                {
                    sw.WriteLine("playerHp");
                }

                if (checkBox8.Checked)
                {
                    sw.WriteLine("preferSpectateRadar");
                }

                // Textboxes
                int i;
                if (int.TryParse(textBox1.Text, out i) && textBox1.Text.Length > 0)  // Add Window Top margin
                {
                    sw.WriteLine("MapTop:" + textBox1.Text);
                }
                else if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Error Occured in 'Window Top'! Please only enter a number value...","Settings Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    settingsError = true;
                }
                if (int.TryParse(textBox2.Text, out i) && textBox2.Text.Length > 0)  // Add Window Left margin
                {
                    sw.WriteLine("MapLeft:" + textBox2.Text);
                }
                else if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Error Occured in 'Window Left'! Please only enter a number value...", "Settings Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    settingsError = true;
                }
                if (int.TryParse(textBox3.Text, out i) && textBox3.Text.Length > 0)  // Add Map Size
                {
                    sw.WriteLine("MapSize:" + textBox3.Text);
                }
                else if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("Error Occured in 'Window Size'! Please only enter a number value...", "Settings Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    settingsError = true;
                }
                if (int.TryParse(textBox5.Text, out i))  // Custom Port
                {
                    sw.WriteLine("Port:" + textBox5.Text);
                }
                else if (!string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("Error Occured in 'Custom Port'! Please only enter a number value...", "Settings Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    settingsError = true;
                }
                if (textBox4.Text.Length > 0)  // Custom IP
                {
                    if (textBox4.Text.Length > 0)
                    {
                        sw.WriteLine("Ip:" + textBox4.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error Occured in 'Custom IP/DNS'! Unvalid IP/DNS...", "Settings Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        settingsError = true;
                    }
                }

                // ComboBoxes
                if (comboBox1.Enabled)
                {
                    if (!comboBox1.SelectedIndex.Equals(-1))
                    {
                        if (comboBox1.SelectedItem.ToString() != "Default")
                        {
                            sw.WriteLine("transparent:" + comboBox1.SelectedItem.ToString());
                        }
                    }
                }
            }

            if (!settingsError)
            {
                Form3.ActiveForm.Close();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            TopMost = true;
            string path = Form1.Ghoster.settings_Path;
            if (File.Exists(path)) // Load old settings
            {
                // Read the file and display it line by line.  
                string line;
                System.IO.StreamReader file =
                    new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        string[] setting = line.Split(':');
                        if (setting[0] == "MapTop" && setting[1] != "")
                        {
                            textBox1.Text = setting[1];
                        }
                        if (setting[0] == "MapLeft" && setting[1] != "")
                        {
                            textBox2.Text = setting[1];
                        }
                        if (setting[0] == "MapSize" && setting[1] != "")
                        {
                            textBox3.Text = setting[1];
                        }
                        if (setting[0] == "Ip" && setting[1] != "")
                        {
                            textBox4.Text = setting[1];
                        }
                        if (setting[0] == "Port" && setting[1] != "")
                        {
                            textBox5.Text = setting[1];
                        }
                        if (setting[0] == "transparent" && setting[1] != "")
                        {
                            comboBox1.SelectedItem = setting[1];
                        }
                    }
                    else
                    {
                        if (line == "transparent")
                        {
                            checkBox2.Checked = true;
                        }
                        else if (line == "debug")
                        {
                            checkBox5.Checked = true;
                        }
                        else if (line == "hideStatus")
                        {
                            checkBox4.Checked = true;
                        }
                        else if (line == "playerNames")
                        {
                            checkBox1.Checked = true;
                        }
                        else if (line == "playerWeapons")
                        {
                            checkBox6.Checked = true;
                        }
                        else if (line == "playerHeight")
                        {
                            checkBox7.Checked = true;
                        }
                        else if (line == "playerHp")
                        {
                            checkBox3.Checked = true;
                        }
                        else if (line == "preferSpectateRadar")
                        {
                            checkBox8.Checked = true;
                        }
                    }
                }
                file.Close();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult clearMaps = MessageBox.Show("Are you sure you want to clear your map library?", "Remove all saved maps?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (clearMaps == DialogResult.Yes)
            {
                string[] maps = Directory.GetFiles(Form1.Ghoster.maps_Path, "*.png");
                foreach(string map in maps)
                {
                    File.Delete(map);
                }
                MessageBox.Show("All maps removed...", "Cleared Map Library", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }
    }
}
