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
using System.Windows.Forms;
using System.Drawing.Imaging;

using CSGSI;
using CSGSI.Nodes;
using Newtonsoft.Json;

using DevIL;
using ImageMagick;

using System.Runtime.InteropServices;

namespace CSGO_Ghoster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static bool isGhosting { get; set; } = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void IntegrateGame()
        {
            Form1 form1 = new Form1(); // Fix so we can access the layout of our main form!
            string ghostingType = Form1.Ghoster.GhostingType;

            //while (true)
            //{
                // Test what type of ghosting enabled...

                if (ghostingType == "Local")
                {
                    isGhosting = true;

                    form1.statusLabel.Text = "Starting Ghosting for local game... (" + Form1.Ghoster.publicHttpUri + ")";
                    form1.DebugBox.Text = form1.DebugBox.Text + "\n> Starting Ghosting for local game... (" + Form1.Ghoster.publicHttpUri + ")";

                    GameStateListener gsl;
                    gsl = new GameStateListener(Form1.Ghoster.publicHttpUri);
                    gsl.NewGameState += new NewGameStateHandler(OnNewGameState);
                    if (!gsl.Start())
                    {
                        Environment.Exit(0);
                        Application.Exit();
                    }
                    form1.DebugBox.Text = form1.DebugBox.Text + "\nListening...";

                }
                else if (ghostingType == "External")
                {
                    isGhosting = true;
                }
            //}
        }

        static void OnNewGameState(GameState gs)
        {
            Form1 frm1 = new Form1();
            frm1.statusLabel.Text = "Ghosting match started!";
            /// Find map and apply it to CS:GO Ghoster
            if (Form1.Ghoster.match_map != null & gs.Map.Name != null)
            {
                Form1.Ghoster.match_map = gs.Map.Name;
                frm1.DebugBox.Text = frm1.DebugBox.Text + "\nFound Map: " + Form1.Ghoster.match_map;
            }
            else if (gs.Map.Name != null)
            {
                string mapFile_path = Form1.Ghoster.maps_Path + @"\" + gs.Map.Name + @".png";

                if (File.Exists(mapFile_path))
                {
                    frm1.MapImage.BackgroundImage = Image.FromFile(mapFile_path);
                }
                else
                {
                    MessageBox.Show("Ooops! Seems like CS:GO Ghoster couldn't find your local map! Please try reloading CS:GO Ghoster... (or try enable 'Prefer spectate radar map' in settings...)", "Map not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
            else
            {
                frm1.MapImage.BackgroundImage = Properties.Resources.ghost_chicken;
                frm1.statusLabel.Text = "Users not playing! Waiting for a match...";
                Environment.Exit(0);
            }
        }
    }
}
