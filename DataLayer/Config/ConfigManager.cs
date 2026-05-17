using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Config
{
    public class ConfigManager
    {
        public static bool UseApi { get; private set; }
        public static bool UseMenWorldCup { get; private set; }
        public static void LoadConfig()
        {
            string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

            if (!File.Exists(configFile))
            {
                Debug.WriteLine($" Config file missing! Creating default settings...");
                File.WriteAllText(configFile, "API\nMen"); //  Default settings (modify if needed)
            }

            string[] configLines = File.ReadAllLines(configFile);
            UseApi = configLines.Any(line => line.Trim().Equals("API", StringComparison.OrdinalIgnoreCase));
            UseMenWorldCup = configLines.Any(line => line.Trim().Equals("Men", StringComparison.OrdinalIgnoreCase));
        }
        public static void SetTournament(bool isMenWorldCup)
        {
            UseMenWorldCup = isMenWorldCup;
            string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            File.WriteAllText(configFile, UseApi ? "API\n" : "JSON\n" + (UseMenWorldCup ? "Men" : "Women"));
        }


    }
}
