using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public enum AsteroideTipo
    {
        Normal,
        Especial
    }

    public class Asteroide : GameObject, IUpdatable, IDrawable, ICollidable, IPoolable
    {
        public float collisionRadius = 40f;
        public Collider Collider { get; }
        public bool IsActive { get; set; }
        public AsteroideTipo Tipo { get; set; }

        public float CenterX => x + 25f;
        public float CenterY => y + 25f;

        public Asteroide() : base(null, 0, 0, 0, 0)
        {
            Collider = new Collider(this);
            Collider.OnCollision += OnCollision;
            IsActive = false;
            Tipo = AsteroideTipo.Normal;
        }

        // Implementación de IPoolable
        public void Reset(float x, float y, float dx, float dy, Image sprite)
        {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
            this.sprite = sprite;
            this.Tipo = AsteroideTipo.Normal;
            IsActive = true;
        }

        
        public void Reset(float x, float y, float dx, float dy, Image sprite, AsteroideTipo tipo)
        {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
            this.sprite = sprite;
            this.Tipo = tipo;
            IsActive = true;
        }

        public bool IsOffScreen(int width, int height)
        {
            return x < -50 || x > width + 50 || y < -50 || y > height + 50;
        }

        public override void Update()
        {
            if (!IsActive) return;
            x += dx;
            y += dy;
        }

        public override void Draw()
        {
            if (IsActive)
                Engine.Draw(sprite, x, y);
        }

        public void OnCollision(GameObject other)
        {
            IsActive = false;
        }
    }
}

