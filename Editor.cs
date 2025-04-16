using System.Text.Json;

namespace Minecraft
{
    internal class Editor

    {


        class Selected
        {
            public int[,] selected_area;

        }

        class Files
        {
            public static int[,] LoadWorld(string path,string folder, string filename)
            {
                string filePath = Path.Combine(path, folder, filename);
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
                string saveFilePath = Path.Combine(documentsPath, "Minecraft-With-render");
                return saveFilePath;
            }
            public static void Save_map(int[,] grid)
            {
                string folderPath = Path.Combine(GetSaveFilePath(),"SaveFile");


                string fileName = "World.json";

                Save.CreateFile(folderPath, fileName, grid);
            }
            public static void Save_structure(int[,] grid, string dir, int[,] area)
            {
                
                string folderPath = Path.Combine(GetSaveFilePath(), "Structures");


                

                Save.CreateFile(folderPath, dir, area) ;
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
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Solid Default = new Solid("null", 0, null, default, default);
            Game Game = new Game();

            

            Default = new Solid("Air", 0, "  ", ConsoleColor.DarkGray, Game.Background); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Grass", 1, "▀▀", ConsoleColor.DarkGreen, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Dirt", 2, "██", ConsoleColor.DarkYellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Stone", 3, "██", ConsoleColor.DarkGray, ConsoleColor.DarkGray);; Game.Block_list.Add(Default);
            Default = new Solid("Log", 4, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);

            Default = new Solid("water", 5, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Default.Collidable = false; Game.Block_list.Add(Default);
            //Default = new Solid("water", 5, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Game.Block_list.Add(Default);

            Default = new Solid("waterTop", 6, "▄▄", ConsoleColor.DarkBlue, ConsoleColor.Cyan); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Leaves", 7, "▄▀", ConsoleColor.DarkGreen, ConsoleColor.Green); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Coal_ore", 8, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Black); Game.Block_list.Add(Default);
            Default = new Solid("Iron_ore", 9, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Yellow); Game.Block_list.Add(Default);
            Default = new Solid("Crafting_table", 10, "TT", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Wooden_planks", 11, "--", ConsoleColor.DarkYellow, ConsoleColor.DarkMagenta); Game.Block_list.Add(Default);
            Default = new Solid("Ladder", 12, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Sand", 13, "██", ConsoleColor.DarkMagenta, ConsoleColor.Cyan); Game.Block_list.Add(Default);
            Default = new Solid("Furnace", 14, "▀▀", ConsoleColor.DarkGray, ConsoleColor.Black); Game.Block_list.Add(Default);
            Default = new Solid("Tree", 15, "▀▀", ConsoleColor.Green, ConsoleColor.DarkYellow); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Torch", 16, "▄▄", ConsoleColor.DarkYellow, ConsoleColor.Magenta); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Cave_Background", 17, "  ", ConsoleColor.DarkYellow, ConsoleColor.Gray); Default.Collidable = false; Game.Block_list.Add(Default);

            Selected selected = new Selected();
            int[,] grid = new int[1000, 1000];
            grid = Files.LoadWorld(Files.GetSaveFilePath(), "SaveFile", "World.json");
            int x = 500;
            int y = 0;
            int id = 0;
            int size = 25;
            bool exiting = false;
            bool select_point_2 = false;
            Cordinates point1 = new Cordinates();
            Cordinates point2 = new Cordinates();
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
                WriteAt(Game.Block_list[id].Texture, size - 1, size / 2);
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
                        grid[y + size / 2, x + size / 2] = id;
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
                    case "Q":
                        if (select_point_2)
                        {
                            point2 = Cordinates.Convert_cor(x + size / 2, y + size / 2);
                            WriteAt("Point 2: " + point2.x + "," + point2.y, 20, 40);
                            select_point_2 = false;
                            continue;
                        }
                        point1 = Cordinates.Convert_cor(x + size / 2 + 1, y + size / 2 + 1);
                        WriteAt("Point 1: " + point1.x + "," + point1.y, 20, 41);

                        select_point_2 = true;

                        break;
                    case "D1":
                        Cordinates pos = new Cordinates();
                        pos.x = Math.Min(point1.x, point2.x);
                        pos.y = Math.Min(point1.y, point2.y);
                        int width = point2.x - point1.x;
                        width = Math.Abs(width);
                        int height = point2.y - point1.y;
                        height = Math.Abs(height);
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                grid[pos.y + i, pos.x + j] = id;
                            }
                        }
                        break;
                    case "D2":
                        pos = new Cordinates();
                        pos.x = Math.Min(point1.x, point2.x);
                        pos.y = Math.Min(point1.y, point2.y);
                        width = point2.x - point1.x;
                        width = Math.Abs(width);
                        height = point2.y - point1.y;
                        height = Math.Abs(height);
                        selected.selected_area = new int[width, height];
                        for (int i = 0; i < width; i++)
                        {
                            for (int j = 0; j < height; j++)
                            {
                                selected.selected_area[i, j] = grid[pos.y + i, pos.x + j];
                            }
                        }

                        break;
                    case "D3":
                        pos = new Cordinates();
                        pos = Cordinates.Convert_cor(x + size / 2, y + size / 2);
                        
                        for (int i = 0; i < selected.selected_area.GetLength(0); i++)
                        {
                            for (int j = 0; j < selected.selected_area.GetLength(1); j++)
                            {
                                grid[pos.y + i, pos.x + j] = selected.selected_area[i, j];
                            }
                        }
                        break;
                    case "D4":
                        
                        string Struct = Console.ReadLine();
                        Files.Save_structure(grid, Struct+".json", selected.selected_area);

                        break;
                    case "D5":
                        Struct = Console.ReadLine();
                        selected.selected_area = Files.LoadWorld(Files.GetSaveFilePath(), "Structures", Struct + ".json");

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



