using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Storage
{
    public class JsonFileManager
    {
        private static readonly string BASE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataLayer", "JSON");
        private static readonly string MEN_JSON_PATH = Path.Combine(BASE_PATH, "men_teams.json");
        private static readonly string WOMEN_JSON_PATH = Path.Combine(BASE_PATH, "women_teams.json");

        public static List<Match> LoadMatchesFromJson(string filePath)
        {
            
            if (!File.Exists(filePath)) //  Use filePath, not jsonPath!
            {
                Console.WriteLine($"ERROR: Matches JSON file NOT FOUND at {filePath}");
                return new List<Match>(); // Return empty list to prevent crashes
            }

            string jsonData = File.ReadAllText(filePath); //  Use filePath directly

            return JsonConvert.DeserializeObject<List<Match>>(jsonData);
        }

        public static List<Team> LoadTeamsFromJson(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);

                    Debug.WriteLine($"ERROR: JSON file NOT FOUND at {filePath}");
                    return JsonConvert.DeserializeObject<List<Team>>(json);
                }
                return new List<Team>();
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"ERROR: JSON file NOT FOUND at {filePath}");
                Console.WriteLine($"Error loading JSON: {ex.Message}");
                return new List<Team>();
            }
            
        }

    }
}
