namespace MyGame
{
    public abstract class GameObject
    {
        public Image sprite;
        public float x, y;
        public float dx, dy;

        public GameObject(Image sprite, float x, float y, float dx, float dy)
        {
            this.sprite = sprite;
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
        }

        public virtual void Update()
        {
            x += dx;
            y += dy;
        }

        public virtual void Draw()
        {
            Engine.Draw(sprite, x, y);
        }
    }
}