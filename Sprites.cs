namespace Minecraft
{
    class Sprites
    {
        
        public string[] sprite;
        
        public Cordinates pos = new Cordinates();
        public Cordinates center = new Cordinates();
        public static void getCenter(Sprites Sprite)
        {
            Sprite.center.y = Sprite.sprite.GetLength(0)/2;
            Sprite.center.x = Sprite.sprite.GetLength(1)/2;
        }
    }
}
