
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Minecraft
{

    internal class CreateContent
    {
        public static void LoadEntities(Game game)
        {
            Entity Mob = new Entity("", 0, "", "██");
        
            // Creates an object(Entity)
            Mob = new Entity("Zombie", 5, "Hostile", "██");
            Mob.BGColor = ConsoleColor.DarkGreen;
            Mob.FGColor = ConsoleColor.White;
            
            Mob.Sprite1D = ["  ", ".."];
            game.Entity_list.Add(Mob);

            Mob = new Entity("Creeper", 5, "Hostile", "██");
            Mob.BGColor = ConsoleColor.DarkGreen;
            Mob.FGColor = ConsoleColor.Red;

            Mob.Sprite1D = ["  ", ".,"];
            game.Entity_list.Add(Mob);

            Mob = new Entity("Slime", 3, "Hostile", "██");
            Mob.BGColor = ConsoleColor.DarkGreen;
            Mob.FGColor = ConsoleColor.DarkBlue;

            Mob.speed = 0;
            game.Entity_list.Add(Mob);

        }




    }

    


}
