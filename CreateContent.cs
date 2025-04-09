
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
            Mob = new Entity("Zombie", 40, "", "██");
            Mob.BGColor = ConsoleColor.Green;
            Mob.Color = ConsoleColor.Blue;
            Mob.Sprite1D = ["''", "██"];
            game.Entity_list.Add(Mob);

        }

        
    }
    
}
