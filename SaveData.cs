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
        

        
    }
    public class Files
    {
        
        public static void Save_map(int[,] grid)
        {
            string folderPath = GetSaveFilePath("SaveFile");
            
            
            string fileName = "World.json";
            

            Save.CreateFile(folderPath, fileName, grid);
        }
        public static void Save_Inventory(int[] grid)
        {
            string folderPath = GetSaveFilePath("SaveFile");


            string fileName = "Backpack.json";


            Save.CreateFile(folderPath, fileName, grid);
        }

        public static string GetSaveFilePath(string dir)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string saveFilePath = Path.Combine(documentsPath, "Minecraft-With-render", dir);
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
        public static void CreateFile(string folderPath, string fileName, int[] grid)
        {
            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);

            // Combine the folder path and file name to get the full file path
            string filePath = Path.Combine(folderPath, fileName);
            int[] jaggedArray = new int[grid.GetLength(0)];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                
                    jaggedArray[i] = grid[i];
                
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

        public static int[] Load_inventory(string path, string filename)
        {

            string filePath = Path.Combine(path, filename);
            //string fileRoot = Path.Combine(Directory.GetCurrentDirectory(), @"\World.json");
            // Read the content of the file
            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string to a jagged array
            int[] jaggedArray = JsonSerializer.Deserialize<int[]>(jsonString);

            // Convert the jagged array back to a multidimensional array
            int rows = jaggedArray.Length;
            
            int[] grid = new int[rows];

            for (int i = 0; i < rows; i++)
            {
                
                    grid[i] = jaggedArray[i];
                
            }

            return grid;
        }

        public static void Test()
        {
            string folderPath = Files.GetSaveFilePath("SaveFile");
            Directory.CreateDirectory(folderPath);
            File.WriteAllText(Path.Combine(folderPath, "World.json"), "Hi");
        }
    }
}