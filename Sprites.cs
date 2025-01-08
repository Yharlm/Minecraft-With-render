namespace Minecraft
{
    class Sprites
    {
        
        public string[] sprite;
        
        public Cordinates pos = new Cordinates();
        public Cordinates center = new Cordinates();
        public double lifetime = 0;
        public void despawn(Game game)
        {
            if(game.curent_tick)
            {
                lifetime -= 0.02;
            }
            if (lifetime > 0)
            {
                game.Displayed_sprites.Remove(this);
            }

        }
        
        public static void getCenter(Sprites Sprite)
        {
            Sprite.center.y = Sprite.sprite.GetLength(0)/2;
            Sprite.center.x = Sprite.sprite.GetLength(1)/2;
        }
    }
}
