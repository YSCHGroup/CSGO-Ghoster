using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace CSGO_Ghoster
{
    class GhosterSettings
    {
        public static bool showDebugger { get; set; }
        public static bool showStatus { get; set; }

        public static string version { get; } = "1.0";
        public static string csgoPath { get; set; }
        public static string data_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + GhosterSettings.version + @"\Data";
        public static string csgoPath_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + GhosterSettings.version + @"\Data\csgoPath.txt";
        public static string settings_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + GhosterSettings.version + @"\Data\settings.txt";
        public static string maps_Path { get; } = @"C:\Users\" + Environment.UserName + @"\Documents\csgo\Ghoster " + GhosterSettings.version + @"\Maps";

        public static WebClient webClient = new WebClient();
        public static string publicIp { get; set; } = webClient.DownloadString("https://api.ipify.org");
        public static string standardPort { get; set; } = "8910";
        public static string publicPort { get; set; } = GhosterSettings.standardPort;

        public static string publicHttpUri { get; set; } = "http://" + GhosterSettings.publicIp + @":" + GhosterSettings.publicPort;
        public static string externalHttpUri { get; set; } = "http://" + GhosterSettings.publicIp + @":" + GhosterSettings.publicPort;

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
}
