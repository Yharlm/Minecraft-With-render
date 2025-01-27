using System.Data;
using System.Text.Json;
using cammera;

namespace Minecraft
{
    internal class Editor

    {


        class Files
        {
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
            public static string GetSaveFilePath()
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string saveFilePath = Path.Combine(documentsPath, "Minecraft-With-render", "SaveFile");
                return saveFilePath;
            }
            public static void Save_map(int[,] grid)
            {
                string folderPath = GetSaveFilePath();


                string fileName = "World.json";

                Save.CreateFile(folderPath,fileName, grid);
            }
        }




        protected static int origRow;
        protected static int origCol;
        class Solid(string name, int Id, string texture, ConsoleColor fG, ConsoleColor bG)
        {
            public bool Collidable = true;
            public bool solid = true;
            public int quantity = 0;
            public string Name = name;
            public int id = Id;
            public string Texture = texture;
            public ConsoleColor FG = fG;
            public ConsoleColor BG = bG;

        }

        class Game
        {
            public static List<Solid> Block_list = new List<Solid>();
            public static ConsoleColor Background = ConsoleColor.Cyan;

        }
        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }


        public static void Editor_mode()
        {

            Solid Default = new Solid("null", 0, null, default, default);
            Game Game = new Game();

            Default = new Solid("Air", 0, "  ", ConsoleColor.DarkGray, Game.Background); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Grass", 1, "▀▀", ConsoleColor.DarkGreen, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Dirt", 2, "██", ConsoleColor.DarkYellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Stone", 3, "██", ConsoleColor.DarkGray, ConsoleColor.DarkGray); Game.Block_list.Add(Default);
            Default = new Solid("Log", 4, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);

            Default = new Solid("water", 5, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Default.Collidable = false; Game.Block_list.Add(Default);
            //Default = new Solid("water", 5, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Game.Block_list.Add(Default);

            Default = new Solid("waterTop", 6, "▄▄", ConsoleColor.DarkBlue, ConsoleColor.Cyan); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Leaves", 7, "▄▀", ConsoleColor.DarkGreen, ConsoleColor.Green); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Coal_ore", 8, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Black); Game.Block_list.Add(Default);
            Default = new Solid("Iron_ore", 9, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Magenta); Game.Block_list.Add(Default);
            Default = new Solid("Crafting_table", 10, "TT", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Wooden_planks", 11, "==", ConsoleColor.DarkYellow, ConsoleColor.Yellow); Game.Block_list.Add(Default);
            Default = new Solid("Ladder", 12, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Sand", 13, "██", ConsoleColor.DarkMagenta, ConsoleColor.Cyan); Game.Block_list.Add(Default);
            Default = new Solid("Furnace", 14, "▀▀", ConsoleColor.DarkGray, ConsoleColor.Black); Game.Block_list.Add(Default);
            Default = new Solid("Tree", 15, "▀", ConsoleColor.Green, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);

            int[,] grid = new int[1000, 1000];
            grid = Files.LoadWorld(Files.GetSaveFilePath(), "World.json");
            int x = 500;
            int y = 0;
            int id = 0;
            int size = 19;
            bool exiting = false;
            while (true)
            {
                if (exiting == true)
                {
                    break;
                }
                for (int i = y; i < size + y; i++)
                {

                    for (int j = x; j < size + x; j++)
                    {
                        if (grid[i, j] == 0)
                        {

                            Render(Game.Block_list[0], j - x, i - y);
                        }
                        else
                        {
                            Render(Game.Block_list[grid[i, j]], j - x, i - y);
                        }

                    }

                }
                Console.ForegroundColor = Game.Block_list[id].FG;
                Console.BackgroundColor = Game.Block_list[id].BG;
                WriteAt(Game.Block_list[id].Texture, size-1, size / 2);
                var key = Console.ReadKey().Key;
                switch (key.ToString())
                {
                    case "D":
                        x++;
                        break;
                    case "S":
                        y++;
                        break;
                    case "W":
                        y--;
                        break;
                    case "A":
                        x--;
                        break;
                    case "Enter":
                        grid[y + size/2, x + size/2] = id;
                        break;
                    case "Spacebar":
                        grid[y + size / 2, x + size / 2] = 0;
                        break;
                    case "E":
                        id++;
                        if (id >= Game.Block_list.Count)
                        {
                            id = 0;
                        }
                        break;
                    case "Escape":
                        Files.Save_map(grid);
                        exiting = true;
                        break;
                    case "I":
                        Files.Save_map(grid);
                        

                        break;

                }


            }
            Console.Clear();
        }

        
        static void Render(Solid block, int x, int y)
        {
            Console.ForegroundColor = block.FG;
            Console.BackgroundColor = block.BG;

            WriteAt(block.Texture, x * 2, y);

            Console.ForegroundColor = default;
            Console.BackgroundColor = default;
        }
    }
}



