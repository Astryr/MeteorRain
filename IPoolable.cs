namespace MyGame
{
    public interface IPoolable
    {
        void Reset(float x, float y, float dx, float dy, Image sprite);
        bool IsActive { get; set; }
    }
}