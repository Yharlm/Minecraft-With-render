using Minecraft;
using System;
using System.Diagnostics;
using System.Numerics;

namespace cammera
{
    internal class Program
    {
        

        protected static int origRow;
        protected static int origCol;

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
        

        static void Main(string[] args)
        {
            Game Game = new Game();
            Player player = new Player();


            Solid Default = new Solid("null", 0, null, default, default);
            Non_solid Background = new Non_solid("", 0, null, default, default);

            Default = new Solid("Air", 0, "  ", ConsoleColor.DarkGray, ConsoleColor.Cyan); Game.Block_list.Add(Default);
            Default = new Solid("Grass", 1, "▀▀", ConsoleColor.DarkGreen, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Dirt", 2, "██", ConsoleColor.DarkYellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Stone", 3, "██", ConsoleColor.DarkGray, ConsoleColor.DarkGray); Game.Block_list.Add(Default);
            Default = new Solid("Log", 4, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);

            Default = new Solid("water", 5, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Game.Block_list.Add(Default);
            //Default = new Solid("water", 5, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Game.Block_list.Add(Default);

            Default = new Solid("waterTop", 6, "▄▄", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Game.Block_list.Add(Default);
            Default = new Solid("Leaves", 7, "▄▀", ConsoleColor.DarkGreen, ConsoleColor.Green); Game.Block_list.Add(Default);
            Default = new Solid("Coal_ore", 8, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Black); Game.Block_list.Add(Default);
            Default = new Solid("Iron_ore", 9, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Magenta); Game.Block_list.Add(Default);
            Default = new Solid("Crafting_table", 10, "TT", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Wooden_planks", 11, "▄▄", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("water", 12, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Game.Block_list.Add(Default);

            Camera camera = new Camera();
            int[,] grid = new int[1000, 1000];
            
            BuildWorld(grid, player, Game);
            double tick = 0.0002;
            while (true)
            {
                //double timer = Math.Ceiling(overworld.time += 0.0002);
                double timer = Game.time += 0.0002;
                if (Game.time >= tick)
                {

                    Game.curent_tick = true;
                    Game.time = 0;

                }
                else
                {

                    Game.curent_tick = false;
                }
                camera.Position.x = player.x-camera.View.GetLength(1) / 2;
                camera.Position.y = player.y - camera.View.GetLength(0)/2;
                



                for (int i = 0; i < camera.View.GetLength(0); i++)
                {
                    for (int j = 0; j < camera.View.GetLength(1); j++)
                    {
                        Render_block(Game.Get_ByID(grid[i+camera.Position.y, j+ camera.Position.x]), j, i);
                        //Fill_block(player.x, player.y, camera.View, Game.GetBlock("Stone"));
                        Console.ForegroundColor = ConsoleColor.Gray;
                        //WriteAt("EE",player.x*2,player.y);
                        WriteAt("██", camera.View.GetLength(1) - 1, (camera.View.GetLength(0) / 2)-1);
                        WriteAt("██", camera.View.GetLength(1) - 1, camera.View.GetLength(0) / 2);
                        Console.ForegroundColor = default;
                    }
                    
                }
                if (grid[player.y + 1, player.x] == 0 && Game.curent_tick)
                {

                    if (grid[player.y - 2, player.x] == 0)
                    {
                        
                    }
                    

                    player.y++;
                    player.grounded = false;
                    Console.ForegroundColor = ConsoleColor.White;

                    //WriteAt("  ", player.x * 2, player.y);

                    Console.ForegroundColor = ConsoleColor.Cyan;

                    //Thread.Sleep(100);


                }
                else
                {
                    player.grounded = true;
                }
                GetInput(grid,player,Game,camera);
                
            }

        }

        
            private static void GetInput(int[,] grid, Player player, Game game,Camera camera)
        {

            //Block_ids air = new Block_ids(0, "  ", default, ConsoleColor.Cyan);
            //Block_ids Grass = new Block_ids(1, "▀▀", ConsoleColor.DarkGreen, ConsoleColor.DarkYellow);
            //Block_ids dirt = new Block_ids(2, "██", ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);
            //Block_ids stone = new Block_ids(3, "██", ConsoleColor.DarkGray, ConsoleColor.DarkGray);
            //Block_ids wood = new Block_ids(4, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            //Block_ids water = new Block_ids(5, "██", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue);
            //Block_ids waterTop = new Block_ids(6, "▄▄", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue);
            //Block_ids leaves = new Block_ids(7, "▄▀", ConsoleColor.DarkGreen, ConsoleColor.Green);
            Random random = new Random();


            Solid air = game.Block_list[0];



            grid[player.y, player.x] = 0;
            int x = player.x;
            int y = player.y;

            if (Console.KeyAvailable == true)
            {
                WriteAt(" ", 50, 12);

                player.Input = Console.ReadKey().Key.ToString(); 
                if (player.Input != "Spacebar" && player.Input != "L" && player.Input != "K" && player.Input != "R")
                {
                    player.last_key = player.Input;

                }
                if (player.Input == "Spacebar")
                {
                    player.special_key = "Spacebar";
                }
                //if(grid[player.y - 1, player.x + 1] == 5)
                //{
                //    Fill_block(x, y,grid,water);
                //    Fill_block(x, y-1, grid, water);
                //}
                //else if(grid[player.y - 1, player.x + 1] == 0)
                //{
                //    Fill_block(x, y, grid, air);
                //    Fill_block(x, y - 1, grid, air);
                //}

                //WriteAt("  ", x * 2, y - 1);
                //WriteAt("  ", x * 2, y);
                //WriteAt("  ", 110, 0);
            }
            else { player.Input = null; }
            Cordinates player_cords = new Cordinates();
            player_cords.x = x;
            player_cords.y = y;
            //player.Selected_block = dirt;
            switch (player.Input)
            {
                case "X":


                    //Shoot_Projectile(player, game, player_cords, player.Entity_hotbar);
                    break;
                case "Q":
                    player.Crafting_select++;

                    if (player.Crafting_select == player.Recipes.Count)
                    {
                        player.Crafting_select = 0;
                    }
                    player.Entity_hotbar++;
                    if (player.Entity_hotbar >= 2)
                    {
                        player.Entity_hotbar = 0;
                    }
                    
                    break;

                case "R":
                    //Attack(game, Convert_cor(player.x, player.y), grid, 4, 5, 2);
                    //Slash(player, game, grid);


                    break;
                case "N":

                    try
                    {

                        foreach (Entity entity in game.Existing_Entities)
                        {


                            WriteAt("  ", entity.cordinates.x * 2, entity.cordinates.y);
                            Cordinates cordinates = entity.cordinates;
                            game.Existing_Entities.Remove(entity);
                            //Explosion(game, grid, cordinates, player, 4);

                        }
                    }
                    catch { }

                    break;
                case "C":


                    //Craft(player, player.Recipes[player.Crafting_select]);


                    //Print_window(player);


                    break;
                case "Y":
                    foreach (Entity ent in game.Existing_Entities)
                    {
                        WriteAt("  ", ent.cordinates.x * 2, ent.cordinates.y);
                    }
                    game.Existing_Entities.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteAt(game.Existing_Entities.Count().ToString() + "  ", 24, 3);
                    Console.ForegroundColor = default;
                    break;
                case "T":
                    {

                        WriteAt(game.Existing_Entities.Count().ToString(), 24, 3);

                        Entity mob = game.Entity_list[1];
                        Entity Default = new Entity(mob.Name, mob.Health, mob.Type, mob.Sprite);
                        Default.Color = mob.Color;
                        //Default.cordinates.x = random.Next(4, 55);
                        Default.cordinates.x = random.Next(30, 70);
                        Default.cordinates.y = player.y - 10;



                        game.Existing_Entities.Add(Default);
                        //game.Spawn_entity(entity);

                        break;
                    }
                case "E":
                    player.hotbar++;

                    player.Selected_block = game.Block_list[player.hotbar];

                    Console.ForegroundColor = ConsoleColor.Red;
                    Print_window(camera, game, player);

                    if (player.hotbar == game.Block_list.Count - 1) player.hotbar = 0;


                    break;
                case "K":

                    {

                        if (player.special_key == "Spacebar")
                        {
                            if (player.special_key == "Spacebar")
                            {
                                Break_block(player.x, player.y - 2, grid, air, game); player.special_key = null;

                            }
                            else if (player.last_key == "D")
                            {
                                Break_block(player.x + 1, player.y - 2, grid, air, game);
                            }
                            else if (player.last_key == "A")
                            {
                                Break_block(player.x - 1, player.y - 2, grid, air, game);
                            }
                        }

                        else if (player.last_key == "D")
                        {
                            if (grid[player.y - 1, player.x + 1] != 0)
                                Break_block(player.x + 1, player.y - 1, grid, air, game);
                            else if (grid[player.y, player.x + 1] != 0)
                                Break_block(player.x + 1, player.y, grid, air, game);
                            else if (grid[player.y - 2, player.x + 1] != 0)
                                Break_block(player.x + 1, player.y - 2, grid, air, game);
                        }
                        else if (player.last_key == "A")
                        {
                            if (grid[player.y - 1, player.x - 1] != 0)
                                Break_block(player.x - 1, player.y - 1, grid, air, game);
                            else if (grid[player.y, player.x - 1] != 0)
                                Break_block(player.x - 1, player.y, grid, air, game);
                            else if (grid[player.y - 2, player.x - 1] != 0)
                                Break_block(player.x - 1, player.y - 2, grid, air, game);
                        }
                        else if (player.last_key == "S")
                        {
                            Break_block(player.x, player.y + 1, grid, air, game);
                        }
                    }

                    //Print_window(player);

                    break;
                case "L":
                    if (player.Selected_block.quantity > 0)
                    {
                        if (player.special_key == "Spacebar")
                            if (grid[player.y + 1, player.x] == 0 && grid[player.y + 2, player.x] != 0)
                            {
                                Fill_block(player.x, player.y + 1, grid, player.Selected_block);
                                player.last_key = null;
                                player.Selected_block.quantity--;
                            }
                            else if (player.last_key == "S")
                            {
                                player.last_key = "D"; player.Selected_block.quantity--;
                            }
                            else if (player.last_key == "D")
                            {
                                if (grid[player.y + 1, player.x + 1] == 0)
                                    Fill_block(player.x + 1, player.y + 1, grid, player.Selected_block);
                                else if (grid[player.y, player.x + 1] == 0)
                                    Fill_block(player.x + 1, player.y, grid, player.Selected_block);
                                else if (grid[player.y - 1, player.x + 1] == 0)
                                    Fill_block(player.x + 1, player.y - 1, grid, player.Selected_block);
                                player.Selected_block.quantity--;
                            }
                            else if (player.last_key == "A")
                            {
                                if (grid[player.y + 1, player.x - 1] == 0)
                                    Fill_block(player.x - 1, player.y + 1, grid, player.Selected_block);
                                else if (grid[player.y, player.x - 1] == 0)
                                    Fill_block(player.x - 1, player.y, grid, player.Selected_block);
                                else if (grid[player.y - 1, player.x - 1] == 0)
                                    Fill_block(player.x - 1, player.y - 1, grid, player.Selected_block);
                                player.Selected_block.quantity--;
                            }
                        //Print_window(player);
                    }
                    break;
                case "Spacebar":
                    if (grid[player.y + 1, player.x] != 0)
                    {

                        if (grid[player.y - 2, player.x] == 0)
                        {
                            y -= 1;
                        }
                        if (grid[player.y - 3, player.x] == 0 && grid[player.y - 2, player.x] == 0)
                        {
                            y -= 1;
                        }
                        if (grid[player.y - 3, player.x] == 0 && grid[player.y - 2, player.x] == 0 && grid[player.y - 4, player.x] == 0)
                        {
                            y -= 1;
                        }


                    }



                    break;
                case "A":

                    camera.Position.x--;
                    if (player.grounded == false)
                    {
                        

                    }

                    //grid[player.y , player.x - 1] = 0;
                    //WriteAt("  ", (x-1) * 2, y );
                    if (grid[player.y, player.x - 1] == 0 && grid[player.y - 1, player.x - 1] == 0)
                    {

                        x--;
                    }
                    if (grid[player.y - 1, player.x - 1] == 6)
                    {
                        x--; player.is_swiming = true;
                    }
                    else
                    {
                        player.is_swiming = false;
                    }

                    break;
                case "S":

                    if (grid[player.y + 1, player.x] == 0)
                    {

                        y++;
                    }
                    break;
                case "D":
                    camera.Position.x++;

                    if (player.grounded == false)
                    {
                        

                    }
                    if (grid[player.y, player.x + 1] == 0 && grid[player.y - 1, player.x + 1] == 0)
                    {

                        x++;
                    }
                    if (grid[player.y - 1, player.x + 1] == 6)
                    {
                        x++; player.is_swiming = true;
                    }
                    else
                    {
                        player.is_swiming = false;
                    }
                    break;
            }
            
            player.Input = null;
            player.x = x;
            player.y = y;
            



        }

        static void Break_block(int x, int y, int[,] grid, Solid Block, Game game)
        {

            Console.ForegroundColor = Block.FG;
            Console.BackgroundColor = Block.BG;
            game.Block_list.Find(i => i.id == grid[y, x]).quantity++;
            grid[y, x] = Block.id;
            
            Console.ForegroundColor = default;
            Console.BackgroundColor = ConsoleColor.Cyan;

        }



        static void Render_block(Solid block, int x, int y)
        {
            Console.ForegroundColor = block.FG;
            Console.BackgroundColor = block.BG;
            WriteAt(block.Texture, x * 2, y);
            Console.ForegroundColor = default;
            Console.BackgroundColor = default;
        }

        private static void BuildWorld(int[,] grid, object instance, Game game)
        {
            Random random = new Random();
            Player player = (Player)instance;
            ConsoleColor Default = ConsoleColor.Cyan;


            Structure tree = new Structure();
            tree.Struct = new int[,]{
                { 0,7,7,7,0 },
                { 0,7,4,7,0 },
                { 7,7,4,7,7 },
                { 7,7,4,7,7 },
                { 0,0,4,0,0 },
                { 0,0,4,0,0 }
            };

            Structure House = new Structure();
            House.Struct = new int[,]{
                { 4,4,4,4,4,4,4,4 },
                { 4,0,0,0,0,0,0,4 },
                { 0,0,0,0,0,0,0,4 },
                { 0,0,0,0,0,0,0,4 },
                { 3,3,3,3,3,3,3,3 }
            };



            Generate_terrain(game, grid);

            int tree_rate = 34
           ;
            int Tree_r = 0;
            for (int i = 20; i < grid.GetLength(1)-40; i++)
            {
                Tree_r = random.Next(1, tree_rate);
                if (Tree_r >= tree_rate - 2)
                {

                    structure(tree, i, grid, game);
                    i += 5;
                }
            }


            //structure(House, 31, grid, player);
        }

        static void structure(object struc, int Local_x, int[,] grid, Game game)
        {
            Structure structure = (Structure)struc;
            //Block_ids block = (Block_ids)Block;
            //Solid tile = (Solid)Block;
            //int[,] str =
            //{
            //    {0,0,1,0,0 },
            //    {0,0,1,0,0 },
            //    {0,1,1,0,0 },
            //    {0,0,1,0,0 },
            //    {0,0,1,0,0 }
            //};

            int x = structure.Struct.GetLength(1);
            int y = structure.Struct.GetLength(0);

            int Local_y = 0;
            while (grid[Local_y, Local_x] == 0)
            {
                Local_y++;
            }
            Local_y -= y;
            for (int i = Local_y; i < Local_y + y; i++)
            {
                for (int j = Local_x; j < Local_x + x; j++)
                {
                    if (structure.Struct[i - Local_y, j - Local_x] == 0)
                    {
                        continue;
                    }
                    int ID = structure.Struct[i - Local_y, j - Local_x];


                    Solid block = game.Block_list.Find(x => x.id == structure.Struct[i - Local_y, j - Local_x]);
                    Fill_block(j, i, grid, block);

                    //Console.ForegroundColor = block.FG;
                    //Console.BackgroundColor = block.BG;
                    //grid[i, j] = block.id;
                    //WriteAt(block.Texture, j * 2, i);
                    //Console.ForegroundColor = default;
                    //Console.BackgroundColor = ConsoleColor.Cyan;


                    //grid[i, j] = structure.Struct[i - Local_y, j - Local_x];
                    //WriteAt(game.Block_list(1).t, j * 2, i);
                    //Console.ForegroundColor = default;
                    //Console.BackgroundColor = ConsoleColor.Cyan;

                }
            }


        }
        static void Generate_terrain(Game game, int[,] grid)
        {
            Random random = new Random();
            int Width = 1000;
            ; int Height = 104
            ;
            int dirt_Height = 5;
            int stone_Height = 42;
            int min = 1;
            int max = 5;
            int c = 0;



            for (int j = 2; j < Width; j++)
            {
                c = random.Next(min, max + 1);
                if (c == min)
                { Height++; }
                else if (c == max)
                { Height--; }

                Fill_block(j, Height, grid, game.GetBlock("Grass"));

                int count = 1;
                while (count < dirt_Height)
                {
                    Fill_block(j, Height + count, grid, game.GetBlock("Dirt"));
                    count++;
                }

                while (count < stone_Height)
                {
                    Fill_block(j, Height + count, grid, game.GetBlock("Stone"));
                    count++;
                }
            }


            //for (int j = 0; j < Width;)
            //{
            //    int coalN = random.Next(14, 24);
            //    int vein = random.Next(1, 6);

            //    if (random.Next(1, 30) < 4)
            //    {
            //        Fill_Index_Cord2(j, Height + coalN - vein, j + vein, Height + coalN, grid, game.GetBlock("Coal_ore"),5);
            //    }
            //    j++;
            //}
        }

        private static void Fill_block(int x, int y, int[,] grid, Solid Block)
        {
            Console.ForegroundColor = Block.FG;
            Console.BackgroundColor = Block.BG;
            grid[y, x] = Block.id;
            //WriteAt(Block.Texture, x * 2, y);
            Console.ForegroundColor = default;
            Console.BackgroundColor = ConsoleColor.Cyan;
        }

        static void Fill_Index_Cord(int x1, int y1, int x2, int y2, int[,] grid, Solid Block)
        {

            for (int j = y1; j < y2; j++)
            {
                for (int i = x1; i < x2; i++)
                {
                    
                    grid[j, i] = Block.id;
                    


                }
            }
            
        }

        static void Fill_Index_Cord2(int x1, int y1, int x2, int y2, int[,] grid, Solid Block, int randomiser)
        {
            Random random = new Random();
            for (int j = y1; j < y2; j++)
            {
                for (int i = x1; i < x2; i++)
                {
                    int e = random.Next(0, randomiser);
                    if (e == 0)
                    {
                        continue;
                    }
                    Console.ForegroundColor = Block.FG;
                    Console.BackgroundColor = Block.BG;
                    grid[j, i] = Block.id;
                    WriteAt(Block.Texture, i * 2, j);


                }
            }
            Console.ForegroundColor = default;
            Console.BackgroundColor = ConsoleColor.Cyan;
        }

        private static void PrintUI(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteAt(player.health.ToString() + " Health", 3, 4);
            Console.ForegroundColor = default;
        }

        static void Print_window(Camera camera,Game game,Player player)
        {
            
            int c = 0;
            int UI = camera.View.GetLength(0) + 1;
            WriteAt("                                    ", 1, UI + 2);
            foreach (Solid i in game.Block_list)
            {
                Console.ForegroundColor = i.FG;
                Console.BackgroundColor = i.BG;
                WriteAt(i.Texture.ToString(),c*2,UI);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                WriteAt(i.quantity.ToString(), c*2, UI-1);
                c++;

                
            }
            WriteAt("^^", player.Selected_block.id * 2, UI + 1);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = default;

        }

    }
}