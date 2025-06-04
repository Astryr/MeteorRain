namespace MyGame
{
    public interface ICollidable
    {
        Collider Collider { get; }
        void OnCollision(GameObject other);
    }
}