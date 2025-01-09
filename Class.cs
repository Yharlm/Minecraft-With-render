namespace Minecraft
{

    class Player : Cordinates

    {
        public List<Entity> Projectiles = new List<Entity>();
        public int Crafting_select = 0;
        public List<Recipe> Recipes = new List<Recipe>();
        public Cordinates last_popup;
        public int health = 100;
        public Entity held = null;
        public bool Holding = false;
        public List<Non_solid> Block_Back_list = new List<Non_solid>();
        public Cordinates Spawnpoint = new Cordinates();
        


        public bool is_swiming = false;
        public int hotbar;
        public int Entity_hotbar = 0;
        //public Block_ids Selected_block = null;
        public Solid Selected_block = null;
        public string special_key = "";
        public string last_key = "";
        public string Input = "";
        //public int x = 4;
        //public int y = 11;
        public int x_size = 70;
        public int y_size = 34;
        public bool grounded = false;
        
    }
    
    class Projectile(string name, int damage)
    {
        public Cordinates cordinates = new Cordinates();
        public string Name = name;
        public Cordinates Velocity;
        public int Damage = damage;
        public void Add_velocity(Cordinates vector)
        {

            cordinates.x += Velocity.x;
            cordinates.y += Velocity.y;
        }
        
    }
    class Inventory : Player
    {
        //public List<Solid> Block_list = new List<Solid>();
    }
    class Cordinates
    {
        
        public int x = 11;
        public int y = 11;
        public int x1 = 0;
        public int y1 = 0;
        public int x2 = 0;
        public int y2 = 0;
        public Cordinates Convert_cor(int x, int y)
        {
            Cordinates cords = new Cordinates();
            cords.y = y;
            cords.x = x;
            return cords;
        }

    }
    class Camera
    {

        public int[,] View = new int[19, 31];
        public Cordinates Position = new Cordinates();
    }
    class Game
    {
        protected static int origRow;
        protected static int origCol;
        public Cordinates mosepos = new Cordinates();
        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);

                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {

            }
        }
        public Cordinates cordinates = new Cordinates();
        public int number = 0;

        public double time = 0;
        public bool curent_tick = false;
        public List<Sprites> Sprite_list = new List<Sprites>();
        public List<Sprites> Displayed_sprites = new List<Sprites>();
        public List<Solid> Block_list = new List<Solid>();
        public List<Entity> Entity_list = new List<Entity>();
        public List<Entity> Existing_Entities = new List<Entity>();
        public List<Entity> Projectiles = new List<Entity>();
        public Solid GetBlock(string name)
        {
            return Block_list.Find(x => x.Name == name);
        }
        public Solid Get_ByID(int id)
        {
            return Block_list.Find(x => x.id == id);
        }

        

        public void Spawn_entity(Entity mob)
        {
            Entity_list.Add(mob);
            Existing_Entities.Add(mob);
            //number++;
        }

        
        public bool delay(int mob_time,int delay,bool tick)
        {
            if (tick)
            {
                mob_time += 1;
            }
            if (mob_time >= delay)
            {
                mob_time = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void gravity(int[,] grid, Game game, List<Entity> Exists)
        {
            int velocity = 0;
            foreach (Entity ent in Exists)
            {

            }
            //WriteAt("  ", cordinates.x, cordinates.y);
            if (grid[cordinates.y + 1, cordinates.x] == 0)
            {

                if (grid[cordinates.y - 2, cordinates.x] == 0 && game.curent_tick)
                {
                    WriteAt("  ", cordinates.x * 2, cordinates.y);
                    cordinates.y++;
                    cordinates.x += velocity;
                }
            }

            WriteAt("██", cordinates.x * 2, cordinates.y);

        }
    }

    class Block_ids(int Id, string texture, ConsoleColor fG, ConsoleColor bG)
    {
        public int id = Id;
        public string Texture = texture;
        public ConsoleColor FG = fG;
        public ConsoleColor BG = bG;


    }
    class Non_Existent(int id, string name, int amount)
    {
        public int Id = id;
        public string Name = name;
        public int Amount = amount;
    }
    class Recipe
    {
        public Solid item;
        public int num = 1;
        public List<Non_Existent> required = new List<Non_Existent>();



    }
    class Solid(string name, int Id, string texture, ConsoleColor fG, ConsoleColor bG)
    {
        public bool solid = true;
        public int quantity = 0;
        public string Name = name;
        public int id = Id;
        public string Texture = texture;
        public ConsoleColor FG = fG;
        public ConsoleColor BG = bG;

    }
    class Non_solid(string name, int Id, string texture, ConsoleColor fG, ConsoleColor bG)
    {
        public int id = Id;
        public string Texture = texture;
        public ConsoleColor FG = fG;
        public ConsoleColor BG = bG;
    }
    //▀▄░█
    class Structure
    {
        public int[,] Struct;
        public void Fill_Index_Cord(int x1, int y1, int x2, int y2, int[,] grid, int id)
        {



        }
    }
    internal class Behaviour
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

            }
        }

        public void Shoot_WithImaginaryProjectiles()
        {

        }

    }
    class Entity(string name, int health, string type, string sprite)
    {
        public bool delay( int delay, bool tick)
        {
            if (tick)
            {
                time += 1;
            }
            if (time >= delay)
            {
                time = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
        public int velocity = 0;
        public string Name = name;
        public int Health = health;
        public string Type = type;
        public List<Behaviour> mob_ais = new List<Behaviour>();
        protected static int origRow;
        protected static int origCol;
        public string Sprite = sprite;
        public Cordinates starting_pos;
        public int time=0;
        public int specialvalue = 0;
        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);

                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {

            }
        }
        public bool grounded = true;

        public Cordinates cordinates = new Cordinates();
        public ConsoleColor Color;
        public ConsoleColor BGColor;
        public void gravity(int[,] grid)
        {

            
            //WriteAt("  ", cordinates.x, cordinates.y);
            if (grid[cordinates.y + 1, cordinates.x] == 0 && Type != "Projectile")
            {




                if (grid[cordinates.y + 2, cordinates.x] == 0)
                {
                    cordinates.y += Math.Abs(velocity) + 1;
                }
                else
                {
                    cordinates.y += 1;
                }
                //cordinates.x += velocity;

            }
            else if (grid[cordinates.y + 1, cordinates.x] != 0)
            {
                velocity = 0;
            }
            
        }



    }
}