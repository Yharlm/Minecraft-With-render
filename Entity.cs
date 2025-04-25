using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft
{
    class Entity(string name, int health, string type, string sprite)
    {
        public string[,] Sprite2D = null;
        public string[] Sprite1D = null;
        public string Sprite = sprite;
        public ConsoleColor FGColor;
        public ConsoleColor BGColor;
        public void Load_sprite(string[,] new_sprite)
        {
            Sprite2D = new_sprite;
        }
        public bool delay(int delay, bool tick)
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

        public string Name = name;
        public int Health = health;
        public string Type = type;
        public int damage = 0;

        protected static int origRow;
        protected static int origCol;
        public int speed = 3;
        public Cordinates starting_pos;
        public double time;
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
        public string Source = "";
        public Cordinates cordinates = new Cordinates();
        public string on_hit = "";
        public bool hit = false;
        public float velocity_x = 0;
        public float velocity_y = 0;
        public double temp_vx = 0;
        public double temp_vy = 0;
        public float Velocity_drag = 1;
        public void Add_velocity(Cordinates vector, float drag)
        {
            Velocity_drag += drag;
            velocity_x += vector.x;
            velocity_y += vector.y;
        }
        public void Add_velocity(Cordinates vector)
        {

            velocity_x += vector.x;
            velocity_y += vector.y;
        }

        public void Overide_velocity(Cordinates vector)
        {

            velocity_x = vector.x;
            velocity_y = vector.y;
        }
        public void Velocity(double time, Game game, int[,] grid)
        {

            if (time % Velocity_drag == 0)
            {
                if (velocity_x > 0 && !game.Block_list.Find(x => x.id == grid[cordinates.y, cordinates.x + 1]).Collidable)
                {
                    cordinates.x += 1;
                    if (velocity_x != 0)
                    {
                        velocity_x--;

                    }
                }

                if (velocity_x < 0 && !game.Block_list.Find(x => x.id == grid[cordinates.y, cordinates.x - 1]).Collidable)
                {
                    cordinates.x -= 1;
                    if (velocity_x != 0)
                    {
                        velocity_x++;

                    }
                }

                if (velocity_y > 0 && !game.Block_list.Find(x => x.id == grid[cordinates.y + 1, cordinates.x]).Collidable)
                {
                    cordinates.y += 1;
                    if (velocity_x != 0)
                    {
                        velocity_y--;

                    }
                }

                if (velocity_y < 0 && !game.Block_list.Find(x => x.id == grid[cordinates.y - 1, cordinates.x]).Collidable)
                {
                    cordinates.y -= 1;
                    if (velocity_x != 0)
                    {
                        velocity_y++;

                    }
                }


            }


        }
        public float x_velocity = 0;
        public float y_velocity = 0;
        public void VelocityV2(double time, Game game, int[,] grid)
        {

            if (x_velocity > 0)
            {

                cordinates.x += (int)float.Ceiling(x_velocity);
            }
            else
            {
                x_velocity += velocity_x / Velocity_drag;
            }
            




        }

    }
}
