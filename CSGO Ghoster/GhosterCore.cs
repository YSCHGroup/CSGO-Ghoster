using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Remoting.Messaging;

namespace CSGO_Ghoster
{
    class GhosterCore
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        #region Setup

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory( IntPtr hProcess,  IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        public const Int32 ClientMapName = 0x4A69E5C;
        public const Int32 EngineMapName = 0x5DD1D0;
        /*
        Current Map Addresses:
        06007140
		08994580
		engine.dll+58B460
		engine.dll+7AE9B9
		engine.dll+826688
        */
        public static string LocalProcessName = "csgo";
        public static int bClient;
        public static int bEngine;

        static bool GetModuleAddClient()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(LocalProcessName);

                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if (m.ModuleName == "client.dll")
                        {
                            bClient = (int)m.BaseAddress;
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        } // Function for testing if the game is running

        static bool GetModuleAddEngine()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(LocalProcessName);

                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if (m.ModuleName == "engine.dll")
                        {
                            bEngine = (int)m.BaseAddress;
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        } // Function for getting the engine
        
        #endregion


        // Ghosting types:

        public static void StartLocalGhosting()
        {
            Form1 f = Application.OpenForms["Form1"] as Form1;
            f.DebugBox.Text = f.DebugBox.Text + "\n> Local Ghosting Started!";

            Thread.Sleep(1000);

            VAMemory vam = new  VAMemory(LocalProcessName);


            if (GetModuleAddClient() && GetModuleAddEngine())
            {
                // CSGO is running
                f.DebugBox.Text = f.DebugBox.Text + "\nFound Client.dll and Engine.dll!\n   > Waiting for map...";

                while (true)
                {
                    PlaceOnTop();

                    /*string map_name = vam.ReadStringASCII();

                    f.DebugBox.Text += "map_name";

                    GhosterCore.DrawPlayer(1, 15, 15);*/
                    //break;
                }

                // End of Local ghosting
                f.DebugBox.Text = f.DebugBox.Text + "\n> Local Ghosting match stopped...";
                f.statusLabel.Text = "Local Ghosting match stopped...";
            }
            else
            {
                // CSGO isn't started
                MessageBox.Show("Couldn't Find CS:GO... Please make sure to start a game before ghosting!", "Game Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                f.DebugBox.Text = f.DebugBox.Text + "\nCounter Strike: GO isn't started, please start your game before trying to ghost!";
            }
        }

        public static void StartRemoteGhosting()
        {
            Form1 f = Application.OpenForms["Form1"] as Form1;
            f.DebugBox.Text = f.DebugBox.Text + "\n> Remote Ghosting Started!";

            DisableMainFormControls();

            /*
             * Check if the entered ipaddress / DNS has an valid CSGO Ghoster Match Beacon
             * (That it's running on the other's computer and sending out information)
             */


            // End of Local ghosting
            EnableMainFormControls();
            f.DebugBox.Text = f.DebugBox.Text + "\n> Remote Ghosting match stopped...";
            f.statusLabel.Text = "Remote Ghosting match stopped...";
        }

        public static void DrawPlayer(int team, int x, int y)
        {
            Form1 f = Application.OpenForms["Form1"] as Form1;

            if (f == null )
             {
                 return;
             }

            //Graphics g = Form1.ActiveForm.CreateGraphics();
            //Graphics g = f.CreateGraphics();
            System.Drawing.Graphics g = f.CreateGraphics();

            f.Refresh();

            int drawPlayerRadius = GhosterSettings.radar_player_size;

            switch (team)
            {
                case 1:
                    var tBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Coral);
                    g.FillEllipse(tBrush, new Rectangle(x - drawPlayerRadius, y - drawPlayerRadius, x + drawPlayerRadius, y + drawPlayerRadius));
                    tBrush.Dispose();
                    g.Dispose();
                    break;
                case 2:
                    var ctBrush = new System.Drawing.SolidBrush(System.Drawing.Color.DodgerBlue);
                    g.FillEllipse(ctBrush, new Rectangle(x - drawPlayerRadius, y - drawPlayerRadius, x + drawPlayerRadius, y + drawPlayerRadius));
                    ctBrush.Dispose();
                    g.Dispose();
                    break;
                default:
                    f.DebugBox.Text = f.DebugBox.Text + "\n  Illegal event in DrawPlayer!";
                    break;
            }
        }

        public static void DisableMainFormControls()
        {
            Form1 f = Application.OpenForms["Form1"] as Form1;
            f.settingsButton.Enabled = false;
            f.startGhostLocalButton.Enabled = false;
            f.startRemoteGhostingButton.Enabled = false;
            f.ipTextBox.Enabled = false;
        }

        public static void EnableMainFormControls()
        {
            Form1 f = Application.OpenForms["Form1"] as Form1;
            f.settingsButton.Enabled = true;
            f.startGhostLocalButton.Enabled = true;
            f.startRemoteGhostingButton.Enabled = true;
            f.ipTextBox.Enabled = true;
        }



        #region Setup for window on top

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE | 0x0200 | 0x0040;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [System.Runtime.InteropServices.ComVisible(true)]

        #endregion


        public static void PlaceOnTop()
        {
            // Place on top of all other applications or games etc...
            Form1 f = Application.OpenForms["Form1"] as Form1;
            SetWindowPos(f.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            f.TopMost = true;
        }
    }
}
