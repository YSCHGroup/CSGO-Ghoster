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


namespace CSGO_Ghoster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public static class Ghoster
        {
            public static bool showDebugger { get; set; }
            public static bool showStatus { get; set; }

            public static string version { get; } = "1.0";
            public static string csgoPath { get; set; }
            public static string cfg_Path { get; set; }
            public static string data_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + Ghoster.version + @"\Data";
            public static string csgoPath_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + Ghoster.version + @"\Data\csgoPath.txt";
            public static string settings_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + Ghoster.version + @"\Data\settings.txt";
            public static string maps_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + Ghoster.version + @"\Maps";

            public static WebClient webClient = new WebClient();
            public static string publicIp { get; set; } = webClient.DownloadString("https://api.ipify.org");
            public static string standardPort { get; set; } = "8910";
            public static string publicPort { get; set; } = Ghoster.standardPort;

            public static string publicHttpUri { get; set; } = "http://" + Form1.Ghoster.publicIp + @":" + Form1.Ghoster.publicPort;
            public static string externalHttpUri { get; set; } = "http://" + Form1.Ghoster.publicIp + @":" + Form1.Ghoster.publicPort;

            public static string GhostingType { get; set; }
            public static string externalGhostIp { get; set; }

            public static string match_map { get; set; }
            public static int map_top { get; set; }
            public static int map_left { get; set; }
            public static int map_size { get; set; }

            public static bool map_prefer_spectate_radar { get; set; }

            public static bool player_names { get; set; }
            public static bool player_weapons { get; set; }
            public static bool player_hp { get; set; }
            public static bool player_height { get; set; }
        }

        void SetupFileSystem()
        {
            if (!System.IO.Directory.Exists(Ghoster.maps_Path)) {
                System.IO.Directory.CreateDirectory(Ghoster.maps_Path);
                DebugBox.Text = DebugBox.Text + "\nCreated: " + Ghoster.maps_Path;
            }
            if (!System.IO.Directory.Exists(Ghoster.data_Path)) {
                System.IO.Directory.CreateDirectory(Ghoster.data_Path);
                DebugBox.Text = DebugBox.Text + "\nCreated: " + Ghoster.data_Path;
            }
        }

        public void GetLocalMaps()
        {
            string mapPath = Ghoster.csgoPath + @"\csgo\resource\overviews";

            if (Directory.GetFiles(Ghoster.maps_Path, "*.png").Count() != Directory.GetFiles(mapPath, "*.dds").Count())
            {
                DialogResult updateMaps = MessageBox.Show("CS:GO Ghoster found new maps in your csgo folder! Do you want to update Ghoster's map library?", "New map available!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (updateMaps == DialogResult.Yes)
                {
                    string[] maps = Directory.GetFiles(mapPath, "*.dds");

                    int mapCounter = 0;

                    foreach (string map in maps)
                    {
                        string map_name = (map.Trim('"').Split('\\')[map.Contains("\\").ToString().Count() + 5]).Split('.')[0];
                        if (!File.Exists(Ghoster.maps_Path + "\\" + map_name + ".png"))
                        {
                            DebugBox.Text = DebugBox.Text + "\n" + map_name;
                            MagickImage mapImage = new MagickImage(map);
                            mapImage.Write(Ghoster.maps_Path + "\\" + map_name + ".png");

                            mapCounter += 1;
                        }
                    }
                }
            }
        }
        
        void GetCsgoPath()
        {
            if (File.Exists(Ghoster.csgoPath_Path) == true)
            {
                /// Load the saved path
                Ghoster.csgoPath = File.ReadAllText(Ghoster.csgoPath_Path);
                DebugBox.Text = DebugBox.Text + "\nFound prev. saved csgo path: \"" + Ghoster.csgoPath + "\"";
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
                        Ghoster.csgoPath = d.Name + @"ProgramFiles\Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"Program Files\Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        Ghoster.csgoPath = d.Name + @"Program Files\Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        Ghoster.csgoPath = d.Name + @"Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        Ghoster.csgoPath = d.Name + @"Steam\steamapps\common\Counter-Strike Global Offensive";
                    }
                    else if (System.IO.File.Exists(d.Name + @"SteamLibrary\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        Ghoster.csgoPath = d.Name + @"SteamLibrary\steamapps\common\Counter-Strike Global Offensive";
                    }
                }
                if (Ghoster.csgoPath == null)
                {   // Show new form to manually add the csgo path to
                    MessageBox.Show("Ooops! It seems like CSGO Ghoster couldn't find your csgo folder! Please enter your path manually!", "Csgo path not found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    using (Form2 f2 = new Form2())
                    {
                        f2.ShowDialog(this);
                    }

                    while (Ghoster.csgoPath == null)
                    {
                        if (File.Exists(Ghoster.csgoPath_Path))
                        {
                            Ghoster.csgoPath = File.ReadAllText(Ghoster.csgoPath_Path);
                            break;
                        }
                    }
                    DebugBox.Text = DebugBox.Text + "\nFound new csgo path after error: \"" + Ghoster.csgoPath + "\"";
                }
                else
                {
                    DebugBox.Text = DebugBox.Text + "\nFound new csgo path when searching: \"" + Ghoster.csgoPath + "\"";
                    (new FileInfo(Form1.Ghoster.csgoPath_Path)).Directory.Create();
                    File.WriteAllText(Form1.Ghoster.csgoPath_Path, Ghoster.csgoPath);
                }

                MessageBox.Show("Please restart CSGO to ensure Ghoster to work properly");
            }
        }

        public void LoadSettings()
        {
            if (File.Exists(Ghoster.settings_Path))
            {
                DebugBox.Text = DebugBox.Text + "\nLoading saved settings from data file...";

                // Setup default values
                Ghoster.showDebugger = false;
                DebugBox.Visible = false;
                Ghoster.showStatus = true;
                statusLabel.Visible = true;
                BackColor = Color.WhiteSmoke;
                Ghoster.map_top = 42;
                Ghoster.map_left = 2;
                Ghoster.map_size = 310;
                Ghoster.map_prefer_spectate_radar = false;

                Ghoster.player_height = false;
                Ghoster.player_names = false;
                Ghoster.player_hp = false;
                Ghoster.player_weapons = false;

                Ghoster.publicPort = Ghoster.standardPort;
                WebClient webClient = new WebClient();
                Ghoster.publicIp = webClient.DownloadString("https://api.ipify.org");


                // Read the file and display it line by line.  
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(Ghoster.settings_Path);

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        string[] setting = line.Split(':');
                        if (setting[0] == "MapTop" && setting[1] != "")
                        {
                            Ghoster.map_top = Int32.Parse(setting[1].ToString());
                            DebugBox.Text = DebugBox.Text + "\n-    Map Top Margin: " + Ghoster.map_top.ToString();
                        }
                        if (setting[0] == "MapLeft" && setting[1] != "")
                        {
                            Ghoster.map_left = Int32.Parse(setting[1].ToString());
                            DebugBox.Text = DebugBox.Text + "\n-    Map Left Margin: " + Ghoster.map_left.ToString();
                        }
                        if (setting[0] == "MapSize" && setting[1] != "")
                        {
                            Ghoster.map_size = Int32.Parse(setting[1].ToString());
                            DebugBox.Text = DebugBox.Text + "\n-    Map Size: " + Ghoster.map_size.ToString();
                        }
                        if (setting[0] == "Ip" && setting[1] != "")
                        {
                            Ghoster.publicIp = setting[1];
                            DebugBox.Text = DebugBox.Text + "\n-    Custom IP: " + Ghoster.publicIp.ToString();
                        }
                        if (setting[0] == "Port" && setting[1] != "")
                        {
                            Ghoster.publicPort = setting[1];
                            DebugBox.Text = DebugBox.Text + "\n-    Custom Port: " + Ghoster.publicPort.ToString();
                        }
                        if (setting[0] == "transparent" && setting[1] != "")
                        {
                            BackColor = Color.FromName(setting[1]);
                            TransparencyKey = Color.FromName(setting[1]);
                            DebugBox.Text = DebugBox.Text + "\n-    Transparent Mode on. (using key: " + setting[1] + ")";
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
                            Ghoster.showDebugger = true;
                            DebugBox.Visible = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Debug Mode on.";
                        }
                        else if (line == "hideStatus")
                        {
                            statusLabel.Visible = false;
                            Ghoster.showStatus = false;
                            DebugBox.Text = DebugBox.Text + "\n-    Hiding Status Information...";
                        }
                        else if (line == "playerNames")
                        {
                            Ghoster.player_names = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player Names on.";
                        }
                        else if (line == "playerWeapons")
                        {
                            Ghoster.player_weapons = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player Weapons on.";
                        }
                        else if (line == "playerHp")
                        {
                            Ghoster.player_hp = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player HP on.";
                        }
                        else if (line == "playerHeight")
                        {
                            Ghoster.player_height = true;
                            DebugBox.Text = DebugBox.Text + "\n-    Player Height on.";
                        }
                        else if (line == "preferSpectateRadar")
                        {
                            Ghoster.map_prefer_spectate_radar = true;
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
            Ghoster.publicHttpUri = "http://" + Form1.Ghoster.publicIp + @":" + Form1.Ghoster.publicPort;

            statusLabel.Visible = Ghoster.showStatus;
            DebugBox.Visible = Ghoster.showDebugger;
            this.Width = this.Height - 2 * 12;

            // Print public ip for debugpurposes
            DebugBox.Text = DebugBox.Text + "\nUsing IP: " + Ghoster.publicIp;
            DebugBox.Text = DebugBox.Text + "\nUsing Port: " + Ghoster.publicPort;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetCsgoPath();
            SetupFileSystem();
            GetLocalMaps();
            LoadSettings();
            GhosterWindowInformationLabel.Visible = false;
            
            // Create the cfg path
            Ghoster.cfg_Path = Ghoster.csgoPath + @"\csgo\cfg\gamestate_integration_ghoster_v" + Ghoster.version + @".cfg";
            // Create the gamestate_integration cfg file
            string integration_code = @"'CSGO Ghoster v " + Ghoster.version + @"'
{
	'uri' '" + Ghoster.publicHttpUri + @"'
	'timeout' '5.0'
    'buffer'  '0.1'
    'throttle' '0.1'
    'heartbeat' '.1'

    'auth'
	{
		'token'				'CSGO_Ghoster_" + Ghoster.version + @"'
	}
	'data'
	{
		'provider'              	'1'
		'map'                   	'1'
		'round'                 	'1'
		'player_id'					'1'
		'player_weapons'			'1'
		'player_match_stats'		'1'
		'player_state'				'1'
		'allplayers_position'       '1'
        'allplayers_id'				'1'
		'allplayers_state'			'1'
		'allplayers_match_stats'	'1'
	}
}";
            integration_code = integration_code.Replace('\'', '"'); // Replace all ' with "...

            (new FileInfo(Form1.Ghoster.cfg_Path)).Directory.Create();
            File.WriteAllText(Form1.Ghoster.cfg_Path, integration_code);
            DebugBox.Text = DebugBox.Text + "\nCreated cfg file: " + Ghoster.cfg_Path;
        }

        private void ipTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ipTextBox.Text.Length >= 7)
            {
                startGhostingButton.Enabled = true;
            }
            else
            {
                startGhostingButton.Enabled = false;
            }
        }

        private void startGhostLocalButton_Click(object sender, EventArgs e)
        {
            startGhostLocalButton.Enabled = false;

            Ghoster.GhostingType = "Local";
            Program.IntegrateGame();
            MessageBox.Show("OBS! Change your video settings to 'fullscreen windowed'!", "Important!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            while (CSGO_Ghoster.Program.isGhosting)
            {
                DebugBox.Text = DebugBox.Text + "\n> Ghosting Local match!";
            }

            // End of Local ghosting
            DebugBox.Text = DebugBox.Text + "\n> Ghosting match stopped...";
            statusLabel.Text = "Ghosting match stopped...";
            startGhostLocalButton.Enabled = true;
        }

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE | 0x0200 | 0x0040;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [System.Runtime.InteropServices.ComVisible(true)]

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;

            GhosterWindowInformationLabel.Visible = false;

            ipTextBox.Visible = false;
            startGhostingButton.Visible = false;
            startGhostLocalButton.Visible = false;
            replaceMapButton.Visible = false;
            settingsButton.Visible = false;

            DebugBox.Visible = false;
            statusLabel.Visible = false;
            
            // Place on top of all other applications or games etc...
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            TopMost = true;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;

            GhosterWindowInformationLabel.Visible = false;

            ipTextBox.Visible = true;
            startGhostingButton.Visible = true;
            startGhostLocalButton.Visible = true;
            replaceMapButton.Visible = true;
            settingsButton.Visible = true;

            DebugBox.Visible = Ghoster.showDebugger;
            statusLabel.Visible = Ghoster.showStatus;
        }

        private void startGhostingButton_Click(object sender, EventArgs e)
        {
            if (ipTextBox.Text.Contains("."))
            {
                if (ipTextBox.Text.Contains(":")) // Use custom port provided
                {
                    statusLabel.Text = "Attempting to connect to " + Ghoster.externalGhostIp + "...";
                    Ghoster.externalHttpUri = "http://" + Ghoster.externalGhostIp;
                }
                else
                {
                    statusLabel.Text = "Attempting to connect to " + Ghoster.externalGhostIp + ":" + Ghoster.standardPort + "...";
                    Ghoster.externalHttpUri = "http://"+ Ghoster.externalGhostIp + ":" + Ghoster.standardPort;
                }

                // Start Ghosting
                Ghoster.GhostingType = "External";
                Program.IntegrateGame();
                MessageBox.Show("OBS! Change your video settings to 'fullscreen windowed'!", "Important!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                startGhostingButton.Enabled = false;

                while (CSGO_Ghoster.Program.isGhosting)
                {
                    DebugBox.Text = DebugBox.Text + "\n> Ghosting External match!";
                }

                // End of Local ghosting
                DebugBox.Text = DebugBox.Text + "\n> Ghosting match stopped...";
                statusLabel.Text = "Ghosting match stopped...";
                startGhostingButton.Enabled = true;
            }
            else
            {
                statusLabel.Text = "Oops! Unvalid IP Address!";
            }
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
            Location = new Point(Ghoster.map_left, Ghoster.map_top);
            Size = new Size(Ghoster.map_size - (2 * 12), Ghoster.map_size);

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

            GhosterWindowInformationLabel.Text = @"Top: " + Location.X + @"
Left: " + Location.Y;
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
    }
}
