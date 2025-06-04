namespace MyGame
{
    public abstract class GameObject : IUpdatable, IDrawable
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

        public virtual void Update() { }
        public virtual void Draw() { }
    }
}