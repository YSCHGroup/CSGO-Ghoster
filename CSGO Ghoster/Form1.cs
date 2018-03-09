using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using CSGSI;
using CSGSI.Nodes;
using Newtonsoft.Json;

using ImageMagick;

using System.Runtime.InteropServices;
using System.Threading;


namespace CSGO_Ghoster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        void SetupFileSystem()
        {
            if (!System.IO.Directory.Exists(GhosterSettings.maps_Path)) {
                System.IO.Directory.CreateDirectory(GhosterSettings.maps_Path);
                DebugBox.Text = DebugBox.Text + "\nCreated: " + GhosterSettings.maps_Path;
            }
            if (!System.IO.Directory.Exists(GhosterSettings.data_Path)) {
                System.IO.Directory.CreateDirectory(GhosterSettings.data_Path);
                DebugBox.Text = DebugBox.Text + "\nCreated: " + GhosterSettings.data_Path;
            }
        }

        public void GetLocalMaps()
        {
            string mapPath = GhosterSettings.csgoPath + @"\csgo\resource\overviews";

            if (Directory.GetFiles(GhosterSettings.maps_Path, "*.png").Count() != Directory.GetFiles(mapPath, "*.dds").Count())
            {
                DialogResult updateMaps = MessageBox.Show("CS:GO Ghoster found new maps in your csgo folder! Do you want to update Ghoster's map library?", "New map available!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (updateMaps == DialogResult.Yes)
                {
                    string[] maps = Directory.GetFiles(mapPath, "*.dds");

                    int mapCounter = 0;

                    foreach (string map in maps)
                    {
                        string map_name = (map.Trim('"').Split('\\')[map.Contains("\\").ToString().Count() + 5]).Split('.')[0];
                        if (!File.Exists(GhosterSettings.maps_Path + "\\" + map_name + ".png"))
                        {
                            DebugBox.Text = DebugBox.Text + "\n" + map_name;
                            MagickImage mapImage = new MagickImage(map);
                            mapImage.Write(GhosterSettings.maps_Path + "\\" + map_name + ".png");

                            mapCounter += 1;
                        }
                    }
                }
            }
        }
        
        void GetCsgoPath()
        {
            if (File.Exists(GhosterSettings.csgoPath_Path) == true)
            {
                /// Load the saved path
                GhosterSettings.csgoPath = File.ReadAllText(GhosterSettings.csgoPath_Path);
                DebugBox.Text = DebugBox.Text + "\nFound prev. saved csgo path: \"" + GhosterSettings.csgoPath + "\"";
            }
            else
            {
                /// Search computer for ?\Steam\steamapps\common\Counter-Strike Global Offensive\
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    DebugBox.Text = DebugBox.Text + "\nSearching for csgo path in: " + d.Name;
                    if (System.IO.File.Exists(d.Name + @"ProgramFiles\Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        GhosterSettings.csgoPath = d.Name + @"ProgramFiles\Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"Program Files\Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        GhosterSettings.csgoPath = d.Name + @"Program Files\Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        GhosterSettings.csgoPath = d.Name + @"Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        GhosterSettings.csgoPath = d.Name + @"Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"SteamLibrary\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        GhosterSettings.csgoPath = d.Name + @"SteamLibrary\steamapps\common\Counter-Strike Global Offensive";
                    }
                }
                if (GhosterSettings.csgoPath == null)
                {   // Show new form to manually add the csgo path to
                    MessageBox.Show("Ooops! It seems like CSGO Ghoster couldn't find your csgo folder! Please enter your path manually!", "Csgo path not found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    using (Form2 f2 = new Form2())
                    {
                        f2.ShowDialog(this);
                    }

                    while (GhosterSettings.csgoPath == null)
                    {
                        if (File.Exists(GhosterSettings.csgoPath_Path))
                        {
                            GhosterSettings.csgoPath = File.ReadAllText(GhosterSettings.csgoPath_Path);
                            break;
                        }
                    }
                    DebugBox.Text = DebugBox.Text + "\nFound new csgo path after error: \"" + GhosterSettings.csgoPath + "\"";
                }
                else
                {
                    DebugBox.Text = DebugBox.Text + "\nFound new csgo path when searching: \"" + GhosterSettings.csgoPath + "\"";
                    (new FileInfo(GhosterSettings.csgoPath_Path)).Directory.Create();
                    File.WriteAllText(GhosterSettings.csgoPath_Path, GhosterSettings.csgoPath);
                }

                MessageBox.Show("Please restart CSGO to ensure Ghoster to work properly");
            }
        }

        public void LoadSettings()
        {
            if (File.Exists(GhosterSettings.settings_Path))
            {
                DebugBox.Text = DebugBox.Text + "\nLoading saved settings from data file...";

                // Setup default values
                GhosterSettings.showDebugger = false;
                DebugBox.Visible = false;
                GhosterSettings.showStatus = true;
                statusLabel.Visible = true;
                BackColor = Color.WhiteSmoke;
                GhosterSettings.map_top = 42;
                GhosterSettings.map_left = 3;
                GhosterSettings.map_size = 310;
                GhosterSettings.map_prefer_spectate_radar = false;

                GhosterSettings.player_height = false;
                GhosterSettings.player_names = false;
                GhosterSettings.player_hp = false;
                GhosterSettings.player_weapons = false;

                GhosterSettings.publicPort = GhosterSettings.standardPort;
                WebClient webClient = new WebClient();
                GhosterSettings.publicIp = webClient.DownloadString("https://api.ipify.org");

                GhosterSettings.radar_player_size = 3;

                // Read the file and display it line by line.  
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(GhosterSettings.settings_Path);

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        string[] setting = line.Split(':');
                        if (setting[0] == "MapTop" && setting[1] != "")
                        {
                            GhosterSettings.map_top = Int32.Parse(setting[1].ToString());
                            DebugBox.Text = DebugBox.Text + "\n-    Map Top Margin: " + GhosterSettings.map_top.ToString();
                        }
                        if (setting[0] == "MapLeft" && setting[1] != "")
                        {
                            GhosterSettings.map_left = Int32.Parse(setting[1].ToString());
                            DebugBox.Text = DebugBox.Text + "\n-    Map Left Margin: " + GhosterSettings.map_left.ToString();
                        }
                        if (setting[0] == "MapSize" && setting[1] != "")
                        {
                            GhosterSettings.map_size = Int32.Parse(setting[1].ToString());
                            DebugBox.Text = DebugBox.Text + "\n-    Map Size: " + GhosterSettings.map_size.ToString();
                        }
                        if (setting[0] == "Ip" && setting[1] != "")
                        {
                            GhosterSettings.publicIp = setting[1];
                            DebugBox.Text = DebugBox.Text + "\n-    Custom IP: " + GhosterSettings.publicIp.ToString();
                        }
                        if (setting[0] == "Port" && setting[1] != "")
                        {
                            GhosterSettings.publicPort = setting[1];
                            DebugBox.Text = DebugBox.Text + "\n-    Custom Port: " + GhosterSettings.publicPort.ToString();
                        }
                        if (setting[0] == "transparent" && setting[1] != "")
                        {
                            BackColor = Color.FromName(setting[1]);
                            TransparencyKey = Color.FromName(setting[1]);
                            DebugBox.Text = DebugBox.Text + "\n-    Transparent Mode on. (using key: " + setting[1] + ")";
                        }
                        if (setting[0] == "playerDotSize" && setting[1] != "")
                        {
                            GhosterSettings.radar_player_size = int.Parse(setting[1]);
                            DebugBox.Text = DebugBox.Text + "\n-    Player Dot Size: " + GhosterSettings.radar_player_size.ToString() + "px";
                        }
                    }
                    else
                    {
                        if (line == "transparent")
                        {
                            BackColor = Color.Gold;
                            TransparencyKey = Color.Gold;
                            DebugBox.Text = DebugBox.Text + "\n-    Transparent Mode on.";
                        }
                        else if (line == "debug")
                        {
                            GhosterSettings.showDebugger = true;
                            DebugBox.Visible = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Debug Mode on.";
                        }
                        else if (line == "hideStatus")
                        {
                            statusLabel.Visible = false;
                            GhosterSettings.showStatus = false;
                            DebugBox.Text = DebugBox.Text + "\n-    Hiding Status Information...";
                        }
                        else if (line == "playerNames")
                        {
                            GhosterSettings.player_names = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player Names on.";
                        }
                        else if (line == "playerWeapons")
                        {
                            GhosterSettings.player_weapons = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player Weapons on.";
                        }
                        else if (line == "playerHp")
                        {
                            GhosterSettings.player_hp = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player HP on.";
                        }
                        else if (line == "playerHeight")
                        {
                            GhosterSettings.player_height = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player Height on.";
                        }
                        else if (line == "preferSpectateRadar")
                        {
                            GhosterSettings.map_prefer_spectate_radar = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Prefer Spectate Radar on.";
                        }
                        else
                        {
                            DebugBox.Text = DebugBox.Text + "\n-    Unknown setting: \"" + line + "\"...";
                        }
                    }
                }

                file.Close();
            }
            else
            {
                DebugBox.Text = DebugBox.Text + "\nNo data file found...";
            }
            // After reading all saved settings, apply changes to other variables aswell...
            GhosterSettings.publicHttpUri = "http://" + GhosterSettings.publicIp + @":" + GhosterSettings.publicPort;

            statusLabel.Visible = GhosterSettings.showStatus;
            DebugBox.Visible = GhosterSettings.showDebugger;
            this.Width = this.Height - 2 * 12;

            // Print public ip for debugpurposes
            DebugBox.Text = DebugBox.Text + "\nUsing IP: " + GhosterSettings.publicIp;
            DebugBox.Text = DebugBox.Text + "\nUsing Port: " + GhosterSettings.publicPort;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetCsgoPath();
            SetupFileSystem();
            GetLocalMaps();
            LoadSettings();
            GhosterWindowInformationLabel.Visible = false;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void ipTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ipTextBox.Text.Length >= 7)
            {
                startRemoteGhostingButton.Enabled = true;
            }
            else
            {
                startRemoteGhostingButton.Enabled = false;
            }
        }

        private void startGhostLocalButton_Click(object sender, EventArgs e)
        {
            // Start Ghosting
            if (GhosterSettings.localMatchGhoster.ThreadState != ThreadState.Running)
            {
                GhosterSettings.localMatchGhoster.Start(); // Start Local Match Ghosting Thread
                GhosterSettings.localMatchGhoster.IsBackground = true; // <-- Set the thread to background
            }
            else
            {
                MessageBox.Show("CS:GO Ghoster is already running...", "The thread is already started!",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void startRemoteGhostingButton_Click(object sender, EventArgs e)
        {
            if (ipTextBox.Text.Contains("."))
            {
                if (ipTextBox.Text.Contains(":")) // Use custom port provided
                {
                    statusLabel.Text = "Attempting to connect to " + GhosterSettings.externalGhostIp + "...";
                    GhosterSettings.externalHttpUri = "http://" + GhosterSettings.externalGhostIp;
                }
                else
                {
                    statusLabel.Text = "Attempting to connect to " + GhosterSettings.externalGhostIp + ":" + GhosterSettings.standardPort + "...";
                    GhosterSettings.externalHttpUri = "http://" + GhosterSettings.externalGhostIp + ":" + GhosterSettings.standardPort;
                }

                // Start Ghosting
                Thread onlineMatchGhoster = new Thread(GhosterCore.StartRemoteGhosting);
                onlineMatchGhoster.IsBackground = true; // <-- Set the thread to background
                onlineMatchGhoster.Start();                                                                           // Start Online Match Ghosting Thread

                // End of Local ghosting
                DebugBox.Text = DebugBox.Text + "\n> Ghosting match stopped...";
                statusLabel.Text = "Ghosting match stopped...";
                startRemoteGhostingButton.Enabled = true;
            }
            else
            {
                statusLabel.Text = "Oops! Unvalid IP Address!";
            }
        }


        private void Form1_Deactivate(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;

            GhosterWindowInformationLabel.Visible = false;

            ipTextBox.Visible = false;
            startRemoteGhostingButton.Visible = false;
            startGhostLocalButton.Visible = false;
            replaceMapButton.Visible = false;
            settingsButton.Visible = false;

            DebugBox.Visible = false;
            statusLabel.Visible = false;
            
            GhosterCore.PlaceOnTop();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;

            GhosterWindowInformationLabel.Visible = false;

            ipTextBox.Visible = true;
            startRemoteGhostingButton.Visible = true;
            startGhostLocalButton.Visible = true;
            replaceMapButton.Visible = true;
            settingsButton.Visible = true;

            DebugBox.Visible = GhosterSettings.showDebugger;
            statusLabel.Visible = GhosterSettings.showStatus;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.Width = this.Height - 2 * 12;
            GhosterWindowInformationLabel.Visible = false;
        }

        private void replaceMapButton_Click(object sender, EventArgs e)
        {
            DebugBox.Text = DebugBox.Text + "\nUse map activated...";

            //Location = System.Drawing.Point.Empty;
            Location = new Point(GhosterSettings.map_left, GhosterSettings.map_top);
            Size = new Size(GhosterSettings.map_size - (2 * 12), GhosterSettings.map_size);

            GhosterWindowInformationLabel.Visible = false;
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.ShowDialog();

            // Wait for the settings window to close

            LoadSettings();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            GhosterWindowInformationLabel.Visible = true;
            GhosterWindowInformationLabel.Location = new Point(Width / 2 - 2 * GhosterWindowInformationLabel.Width / 3, Height / 2 - GhosterWindowInformationLabel.Height);

            GhosterWindowInformationLabel.Text = @"Map Size: " + Size.Height;
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            GhosterWindowInformationLabel.Visible = true;
            GhosterWindowInformationLabel.Location = new Point(Width / 2 - 2 * GhosterWindowInformationLabel.Width /3, Height / 2 - GhosterWindowInformationLabel.Height);

            GhosterWindowInformationLabel.Text = @"Top: " + Location.Y + @"
Left: " + Location.X;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            GhosterWindowInformationLabel.Visible = false;
        }

        private void GhosterWindowInformationLabel_MouseHover(object sender, EventArgs e)
        {
            GhosterWindowInformationLabel.Visible = false;
        }

        private void DebugBox_Click(object sender, EventArgs e)
        {
            DebugBox.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            DebugBox.Show();
        }

        private void GhosterWindowInformationLabel_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
            Application.Exit();
        }
    }
}
