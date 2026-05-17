using System.IO;

namespace WPF.Helpers
{
    public class UserSettings
    {
        public static string settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),"WorldCupStats", "settings.txt");

        public static void Save(string language, string wc,string windowSize)
        {
            //Create folder if it doesn’t exist
            string folderPath = Path.GetDirectoryName(settingsFilePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //  Now write the settings file safely
            File.WriteAllLines(settingsFilePath, new string[] { language, wc, windowSize });
          
        }

        public static string[] Load()
        {
            // Ensure folder exists before reading settings
            string folderPath = Path.GetDirectoryName(settingsFilePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // If file doesn't exist, return default values
            if (!File.Exists(settingsFilePath))
            {
                return new string[] { "en-US", "Men", "1280x720" }; // Default settings
            }

            return File.ReadAllLines(settingsFilePath);
        }
    }
}
