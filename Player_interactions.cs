using cammera;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft
{
    internal class Player_interactions
    {
        
        public static void Use_Item(Player player, Game game, int[,] Grid, Solid Item)
        {
            if (Item.Name[0] == 'h' && Item.quantity > 0
                )
            {
                if(Grid[player.y+1, player.x] == 1 || Grid[player.y + 1, player.x] == 2)
                {
                    Grid[player.y+1, player.x] = game.GetBlock("Soil").id;
                }
            }
            else
            if (Item.Name == "Weeds" && Item.quantity > 0)
            {
                Item.quantity--;
                if (Grid[player.y + 1, player.x] == game.GetBlock("Soil").id)
                {
                    Grid[player.y, player.x] = game.GetBlock("Planted_seeds_wheat").id;
                }
            }
            if(Item.Category == "Weapon")
            {
                Program.Attack(game, player.Convert_cor(player.x, player.y), Grid, Item.knc, 2, Item.dmg);
            }

        }
    }
}
