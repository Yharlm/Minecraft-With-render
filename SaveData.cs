using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Minecraft
{
    public class Files
    {
        public static void Save_map(int[,] grid) {
        string folderPath = @"C:\MyGame";
        string fileName = "World.json";


        Save.CreateFile(folderPath, fileName, grid); }
        
    }
    public class Save{
        public static void CreateFile(string folderPath, string fileName, int[,] grid, bool append = false)
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
        public static string ReadFile(string folderPath, string fileName)
        {
            // Combine the folder path and file name to get the full file path
            string filePath = Path.Combine(folderPath, fileName);
            // Read the content of the file
            try { return File.ReadAllText(filePath); }
            catch { return "No file detected. lol"; }
        }

        public static int[,] LoadWorld(string path,string filename)
        {
            string filePath = Path.Combine(path, filename);

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

        

    }

}
