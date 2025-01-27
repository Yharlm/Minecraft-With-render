using System;
using System.IO;
using System.Text.Json;

namespace Minecraft
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using global::Minecraft.Minecraft;

    namespace Minecraft
    {
        public class Command
        {
            public string Action { get; set; }
            public bool SingleLine { get; set; }
            public string Split { get; set; }
            public string SplitMode { get; set; }
        }

        public class Action
        {
            public Command Command { get; set; }
            public string Keys { get; set; }
        }

        public class Profile
        {
            public string Commandline { get; set; }
            public string Guid { get; set; }
            public bool Hidden { get; set; }
            public string Name { get; set; }
            public string Source { get; set; }
            public string ColorScheme { get; set; }
            public bool ExperimentalRetroTerminalEffect { get; set; }
            public Font Font { get; set; }
            public int Opacity { get; set; }
        }

        public class Font
        {
            public int Weight { get; set; }
        }

        public class Scheme
        {
            public string Background { get; set; }
            public string Black { get; set; }
            public string Blue { get; set; }
            public string BrightBlack { get; set; }
            public string BrightBlue { get; set; }
            public string BrightCyan { get; set; }
            public string BrightGreen { get; set; }
            public string BrightPurple { get; set; }
            public string BrightRed { get; set; }
            public string BrightWhite { get; set; }
            public string BrightYellow { get; set; }
            public string CursorColor { get; set; }
            public string Cyan { get; set; }
            public string Foreground { get; set; }
            public string Green { get; set; }
            public string Name { get; set; }
            public string Purple { get; set; }
            public string Red { get; set; }
            public string SelectionBackground { get; set; }
            public string White { get; set; }
            public string Yellow { get; set; }
        }

        public class Profiles
        {
            public object Defaults { get; set; }
            public List<Profile> List { get; set; }
        }

        public class Settings
        {
            [JsonPropertyName("$help")]
            public string Help { get; set; }
            [JsonPropertyName("$schema")]
            public string Schema { get; set; }
            public List<Action> Actions { get; set; }
            public string CopyFormatting { get; set; }
            public bool CopyOnSelect { get; set; }
            public string DefaultProfile { get; set; }
            public string LaunchMode { get; set; }
            public Profiles Profiles { get; set; }
            public List<Scheme> Schemes { get; set; }
        }

        
    }
    public class Files
    {
        public static void EditSettings()
        {
            string jsonFilePath = "C:\\Users\\Teachers\\AppData\\Local\\Packages\\Microsoft.WindowsTerminal_8wekyb3d8bbwe\\LocalState\\settings.json";
            try
            {
                // Read the JSON file
                string jsonString = File.ReadAllText(jsonFilePath);
                Settings settings = JsonSerializer.Deserialize<Settings>(jsonString);

                // Modify the desired properties
                if (settings.Schemes != null && settings.Schemes.Count > 0)
                {
                    settings.Schemes[0].Red = "#FFFFFF"; // Change the red color to a new value
                }

                // Serialize the modified object back to JSON
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                string modifiedJsonString = JsonSerializer.Serialize(settings, options);

                // Write the JSON file
                File.WriteAllText(jsonFilePath, modifiedJsonString);

                Console.WriteLine("Settings updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or writing JSON file: {ex.Message}");
            }
        }
        public static void Save_map(int[,] grid)
        {
            string folderPath = GetSaveFilePath();
            
            
            string fileName = "World.json";
            

            Save.CreateFile(folderPath, fileName, grid);
        }

        public static string GetSaveFilePath()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string saveFilePath = Path.Combine(documentsPath, "Minecraft-With-render", "SaveFile");
            return saveFilePath;
        }
    }

    public class Save
    {
        public static void CreateFile(string folderPath, string fileName, int[,] grid)
        {
            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);

            // Combine the folder path and file name to get the full file path
            string filePath = Path.Combine(folderPath, fileName);
            int[][] jaggedArray = new int[grid.GetLength(0)][];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                jaggedArray[i] = new int[grid.GetLength(1)];
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    jaggedArray[i][j] = grid[i, j];
                }
            }

            string jsonString = JsonSerializer.Serialize(jaggedArray);

            // Write the content to the file (overwrite if it exists)
            File.WriteAllText(filePath, jsonString);
        }

       

        public static int[,] LoadWorld(string path, string filename)
        {
            
            string filePath = Path.Combine(path, filename);
            //string fileRoot = Path.Combine(Directory.GetCurrentDirectory(), @"\World.json");
            // Read the content of the file
            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string to a jagged array
            int[][] jaggedArray = JsonSerializer.Deserialize<int[][]>(jsonString);

            // Convert the jagged array back to a multidimensional array
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;
            int[,] grid = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = jaggedArray[i][j];
                }
            }

            return grid;
        }

        public static void Test()
        {
            string folderPath = Files.GetSaveFilePath();
            Directory.CreateDirectory(folderPath);
            File.WriteAllText(Path.Combine(folderPath, "World.json"), "Hi");
        }
    }
}