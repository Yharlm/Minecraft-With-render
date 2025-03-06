using Minecraft;

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

        static void Entity_behaviour(Game game, Player player, int[,] grid)
        {
            try
            {
                foreach (Entity mob in game.Existing_Entities)
                {
                    string behaviour = mob.Type;
                    switch (behaviour)
                    {
                        case "Pig":

                            //try { Walk_to_player(mob, player, grid, game); }
                            //catch { }
                            if (mob.Health <= 0) { Kill_entity(game, mob); }
                            Walk_to_player(mob, player, grid, game);
                            if (GetRadius_forplayer(player, mob.cordinates, 2, 2) && game.delay(mob.time, 2, game.curent_tick))
                            {
                                Explosion(game, grid, mob.cordinates, player, 2);
                                Kill_entity(game, mob);
                            }
                            if (grid[mob.cordinates.y + 1, mob.cordinates.x] != 0)
                            {
                                if (grid[mob.cordinates.y, mob.cordinates.x + 1] != 0 || grid[mob.cordinates.y, mob.cordinates.x - 1] != 0)
                                {

                                    mob.cordinates.y -= 2;
                                }
                            }
                            break;
                        case "Boss":
                            if (mob.Health <= 0) { Kill_entity(game, mob); }
                            Walk_to_player(mob, player, grid, game);
                            if (grid[mob.cordinates.y + 1, mob.cordinates.x] != 0)
                            {
                                if (grid[mob.cordinates.y, mob.cordinates.x + 1] != 0 || grid[mob.cordinates.y, mob.cordinates.x - 1] != 0)
                                {

                                    mob.cordinates.y -= 2;
                                }
                            }
                            if (GetRadius_forplayer(player, mob.cordinates, 2, 2) && game.curent_tick)
                            {
                                player.health -= 10;
                            }


                            break;
                        case "Projectile":

                            Cordinates pos = mob.cordinates;
                            if (game.curent_tick)
                            {

                                if (mob.Name == "Slash")
                                {
                                    if (pos.x <= pos.x1 + 8)
                                    {
                                        Break_block(pos.x, pos.y, grid, game.GetBlock("Air"), game);
                                        Break_block(pos.x - 1, pos.y - 1, grid, game.GetBlock("Air"), game);
                                        Break_block(pos.x - 1, pos.y + 1, grid, game.GetBlock("Air"), game);
                                    }
                                    if (pos.x >= pos.x1 + 14)
                                    {
                                        Break_block(pos.x + 1, pos.y, grid, game.GetBlock("Air"), game);
                                        Break_block(pos.x + 2, pos.y, grid, game.GetBlock("Air"), game);

                                    }
                                    if (pos.x >= pos.x1 + 20)
                                    {
                                        Kill_entity(game, mob);
                                    }
                                    grid[pos.y, pos.x] = 0;
                                    //Fill_Index_Cord(pos.x-2,pos.y-2,pos.x,pos.y,grid,player.GetBlock("air"));


                                    mob.cordinates.x += 1;

                                    Attack(game, mob.cordinates, grid, 1, 3, 10);
                                }

                                else if (mob.Name == "Arrow")
                                {
                                    int start = mob.cordinates.x1;
                                    int range = mob.specialvalue;
                                    //if (mob.cordinates.x % 2 == 0)
                                    //{
                                    //    mob.specialvalue += 1;
                                    //}
                                    if (grid[pos.y, pos.x] != 0)
                                    {
                                        Explosion(game, grid, pos, player, range);
                                        Kill_entity(game, mob);
                                    }

                                    mob.cordinates.x += 1;
                                }

                                else if (mob.Name == "Blue")
                                {

                                    Random random = new Random();
                                    int radius = mob.specialvalue + 6;
                                    int x = pos.x - radius / 2;
                                    int y = pos.y - radius / 2;
                                    int xq = 1;
                                    int yq = 1;



                                    int counter = 0;
                                    while (counter <= radius * radius)
                                    {

                                        int X = random.Next(0, radius);
                                        int Y = random.Next(0, radius);
                                        int i = 0;
                                        int j = 0;
                                        //if (X < Y) { i = Y / X; j = 1; }
                                        //else { j = X / Y; i = 1; }

                                        if (X > radius / 2)
                                        {
                                            xq *= -1;
                                        }
                                        if (Y > radius / 2)
                                        {
                                            yq *= -1;
                                        }
                                        if (grid[y + Y, x + X] != 0)
                                        {
                                            grid[y + Y + yq * 1, x + X + xq * 1] = grid[y + Y, x + X];
                                            grid[y + Y, x + X] = 0;
                                        }
                                        counter++;
                                    }
                                    for (int i = 0; i < 3; i++)
                                    {
                                        for (int j = 0; j < 3; j++)
                                        {
                                            grid[y + i - 1, x + j - 1] = 0;
                                        }
                                    }

                                    //if (mob.delay(3, game.curent_tick))
                                    //{
                                    //    mob.specialvalue++;
                                    //}
                                    //if(mob.time %2  == 0) { pos.x++; }
                                    int range = 5;
                                    if (mob.delay(3, game.curent_tick))

                                    {
                                        if (player.Looking == "Right")
                                        {
                                            if (player.x + range > pos.x)
                                            {
                                                mob.cordinates.x += 1;
                                            }
                                        }
                                        if (player.Looking == "Left")
                                        {
                                            if (player.x - range < pos.x)
                                            {
                                                mob.cordinates.x -= 1;
                                            }

                                        }

                                    }
                                    foreach (Entity entity in game.Existing_Entities)
                                    {

                                        if (GetRadius(entity, pos, 3, 3) && entity.Type != "Projectile")
                                        {
                                            entity.cordinates.x = pos.x;
                                            entity.cordinates.y = pos.y + -1;

                                        }
                                    }


                                }
                                else if (mob.Name == "Red")
                                {
                                    foreach (Entity entity in game.Existing_Entities)
                                    {
                                        if (GetRadius(entity, pos, 3, 3) && entity.Name == "Blue")
                                        {
                                            Explosion(game, grid, pos, player, 22);

                                            Kill_entity(game, mob);
                                            Kill_entity(game, entity);
                                        }
                                    }
                                    if (grid[pos.y, pos.x] != 0 && grid[pos.y, pos.x] != 18 && grid[pos.y, pos.x] != 17)
                                    {
                                        Explosion(game, grid, pos, player, 5);
                                        game.Displayed_sprites.Add(game.Sprite_list[0]);
                                        Kill_entity(game, mob);

                                    }
                                    int range = 15;
                                    if (mob.delay(1, game.curent_tick))

                                    {
                                        if (player.Looking == "Right")
                                        {
                                            if (player.x + range > pos.x)
                                            {
                                                mob.cordinates.x += 1;
                                            }
                                        }
                                        if (player.Looking == "Left")
                                        {
                                            if (player.x - range < pos.x)
                                            {
                                                mob.cordinates.x -= 1;
                                            }

                                        }

                                    }

                                }
                            }
                            break;
                    }
                }
            }
            catch { }
        }

        static async Task Cordinates(Cordinates Cords)
        {
            var db = new Database();
            var cordinates = new Cordinates { x = Cords.x, y = Cords.y };
            bool isSaved = await db.SaveCordinates(cordinates);
        }

        static async Task Main(string[] args)
        {
            //var database = new Database();

            //// Call the async method and await its result
            //var cordinates = await database.GetCordinates();

            //// Use the result (for example, print the coordinates)
            //foreach (var cordinate in cordinates)
            //{
            //    Console.WriteLine($"X: {cordinate.x}, Y: {cordinate.y}");
            //}

            Console.ReadLine();
            //Thread.Sleep(80000);
            //Save.Test();
            Console.CursorVisible = false;
            Game Game = new Game();
            Player player = new Player();


            Entity Mob = new Entity(null, 0, null, "EE"); Game.Entity_list.Add(Mob);

            Mob = new Entity("pig", 10, "Pig", "██"); Game.Entity_list.Add(Mob);
            Mob.Color = ConsoleColor.Blue;

            Mob = new Entity("TNT", 0, null, "██"); Game.Entity_list.Add(Mob);
            Mob.Color = ConsoleColor.Blue;
            Mob = new Entity("Boss", 30, "E", "██");

            Mob.Color = ConsoleColor.Blue;
            Game.Entity_list.Add(Mob);

            Update_Textures(Game);


            // recepies
            Recipe recipe = new Recipe();
            Non_Existent placehold = new Non_Existent(4, "Log", 1);

            recipe = new Recipe(); recipe.item = Game.GetBlock("Crafting_table"); placehold = new Non_Existent(4, "Log", 1); recipe.num = 1; recipe.required.Add(placehold); Game.recipes.Add(recipe);
            recipe = new Recipe(); recipe.item = Game.GetBlock("Wooden_planks"); placehold = new Non_Existent(4, "Log", 1); recipe.num = 4; recipe.required.Add(placehold); Game.recipes.Add(recipe);
            recipe = new Recipe(); recipe.item = Game.GetBlock("Ladder"); placehold = new Non_Existent(11, "Wooden_planks", 2); recipe.num = 4; recipe.required.Add(placehold); Game.recipes.Add(recipe);
            recipe = new Recipe(); recipe.item = Game.GetBlock("Furnace"); placehold = new Non_Existent(14, "Stone", 8); recipe.num = 1; recipe.required.Add(placehold); Game.recipes.Add(recipe);
            recipe = new Recipe(); recipe.item = Game.GetBlock("Tree"); placehold = new Non_Existent(15, "Leaves", 1); recipe.num = 2; recipe.required.Add(placehold); Game.recipes.Add(recipe);
            recipe = new Recipe(); recipe.item = Game.GetBlock("Wood_Background"); placehold = new Non_Existent(15, "Wooden_planks", 1); recipe.num = 2; recipe.required.Add(placehold); Game.recipes.Add(recipe);
            recipe = new Recipe(); recipe.item = Game.GetBlock("Torch"); placehold = new Non_Existent(15, "Wooden_planks", 1); recipe.num = 4; recipe.required.Add(placehold); placehold = new Non_Existent(8, "Coal_ore", 1); recipe.required.Add(placehold); Game.recipes.Add(recipe);
            recipe = new Recipe(); recipe.item = Game.GetBlock("Wooden_pickaxe"); placehold = new Non_Existent(15, "Wooden_planks", 5); recipe.num = 1; recipe.required.Add(placehold); Game.recipes.Add(recipe);



            //Projectiles
            Mob = new Entity("Blue", 0, "Projectile", "██");
            Mob.Color = ConsoleColor.DarkBlue;
            Game.Projectiles.Add(Mob);
            Mob = new Entity("Red", 0, "Projectile", "██");
            Mob.Color = ConsoleColor.DarkRed;
            Game.Projectiles.Add(Mob);
            Mob = new Entity("Slash", 0, "Projectile", "--");
            Mob.Color = ConsoleColor.White;
            Game.Projectiles.Add(Mob);
            Mob = new Entity("Arrow", 0, "Projectile", "  ");

            Mob.specialvalue = 5;
            Mob.BGColor = ConsoleColor.Yellow;
            Game.Projectiles.Add(Mob);


            // Sprites ▄█▀

            Sprites sprite = new Sprites();
            sprite.sprite = new string[3];
            sprite.sprite[0] = "  WW  ";
            sprite.sprite[1] = "WWWWWW";
            sprite.sprite[2] = "  WW  ";
            sprite.lifetime = 2;

            Game.Sprite_list.Add(sprite);
            bool selected = false;
            var option = 0;
            int[] backpack = new int[Game.Block_list.Count()];
            bool saved = true;
            int[,] grid = new int[500, 1000];
            int selected_button = 0;
            
            while (option != null && !selected)
            {
                Console.Title = "Minecraft";
                if (selected_button > 3)
                {
                    selected_button = 1;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Clear();
                int logo_x = 45;
                int logo_y = 11;
                WriteAt("████         ██ ████ ████      ████ ██████████ ████████ █████████  ██████████ █████████ ████████████", logo_x, logo_y++);
                WriteAt("█████     █████ ████ ████████  ████ ████       ████████ ████ █████ ██  ██  ██ ████          ████    ", logo_x, logo_y++);
                WriteAt("███████████████ ████ ████████  ████ ████▄▄▄▄▄▄ ██       ████████▀▀ ███▀  ▀███ ████▄▄▄▄▄     ████    ", logo_x, logo_y++);
                WriteAt("████  ███  ████ ████ ████  ████████ ████▀▀▀▀▀▀ ██       █████████  ███▄██▄███ █████████     ████    ", logo_x, logo_y++);
                WriteAt("████       ████ ████ ████  ████████ ████       ████████ ████  ████ ████▀▀████ ████          ████    ", logo_x, logo_y++);
                WriteAt("████       ████ ████ ████    ██████ ██████████ ████████ ████  ████ ████  ████ ████          ████    ", logo_x, logo_y++);




                var not_selected_color = ConsoleColor.Gray;
                var selected_color = ConsoleColor.Blue;
                
                

                //Console.WriteLine("Welcome to bootleg-craft");
                int counter2 = 12;

                int buttons_x = 86;
                int buttons_y = 30;
                Console.BackgroundColor = ConsoleColor.Gray;
                if (selected_button == 1) { Console.BackgroundColor = selected_color; }
                WriteAt("    Create World   ", buttons_x, buttons_y);
                Console.BackgroundColor = ConsoleColor.Cyan;

                Console.BackgroundColor = ConsoleColor.Gray;
                if (selected_button == 2) { Console.BackgroundColor = selected_color; }
                WriteAt("     Load World    ", buttons_x, buttons_y+3);
                Console.BackgroundColor = ConsoleColor.Cyan;

                Console.BackgroundColor = ConsoleColor.Gray;
                if (selected_button == 3) { Console.BackgroundColor = selected_color; }
                WriteAt("       Editor      ", buttons_x, buttons_y+6);
                Console.BackgroundColor = ConsoleColor.Cyan;

                //option = Console.ReadLine();
                var inputed = Console.ReadKey().Key;
                if (inputed == ConsoleKey.Spacebar)
                {
                    selected_button++;
                }
                else if (inputed == ConsoleKey.Enter)
                {
                    option = selected_button;
                }

                








                    switch (option)
                {
                    case 1:
                        selected = true;
                        Console.WriteLine("Generating...");
                        BuildWorld(grid, player, Game);


                        break;
                    case 2:
                        selected = true;

                        Console.WriteLine("Loading...");
                        grid = Save.LoadWorld(Files.GetSaveFilePath("SaveFile"), "World.json");
                        backpack = Save.Load_inventory(Files.GetSaveFilePath("SaveFile"), "Backpack.json");
                        break;
                    case 3:
                        selected = true;

                        Console.WriteLine("Loading...");
                        Editor.Editor_mode();
                        option = 0;

                        break;
                    case 4:
                        selected = true;

                        grid[50, 500] = 2;
                        grid[49, 500] = Game.GetBlock("Tree").id;
                        break;


                }

            }
            Console.BackgroundColor= ConsoleColor.Black;
            Console.Clear();






            Camera camera = new Camera();
            //Co-pilot carried me through file creation and reading the file lol





            player.Spawnpoint = Convert_cor(500, 20);
            player.x = player.Spawnpoint.x;
            player.y = player.Spawnpoint.y;
            player.Selected_block = Game.Get_ByID(0);
            int counter = 0;
            while (grid[player.y + counter, player.x] == 0)
            {
                counter++;
            }
            if (grid[player.y + counter, player.x] == 5) { Fill_block(player.x, player.y + counter, grid, Game.GetBlock("Wooden_planks")); }
            if (saved)
            {
                for (int i = 0; i < backpack.Length; i++)
                {
                    Game.Block_list[i].quantity = backpack[i];
                }
            }
            if (player.creative)
            {
                foreach (Solid item in Game.Block_list)
                {
                    item.quantity = 99;
                }
            }

            double block_tick = 0;
            double tick = 0.001;
            double time = 0;
            while (true)

            {



                GetInput(grid, player, Game, camera);
                if (player.Selected_block.Category == "Item")
                {
                    Game.player_pic_lv = player.Selected_block.level;
                }
                else
                {
                    Game.player_pic_lv = 1;
                }
                //double timer = Math.Ceiling(overworld.time += 0.0002);
                double timer = Game.time += 0.002;
                if (Game.time >= tick)
                {

                    Game.curent_tick = true;
                    Game.time = 0;
                    time += 1;

                }
                else
                {

                    Game.curent_tick = false;
                }
                camera.Position.x = player.x - camera.View.GetLength(1) / 2;
                camera.Position.y = player.y - camera.View.GetLength(0) / 2;
                Entity_update(grid, Game.Existing_Entities, Game, player);
                if (player.y > 80)
                {
                    foreach (Solid block in Game.Block_list)
                    {
                        if (block.BG == Game.Background)
                        {
                            block.BG = ConsoleColor.Gray;
                        }
                    }

                }
                else
                {
                    foreach (Solid block in Game.Block_list)
                    {
                        if (block.BG == Game.Background || block.BG == ConsoleColor.DarkGray)
                        {
                            block.BG = ConsoleColor.Cyan;
                        }
                    }

                }


                Game.cycle += 0.1f;
                if (Game.cycle >= 120)
                { Game.day = false; }
                else
                { Game.day = true; }
                if (Game.cycle == 240)
                { Game.cycle = 0; }


                for (int i = 0; i < camera.View.GetLength(0); i++)
                {
                    for (int j = 0; j < camera.View.GetLength(1); j++)
                    {
                        //if (i <= size - 1 && j == size - 1 || i == 0 && j == 0 || i == size - 1 && j == 0 || i == 0 && j == size - 1)
                        //{ continue; }
                        Render_block(Game.Get_ByID(grid[i + camera.Position.y, j + camera.Position.x]), j, i, Game, camera, player, grid);



                        //Fill_block(player.x, player.y, camera.View, Game.GetBlock("Stone"));
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        //WriteAt("EE",player.x*2,player.y);

                        if (!Check_area(grid, player.x, player.y, 16, 6) && !Game.day)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;

                        }

                        WriteAt("..", camera.View.GetLength(1) - 1, (camera.View.GetLength(0) / 2) - 1);
                        WriteAt("  ", camera.View.GetLength(1) - 1, camera.View.GetLength(0) / 2);
                        Console.ForegroundColor = default;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    try
                    {
                        foreach (Sprites Sprites in Game.Displayed_sprites)
                        {
                            int x = Sprites.pos.x - camera.Position.x;
                            int y = Sprites.pos.y - camera.Position.y;


                            if (Sprites.pos.x >= camera.Position.x && Sprites.pos.x <= camera.Position.x + camera.View.GetLength(1) &&
                                Sprites.pos.y >= camera.Position.y && Sprites.pos.y <= camera.Position.y + camera.View.GetLength(0))
                            {
                                for (int a = 0; a < Sprites.sprite.Length; a++)
                                {
                                    for (int b = 0; b < Sprites.sprite[a].Length; b++)
                                    {
                                        char c = sprite.sprite[a][b];
                                        if (c != ' ')
                                        {
                                            switch (c)
                                            {
                                                case 'R':
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    break;
                                                case 'W':
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    break;
                                            }
                                            WriteAt("█", x * 2 + b, y + a);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                Game.Displayed_sprites.Remove(Sprites);
                            }
                            Sprites.despawn(Game);
                        }
                    }
                    catch { }
                }
                if (time % 2 == 0)
                {
                    for (int i = 0; i < camera.View.GetLength(0) * 2; i++)
                    {
                        for (int j = 0; j < camera.View.GetLength(1) * 2; j++)
                        {

                            Block_Update(camera, j + camera.Position.x, i + camera.Position.y, grid, Game, time);
                        }
                    }
                }
                if (player.oxygen <= 0 && Game.curent_tick) { player.health -= 1; }
                if (grid[player.y - 1, player.x] == 5)
                {
                    player.is_swiming = true;
                    if (Game.curent_tick)
                    {
                        if (player.oxygen >= 0) player.oxygen -= 0.5;
                        Print_window(camera, Game, player);
                    }
                }
                else
                {
                    player.is_swiming = false;
                }

                if (Game.Get_ByID(grid[player.y + 1, player.x]).Collidable == false && Game.curent_tick && player.grounded)
                {

                    if (player.is_swiming)
                    {
                        if (time % 2 == 0) { player.y++; }

                    }
                    else
                    {
                        player.y++;
                        if (player.oxygen < 20 && Game.curent_tick) { player.oxygen += 0.5; }
                    }

                    //Thread.Sleep(100);


                }
                if (grid[player.y, player.x] == 12)
                {
                    player.grounded = false;
                }

                else
                {
                    player.grounded = true;
                }

                if (player.health <= 0)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteAt("YOU ARE DEAD", 20, 11);
                    Thread.Sleep(2000);
                    WriteAt("[idiot]", 22, 12);
                    Thread.Sleep(500);
                    WriteAt("[idiot].", 22, 12);
                    Thread.Sleep(500);
                    WriteAt("[idiot]..", 22, 12);
                    Thread.Sleep(500);
                    WriteAt("[idiot]...", 22, 12);

                    Console.ForegroundColor = default;

                    Thread.Sleep(2000);
                    player.health = 100;

                    player.x = player.Spawnpoint.x;
                    player.y = player.Spawnpoint.y;

                }



            }

        }

        private static void Update_Textures(Game Game)
        {
            int id = 0;
            Solid Default = new Solid("null", 0, null, default, default);
            Non_solid Background = new Non_solid("", 0, null, default, default);

            Default = new Solid("Air", id++, "  ", ConsoleColor.DarkGray, Game.Background); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Grass", id++, "▀▀", ConsoleColor.DarkGreen, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Dirt", id++, "██", ConsoleColor.DarkYellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);
            Default = new Solid("Stone", id++, "██", ConsoleColor.DarkGray, ConsoleColor.DarkGray); Default.level = 2; Game.Block_list.Add(Default);
            Default = new Solid("Log", id++, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Game.Block_list.Add(Default);

            Default = new Solid("water", id++, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Default.Collidable = false; Game.Block_list.Add(Default);
            //Default = new Solid("water", 5, "  ", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue); Game.Block_list.Add(Default);

            Default = new Solid("waterTop", id++, "▄▄", ConsoleColor.DarkBlue, ConsoleColor.Cyan); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Leaves", id++, "▄▀", ConsoleColor.DarkGreen, ConsoleColor.Green); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Coal_ore", id++, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Black); Default.level = 2; Game.Block_list.Add(Default);
            Default = new Solid("Iron_ore", id++, "▄▀", ConsoleColor.DarkGray, ConsoleColor.Yellow); Default.level = 2; Game.Block_list.Add(Default);
            Default = new Solid("Crafting_table", id++, "TT", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Default.Collidable = true; Game.Block_list.Add(Default);
            Default = new Solid("Wooden_planks", id++, "--", ConsoleColor.DarkYellow, ConsoleColor.DarkMagenta); Game.Block_list.Add(Default);
            Default = new Solid("Ladder", id++, "||", ConsoleColor.Yellow, ConsoleColor.DarkYellow); Default.Collidable = false; Default.climbable = true; Game.Block_list.Add(Default);
            Default = new Solid("Sand", id++, "██", ConsoleColor.DarkMagenta, ConsoleColor.Cyan); Game.Block_list.Add(Default);
            Default = new Solid("Furnace", id++, "▀▀", ConsoleColor.DarkGray, ConsoleColor.Black); Default.Collidable = false; Game.Block_list.Add(Default);//15
            Default = new Solid("Tree", id++, "▀▀", ConsoleColor.Green, ConsoleColor.DarkYellow); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Torch", id++, "▄▄", ConsoleColor.DarkYellow, ConsoleColor.Magenta); Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Cave_Background", id++, "  ", ConsoleColor.DarkYellow, ConsoleColor.Gray); Default.level = 3; Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Wood_Background", id++, "==", ConsoleColor.DarkYellow, ConsoleColor.Magenta); Default.level = 1; Default.Collidable = false; Game.Block_list.Add(Default);
            Default = new Solid("Wooden_pickaxe", id++, "T ", ConsoleColor.DarkYellow, Game.Background); Default.level = 3; Default.Collidable = false; Default.Category = "Item"; Game.Block_list.Add(Default);
            Default = new Solid("Iron_ingot", id++, "▄▄", ConsoleColor.DarkGray, Game.Background); Default.Collidable = false; Default.Category = "Item"; Game.Block_list.Add(Default);
            Default = new Solid("Furnace_active", id++, "▀▀", ConsoleColor.DarkGray, ConsoleColor.Magenta); Default.level = 3; Default.Collidable = false; Game.Block_list.Add(Default); // 21

        }

        static void Block_Update(Camera camera, int x, int y, int[,] grid, Game game, double time)
        {
            Structure tree = new Structure();
            tree.Struct = new int[,]{
                { 0,7,7,7,0 },
                { 0,7,4,7,0 },
                { 7,7,4,7,7 },
                { 7,7,4,7,7 },
                { 0,0,4,0,0 },
                { 0,0,4,0,0 }
            };
            Random random = new Random();


            switch (grid[y, x])
            {
                case 5:

                    {
                        if (grid[y + 1, x] == 0 || grid[y + 1, x] == 18)
                        {
                            grid[y, x] = 0;
                            grid[y + 1, x] = 6;

                        }
                        else if (grid[y, x + 1] == 0 || grid[y, x + 1] == 18)
                        {
                            grid[y, x] = 0;
                            grid[y, x + 1] = 6;

                        }
                        else if (grid[y, x - 1] == 0 || grid[y, x - 1] == 18)
                        {
                            grid[y, x] = 0;
                            grid[y, x - 1] = 6;
                        }
                    }

                    break;
                case 6:
                    if (grid[y, x] == 6)
                    {
                        grid[y, x] = 5;
                    }

                    break;
                case 7:
                    if (time % 132 == 0)
                    {
                        if (random.Next(1, 30) < 2)
                        {
                            grid[y, x] = 0;
                        }
                    }
                    break;
                case 13:
                    if (time % 2 == 0 && grid[y + 1, x] == 0 || grid[y + 1, x] == 18 && time % 2 == 18)
                    {
                        grid[y, x] = 0;
                        grid[y + 1, x] = -13;
                    }
                    break;
                case -13:
                    grid[y, x] = 13;
                    break;

                case 15:
                    if (random.Next(1, 100) < 10 && time % 4 == 0)
                    {
                        structure(tree, x - 2, y + 1, grid, game);
                    }
                    break;

                case 14:
                    if (grid[y - 1, x] == game.GetBlock("Coal_ore").id)
                    {
                        grid[y - 1, x] = 0;
                        grid[y, x] = 21;
                    }
                    break;
                case 21:
                    if (time % 11 == 0 && grid[y - 1, x] == game.GetBlock("Iron_ore").id)
                    {
                        grid[y - 1, x] = game.GetBlock("Iron_ingot").id;

                    }
                    if (time % 500 == 0)
                    {
                        grid[y, x] = game.GetBlock("Furnace").id;
                    }
                    break;
                case 2:
                    if (time % 15 == 0 && grid[y - 1, x] == 0)
                    {
                        grid[y, x] = 1;
                    }
                    break;
                case 1:
                    if (time % 15 == 0 && grid[y - 1, x] != 0)
                    {
                        grid[y, x] = 2;
                    }
                    break;

            }




        }
        static void Entity_update(int[,] grid, List<Entity> Entity_list, Game game, Player player)
        {



            //Entity_behaviour(game, player, grid);
            Entity_behaviour(game, player, grid);
            //PlayerAbilities(grid, player, game);
            if (game.Existing_Entities.Count != 0)
            {
                foreach (Entity entity in game.Existing_Entities)
                {
                    if (game.curent_tick)
                    {
                        entity.gravity(grid);
                    }

                    //try { Walk_to_player(entity, player, grid, game); }
                    //catch { }
                }

            }
        }

        static void Walk_to_player(Entity entity, Player player, int[,] grid, Game game)
        {

            int speed = 3;

            if (player.x < entity.cordinates.x)
            {

                if (grid[entity.cordinates.y, entity.cordinates.x - 1] == 0 && entity.delay(speed, game.curent_tick))
                {

                    entity.cordinates.x--;
                }



            }
            else if (player.x > entity.cordinates.x)
            {

                if (grid[entity.cordinates.y, entity.cordinates.x + 1] == 0 && entity.delay(speed, game.curent_tick))
                {

                    entity.cordinates.x++;
                }

            }


        }
        private static void Kill_entity(Game game, Entity entity)
        {

            game.Existing_Entities.Remove(entity);
        }



        private static void GetInput(int[,] grid, Player player, Game game, Camera camera)
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



            //grid[player.y, player.x] = 0;
            int x = player.x;
            int y = player.y;

            if (Console.KeyAvailable == true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                WriteAt(" ", 0, 0);

                player.Input = Console.ReadKey().Key.ToString();
                if (player.Input != "Spacebar" && player.Input != "L" && player.Input != "K" && player.Input != "R")
                {
                    player.last_key = player.Input;

                }
                if (player.Input == "Spacebar")
                {
                    player.special_key = "Spacebar";
                }
                if (player.Input == "D")
                {
                    player.Looking = "Right";
                }
                if (player.Input == "A")
                {
                    player.Looking = "Left";
                }


            }

            else { player.Input = null; }

            Cordinates player_cords = new Cordinates();
            player_cords.x = x;
            player_cords.y = y;
            //player.Selected_block = dirt;
            if (game.curent_tick)
            {
                switch (player.Input)
                {
                    case "U":
                        Caves(x, y + 3, grid);
                        break;
                    case "I":
                        int blocks_num = game.Block_list.Count();
                        int[] invent = new int[blocks_num];
                        for (int i = 0; i < blocks_num; i++)
                        {
                            invent[i] = game.Block_list[i].quantity;
                        }



                        Cordinates(player_cords);
                        Files.Save_map(grid);
                        Files.Save_Inventory(invent);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                        WriteAt("Saved", 60, 4);

                        Console.ForegroundColor = default;
                        Thread.Sleep(1000);
                        System.Environment.Exit(0);
                        break;
                    case "Q":
                        Fill_block(x, y - 5, grid, game.GetBlock("water"));
                        break;
                    case "E":
                        player.Crafting_select++;
                        if (player.Crafting_select == game.recipes.Count)
                        {
                            player.Crafting_select = 0;
                        }

                        break;
                    case "X":

                        //player.Holding = true;
                        Shoot_Projectile(player, game, player_cords, player.Entity_hotbar);
                        break;
                    case "D1":
                        player.hotbar--;



                        Console.ForegroundColor = ConsoleColor.Red;


                        if (player.hotbar == 0) player.hotbar = game.Block_list.Count; player.Selected_block = game.Block_list[player.hotbar + player.hotbar_offset];


                        break;

                    case "Z":
                        player.Entity_hotbar++;
                        if (player.Entity_hotbar == 2)
                        {
                            player.Entity_hotbar = 0;
                        }
                        break;
                    case "N":

                        try
                        {

                            foreach (Entity entity in game.Existing_Entities)
                            {




                                game.Existing_Entities.Remove(entity);
                                Explosion(game, grid, entity.cordinates, player, 4);

                            }
                        }
                        catch { }

                        break;
                    case "C":
                        if (player.Crafting_select == 0 || grid[player.y + 1, player.x] == game.GetBlock("Crafting_table").id)
                        {
                            Craft(game.recipes[player.Crafting_select], game);
                        }





                        break;
                    case "Y":

                        game.Existing_Entities.Clear();

                        break;
                    case "T":
                        {

                            //WriteAt(game.Existing_Entities.Count().ToString(), 24, 3);

                            Entity mob = game.Entity_list[1];
                            Entity Default = new Entity(mob.Name, mob.Health, mob.Type, mob.Sprite);
                            Default.Color = mob.Color;
                            //Default.cordinates.x = random.Next(4, 55);
                            Default.cordinates.x = player.x + 13;
                            Default.cordinates.y = player.y - 4;



                            game.Existing_Entities.Add(Default);
                            //game.Spawn_entity(entity);

                            break;
                        }
                    case "D2":
                        player.hotbar++;





                        Console.ForegroundColor = ConsoleColor.Red;

                        if (player.hotbar == 10) player.hotbar = 0;
                        player.Selected_block = game.Block_list[player.hotbar+player.hotbar_offset];



                        break;
                    case "D3":
                        player.hotbar_offset++;
                        if(player.hotbar_offset == game.Block_list.Count-10)
                        {
                            player.hotbar_offset = 0;
                        }
                        
                        break;
                    case "K":

                        {

                            if (player.special_key == "Spacebar")
                            {
                                if (player.special_key == "Spacebar")
                                {
                                    if (grid[player.y - 2, player.x] == 0)
                                    {
                                        Break_block(player.x, player.y - 3, grid, air, game); player.special_key = null;
                                    }
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



                        break;
                    case "L":
                        if (player.Selected_block == game.GetBlock("Ladder") && player.Selected_block.quantity > 0)
                        {
                            Fill_block(player.x, player.y, grid, player.Selected_block);
                            player.last_key = null;
                            player.Selected_block.quantity--;
                            break;
                        }

                        if (player.Selected_block.quantity > 0)
                        {
                            if (grid[player.y + 1, player.x] != 0 && grid[player.y + 1, player.x + 1] == 0)
                            {

                                Fill_block(player.x + 1, player.y + 1, grid, player.Selected_block);
                                player.Selected_block.quantity--;
                            }
                            else if (player.special_key == "Spacebar")
                            {
                                Fill_block(player.x, player.y, grid, player.Selected_block);
                                player.Selected_block.quantity--;
                            }
                            else if (grid[player.y - 1, player.x] == 0 && grid[player.y - 2, player.x] != 0)
                            {
                                Fill_block(player.x, player.y + 1, grid, player.Selected_block);
                                player.last_key = null;
                                player.Selected_block.quantity--;
                            }
                            else if (player.last_key == "S")
                            {
                                Fill_block(player.x, player.y + 1, grid, player.Selected_block); player.y--; ; player.Selected_block.quantity--;
                            }
                            else if (player.last_key == "D")
                            {
                                if (grid[player.y + 1, player.x + 1] == 0)
                                    Fill_block(player.x + 1, player.y + 1, grid, player.Selected_block);
                                else if (grid[player.y, player.x + 1] == 0 && grid[player.y + 1, player.x + 1] != 0)
                                    Fill_block(player.x + 1, player.y, grid, player.Selected_block);
                                else if (grid[player.y - 1, player.x + 1] == 0 && grid[player.y, player.x + 1] != 0)
                                    Fill_block(player.x + 1, player.y - 1, grid, player.Selected_block);
                                player.Selected_block.quantity--;
                            }
                            else if (player.last_key == "A")
                            {
                                if (grid[player.y + 1, player.x - 1] == 0)
                                    Fill_block(player.x - 1, player.y + 1, grid, player.Selected_block);
                                else if (grid[player.y, player.x - 1] == 0 && grid[player.y + 1, player.x - 1] != 0)
                                    Fill_block(player.x - 1, player.y, grid, player.Selected_block);
                                else if (grid[player.y - 1, player.x - 1] == 0 && grid[player.y, player.x - 1] != 0)
                                    Fill_block(player.x - 1, player.y - 1, grid, player.Selected_block);
                                player.Selected_block.quantity--;
                            }

                        }
                        break;
                    case "Spacebar":
                        if (grid[player.y + 1, player.x] != 0 && grid[player.y + 1, player.x] != 6 && grid[player.y + 1, player.x] != 5)
                        {

                            if (game.Get_ByID(grid[player.y - 2, player.x]).Collidable == false)
                            {
                                y -= 1;
                            }
                            if (game.Get_ByID(grid[player.y - 3, player.x]).Collidable == false && game.Get_ByID(grid[player.y - 2, player.x]).Collidable == false)
                            {
                                y -= 1;
                            }
                            if (game.Get_ByID(grid[player.y - 3, player.x]).Collidable == false && game.Get_ByID(grid[player.y - 2, player.x]).Collidable == false && game.Get_ByID(grid[player.y - 4, player.x]).Collidable == false)
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
                        //   game.Get_ByID().Collidable == false
                        if (game.Get_ByID(grid[player.y, player.x - 1]).Collidable == false && game.Get_ByID(grid[player.y - 1, player.x - 1]).Collidable == false)
                        {

                            x--;
                        }
                        else if (game.Get_ByID(grid[player.y - 1, player.x - 1]).Collidable == false && game.Get_ByID(grid[player.y - 2, player.x - 1]).Collidable == false && player.special_key == "Spacebar")
                        {

                            x--; y--;
                            player.special_key = null;

                        }


                        break;
                    case "W":
                        if (game.Get_ByID(grid[player.y, player.x]).climbable)
                        {
                            y--;
                        }
                        break;
                    case "S":
                        if (grid[player.y, player.x] == 12 && game.Get_ByID(grid[player.y + 1, player.x]).Collidable == false)
                        {
                            y++;

                            break;
                        }
                        if (game.Get_ByID(grid[player.y + 1, player.x]).Collidable == false)
                        {

                            y++;
                        }
                        break;
                    case "D":
                        camera.Position.x++;

                        if (player.grounded == false)
                        {


                        }
                        else if (game.Get_ByID(grid[player.y, player.x + 1]).Collidable == false && game.Get_ByID(grid[player.y - 1, player.x + 1]).Collidable == false)
                        {

                            x++;
                        }
                        else if (game.Get_ByID(grid[player.y - 1, player.x + 1]).Collidable == false && game.Get_ByID(grid[player.y - 2, player.x + 1]).Collidable == false && player.special_key == "Spacebar")
                        {

                            x++; y--;
                            player.special_key = null;
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
                Print_window(camera, game, player);
                player.Input = null;
                player.x = x;
                player.y = y;



            }
        }

        private static void spawnSprite(int v, Game game, Cordinates pos)
        {
            Sprites temp = game.Sprite_list[0];
            Sprites test = new Sprites();
            test.sprite = temp.sprite;

            test.pos = pos;
            game.Displayed_sprites.Add(test);

        }

        static void Break_block(int x, int y, int[,] grid, Solid Block, Game game)
        {

            if (game.player_pic_lv >= game.Get_ByID(grid[y, x]).level)
            {
                game.Block_list.Find(i => i.id == grid[y, x]).quantity++;
                grid[y, x] = Block.id;
            }


        }



        static void Render_block(Solid block, int x, int y, Game game, Camera camera, Player player, int[,] grid)
        {
            bool front = true;
            int vision = 0;
            if (player.x + vision >= x + camera.Position.x && player.y + vision >= y + camera.Position.y && player.x - vision <= x + camera.Position.x && player.y - vision <= y + camera.Position.y)
            {

                Console.ForegroundColor = block.FG;
                Console.BackgroundColor = block.BG;
            }
            else if (grid[y + camera.Position.y, x + camera.Position.x] != 0)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (grid[y + camera.Position.y, x + camera.Position.x] == 0)
            {

                Console.BackgroundColor = ConsoleColor.Gray;
            }
            if (grid[y + camera.Position.y, x + camera.Position.x] == 8)
            {

                Console.BackgroundColor = ConsoleColor.White;
            }
            if (Check_area(grid, camera.Position.x + x, camera.Position.y + y, 16, 5) || Check_area(grid, camera.Position.x + x, camera.Position.y + y, 16, game.GetBlock("Furnace_active").id))
            {
                Console.ForegroundColor = block.FG;
                Console.BackgroundColor = block.BG;
            }
            if (game.day)
            {
                Console.ForegroundColor = block.FG;
                Console.BackgroundColor = block.BG;
            }







            WriteAt(block.Texture, x * 2, y);

            Console.ForegroundColor = default;
            Console.BackgroundColor = default;
            foreach (Entity mob in game.Existing_Entities)
            {

                if (mob.cordinates.x >= x + camera.Position.x && mob.cordinates.x <= x + camera.Position.x && mob.cordinates.y >= y + camera.Position.y && mob.cordinates.y <= y + camera.Position.y)
                {
                    Console.ForegroundColor = mob.Color;
                    Console.BackgroundColor = mob.BGColor;
                    WriteAt(mob.Sprite, x * 2, y);
                }
            }
            //foreach (Sprites sprite in game.Displayed_sprites)
            //{

            //    if (sprite.pos.x >= x + camera.Position.x && sprite.pos.x <= x + camera.Position.x && sprite.pos.y >= y + camera.Position.y && sprite.pos.y <= y + camera.Position.y)
            //    {
            //        for (int i = 0; i < sprite.sprite.GetLength(0); i++)
            //        {
            //            for (int j = 0; j < sprite.sprite[i].Length; j++)
            //            {
            //                if (grid[sprite.pos.y + i, sprite.pos.x + j] == 0 && j % 2 != 0)
            //                {
            //                    var c = sprite.sprite[i];
            //                    WriteAt(c[j].ToString(), x * 2 + j, y + i);
            //                }

            //            }
            //        }
            //    }

            //}

        }


        static bool Check_area(int[,] grid, int x, int y, int id, int range)
        {
            bool result = false;
            //if(
            //    grid[y, x] == id ||
            //    grid[y+1, x] == id ||
            //    grid[y-1, x] == id ||
            //    grid[y, x+1] == id ||
            //    grid[y, x-1] == id

            //    )
            for (int i = 0; i < range; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    if (i + j == 0 || i + j == range * 2 - 2 || i == 0 && j == range - 1 || j == 0 && i == range - 1)
                    {
                        continue;
                    }
                    if (grid[y - i + range / 2, x - j + range / 2] == id)
                    {
                        result = true;
                    }
                }
            }


            return result;
        }
        static bool Check_area(int[,] grid, int x, int y, Cordinates pos, int range)
        {
            bool result = false;
            if
            (Convert_cor(x, y - 2) == pos)
            //    Console.BackgroundColor = ConsoleColor.Red;
            //WriteAt("Detected", 50, 20);
            {
                result = true;
            }



            return result;
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



            int tree_rate = 34;
            int Tree_r = 0;
            for (int i = 20; i < 700; i++)
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
            while (game.Get_ByID(grid[Local_y, Local_x]).Collidable == false)
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



                }
            }


        }
        static void structure(object struc, int Local_x, int Local_y, int[,] grid, Game game)
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



                }
            }


        }
        static void Generate_terrain(Game game, int[,] grid)
        {
            Random random = new Random();
            int Width = 1000;
            ; int Height = 45
            ;
            int dirt_Height = 5;
            int stone_Height = 302;
            int min_level = 60;
            int min = 1;
            int max = 3;
            int c = 0;
            int sea_level = 70;


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
                //Caves(j, random.Next(sea_level, 100), grid);
            }


            for (int j = 0; j < Width - 12;)
            {
                int coalN = random.Next(1, 40);
                int vein = random.Next(1, 6);

                if (random.Next(1, 30) < 4)
                {
                    Fill_Index_Cord2(j, Height + coalN - vein, j + vein, Height + coalN, grid, game.GetBlock("Coal_ore"), 5);
                }
                j++;
            }
            for (int j = 0; j < Width - 12;)
            {
                int ironN = random.Next(1, 40);
                int vein = random.Next(1, 6);
                if (random.Next(1, 30) < 4)
                {
                    Fill_Index_Cord2(j, Height + ironN - vein, j + vein, Height + ironN, grid, game.GetBlock("Iron_ore"), 5);
                }
                j++;
            }

            for (int j = 0; j < Width - 30; j++)
            {
                for (int i = sea_level - 10; i < sea_level + 50; i++)
                {
                    if (grid[i, j] == 0)
                    {
                        Fill_block(j, i, grid, game.GetBlock("water"));
                    }
                }
            }

            for (int j = 12; j < Width - 12; j++)
            {

                int count = random.Next(0, 11);
                while (count > 0)
                {
                    int y = random.Next(30, 460);
                    if (grid[y, j] != 0)
                    {

                        Caves(j, y, grid, game, game.GetBlock("Dirt"), false);
                        Caves(j, y, grid, game, game.GetBlock("Iron_ore"), false);
                        Caves(j, y, grid);
                    }
                    count--;
                }


            }


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

        public static void Caves(int x, int y, int[,] grid)
        {
            Random random = new Random();

            int size = random.Next(2, 5);


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == size - 1 && j == size - 1 || i == 0 && j == 0 || i == size - 1 && j == 0 || i == 0 && j == size - 1)
                    {
                        if (random.Next(0, 3) == 1 && grid[i + y, j + x] == 3)
                        {
                            Caves(x - size / 2 + i, y - size / 2 + j, grid);
                        }
                        continue;
                    }
                    grid[j + y, i + x] = 17;
                }
            }

        }

        public static void Caves(int x, int y, int[,] grid, Game game, Solid solid, bool replace)
        {
            Random random = new Random();

            int size = random.Next(2, 5);
            int id = -1;
            if (replace)
            {
                id = 0;
            }


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == size - 1 && j == size - 1 || i == 0 && j == 0 || i == size - 1 && j == 0 || i == 0 && j == size - 1)
                    {
                        if (random.Next(0, j) == 1 && grid[i + y, j + x] != id)
                        {
                            Caves(x - size / 2 + i, y - size / 2 + j, grid);
                        }
                        continue;
                    }
                    grid[j + y, i + x] = solid.id;
                }
            }

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
                    if (e == 0 || grid[j, i] == 0)
                    {
                        continue;
                    }

                    grid[j, i] = Block.id;
                }
            }

        }

        static void Fill_Index_Cord3(int x1, int y1, int x2, int y2, int[,] grid, Game game, Solid Block, int randomiser, int particles)
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
                    int p = random.Next(0, particles);
                    if (p <= particles && p > 4)
                    {
                        spawnSprite(0, game, Convert_cor(i, j));
                    }

                    grid[j, i] = Block.id;



                }
            }

        }
        static void Explosion(Game game, int[,] grid, Cordinates pos, Player player, int radius)
        {


            Solid air = new Solid("air", 0, "  ", ConsoleColor.White, ConsoleColor.Cyan);


            int range = radius;
            int range_max = range * 2;
            int x = pos.x;
            int y = pos.y;

            Cordinates cordinates = new Cordinates();
            cordinates.x = range_max;
            cordinates.y = range_max;
            cordinates.x1 = x + range_max + 1;
            cordinates.y1 = y + range_max + 1;

            Fill_Index_Cord3(x - range, y - range, x + range + 1, y + range + 1, grid, game, air, 30, 6);
            //Fill_Index_Cord3(x - range_max, y - range_max, x + range_max + 1, y + range_max + 1, grid,game, air, 2,3);
            Fill_Index_Cord2(x - range_max, y - range_max, x + range_max + 1, y + range_max + 1, grid, air, 2);
            Attack(game, pos, grid, 5, range, range_max);


            if (GetRadius_forplayer(pos, Convert_cor(player.x, player.y), range_max, range_max)) { player.health -= 50; }
            if (GetRadius_forplayer(pos, Convert_cor(player.x, player.y), range, range)) { player.health -= 50; }

        }
        private static Cordinates Convert_cor(int x, int y)
        {
            Cordinates cords = new Cordinates();
            cords.y = y;
            cords.x = x;
            return cords;
        }
        static bool GetRadius(Entity mob1, Cordinates plr, int x, int y)
        {
            bool res = false;
            if (mob1.cordinates.x > plr.x - x && mob1.cordinates.x < plr.x + x)
            {
                res = true;
            }
            //if (mob1.cordinates.y > plr.y - y && mob1.cordinates.y < plr.y + y)
            //{
            //    res = true;
            //}

            return res;
        }
        static bool GetRadius_forplayer(Cordinates object1, Cordinates object2, int x, int y)
        {
            bool res = false;
            if (object1.x >= object2.x - x && object1.x <= object2.x + x && object1.y >= object2.y - y && object1.y <= object2.y + y)
            {
                res = true;
            }


            return res;
        }

        private static bool Attack(Game game, Cordinates player, int[,] grid, int knockback, int range, int dmg)
        {
            bool is_there = false;
            foreach (Entity entity in game.Existing_Entities)
            {
                if (entity.Type != "Projectle")
                {
                    if (GetRadius(entity, player, range, 3) && entity.cordinates.x > player.x)
                    {
                        WriteAt("  ", entity.cordinates.x * 2, entity.cordinates.y);
                        entity.Health -= dmg;
                        if (grid[entity.cordinates.y, entity.cordinates.x + range] == 0) { entity.cordinates.x += knockback; }
                        is_there = true;
                        entity.cordinates.y -= knockback;
                        entity.velocity += 1;
                    }
                    if (GetRadius(entity, player, range, 3) && entity.cordinates.x < player.x)
                    {
                        WriteAt("  ", entity.cordinates.x * 2, entity.cordinates.y);
                        entity.Health -= dmg;
                        if (grid[entity.cordinates.y, entity.cordinates.x - range] == 0) { entity.cordinates.x -= knockback; }
                        is_there = true;
                        entity.cordinates.y -= knockback;
                        entity.velocity += -1;
                    }
                }

            }
            return is_there;
        }
        static void Shoot_Projectile(Player player, Game game, Cordinates cordinates, int ID)
        {
            Entity entity = game.Projectiles[ID];
            entity.cordinates = cordinates;
            entity.cordinates.y -= 1;
            entity.cordinates.x += 1;
            entity.cordinates.x1 = cordinates.x;
            game.Existing_Entities.Add(entity);


        }

        static void Slash(Player player, Game game, int[,] grid)
        {
            int x = 0;
            int y = player.y - 4;
            if (player.last_key == "D")
            {
                x = (player.x + 2) * 2;
                Cordinates cordinates = new Cordinates();
                cordinates.x = 5;
                cordinates.y = 5;
                cordinates.x1 = -2;
                cordinates.y1 = 4;
                Console.ForegroundColor = ConsoleColor.Red;

                Thread.Sleep(20);
                WriteAt("▀ ▄       ", x, y + 0);
                WriteAt("  ▀ ▄     ", x, y + 1);
                WriteAt("    █▄    ", x, y + 2);
                WriteAt("    ▀     ", x, y + 3);
                WriteAt("          ", x, y + 4);
                Thread.Sleep(20);
                Console.ForegroundColor = ConsoleColor.Yellow;
                WriteAt("   ▄      ", x, y + 0);
                WriteAt("    ▀▄    ", x, y + 1);
                WriteAt("     █▄   ", x, y + 2);
                WriteAt("   ▄██▀   ", x, y + 3);
                WriteAt("▄██▀▀     ", x, y + 4);
                Thread.Sleep(40);
                Console.ForegroundColor = ConsoleColor.White
                    ;
                WriteAt("          ", x, y + 0);
                WriteAt("     ▄    ", x, y + 1);
                WriteAt("      ▄   ", x, y + 2);
                WriteAt("     █▀   ", x, y + 3);
                WriteAt("▄ ▄█▀▀    ", x, y + 4);
                Thread.Sleep(10);
                WriteAt("          ", x, y + 0);
                WriteAt("          ", x, y + 1);
                WriteAt("          ", x, y + 2);
                WriteAt("          ", x, y + 3);
                WriteAt("          ", x, y + 4);
                Console.ForegroundColor = default;
            }
            else if (player.last_key == "A")
            {
                Cordinates cordinates = new Cordinates();
                cordinates.x = 5;
                cordinates.y = 5;
                cordinates.x1 = 6;
                cordinates.y1 = 4;
                x = (player.x - 6
                    ) * 2;
                Console.ForegroundColor = ConsoleColor.Red;
                Thread.Sleep(20);
                WriteAt("       ▄ ▀", x, y + 0);
                WriteAt("     ▄ ▀  ", x, y + 1);
                WriteAt("    ▄█    ", x, y + 2);
                WriteAt("     ▀    ", x, y + 3);
                WriteAt("          ", x, y + 4);
                Thread.Sleep(20);
                Console.ForegroundColor = ConsoleColor.Yellow;
                WriteAt("      ▄   ", x, y + 0);
                WriteAt("    ▄▀    ", x, y + 1);
                WriteAt("   ▄█     ", x, y + 2);
                WriteAt("   ▀██▄   ", x, y + 3);
                WriteAt("     ▀▀██▄", x, y + 4);
                Thread.Sleep(40);
                Console.ForegroundColor = ConsoleColor.White;
                WriteAt("          ", x, y + 0);
                WriteAt("    ▄     ", x, y + 1);
                WriteAt("   ▄      ", x, y + 2);
                WriteAt("   ▀█     ", x, y + 3);
                WriteAt("    ▀▀█▄ ▄", x, y + 4);
                Thread.Sleep(10);
                WriteAt("          ", x, y + 0);
                WriteAt("          ", x, y + 1);
                WriteAt("          ", x, y + 2);
                WriteAt("          ", x, y + 3);
                WriteAt("          ", x, y + 4);
            }

        }


        static void Print_window(Camera camera, Game game, Player player)
        {

            int c = 0;
            int UI = camera.View.GetLength(0) + 1;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            WriteAt("Health" + player.health.ToString(), 1, UI + 2);
            WriteAt("Oxygen" + player.oxygen.ToString(), 1, UI + 3);
            WriteAt("                                                                    ", 1, UI + 1);
            int offset = player.hotbar_offset;
            for (int e = 0; e < 10; e++)
            {
                var i = game.Block_list[e + offset];
                Console.ForegroundColor = i.FG;
                Console.BackgroundColor = i.BG;
                WriteAt(i.Texture.ToString(), c * 2, UI);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                WriteAt(i.quantity.ToString(), c * 2, UI - 1);
                c++;


            }
            WriteAt("Selected id:" + player.Selected_block.id.ToString() + "   ", 40, 12);

            WriteAt("Mining level " + game.player_pic_lv.ToString(), 40, 10);

            int hotbar_selected = player.hotbar ;

            WriteAt("^^", hotbar_selected * 2, UI + 1);
            WriteAt(player.Crafting_select.ToString() + ":" + game.recipes[player.Crafting_select].item.Name + "       ", 2, UI + 4);
            WriteAt(player.x.ToString() + ":" + player.y.ToString(), 44, UI + 3);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = default;

        }
        static void Craft(Recipe name, Game game)
        {

            foreach (var Item in name.required)
            {
                if (game.GetBlock(Item.Name).quantity >= Item.Amount)
                {
                    game.GetBlock(name.item.Name).quantity += name.num;
                    game.GetBlock(Item.Name).quantity -= Item.Amount;
                }

            }
        }

    }
}
