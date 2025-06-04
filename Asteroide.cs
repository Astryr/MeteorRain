using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Asteroide : GameObject, IUpdatable, IDrawable, ICollidable
    {
        public float collisionRadius = 40f;
        public Collider Collider { get; }

        public float CenterX => x + 25f; 
        public float CenterY => y + 25f; 

        public Asteroide(Image img, float startX, float startY, float dirX, float dirY)
            : base(img, startX, startY, dirX, dirY)
        {
            Collider = new Collider(this);
            Collider.OnCollision += OnCollision;
        }

        public bool IsOffScreen(int width, int height)
        {
            return x < -50 || x > width + 50 || y < -50 || y > height + 50;
        }

        public override void Update()
        {
            x += dx;
            y += dy;
        }

        public override void Draw()
        {
            Engine.Draw(sprite, x, y);
        }

        public void OnCollision(GameObject other)
        {
            // Lógica de colisión para asteroide (puedes dejarlo vacío o implementar efectos)
        }
    }
}

