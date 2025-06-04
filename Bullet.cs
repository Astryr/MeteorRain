using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Bullet : GameObject, IUpdatable, IDrawable, ICollidable
    {
        private float speed = 10f;
        private float angle;
        private int direction;
        public bool IsActive { get; private set; } = true;
        public Collider Collider { get; }

        public Bullet(float startX, float startY, float angleDegrees, Image img, int direction = 1)
            : base(img, startX, startY, 0, 0)
        {
            this.angle = angleDegrees;
            this.direction = direction;
            Collider = new Collider(this);
            Collider.OnCollision += OnCollision;
            float rad = angleDegrees * (float)Math.PI / 180f;
            dx = (float)Math.Cos(rad) * speed * direction;
            dy = (float)Math.Sin(rad) * speed * direction;
        }

        public override void Update()
        {
            x += (float)Math.Cos(angle * Math.PI / 180f) * speed * direction;
            y += (float)Math.Sin(angle * Math.PI / 180f) * speed * direction;
            if (x < 0 || x > 1700 || y < 0 || y > 900)
                IsActive = false;
        }

        public override void Draw()
        {
            Engine.Draw(sprite, x, y);
        }

        public void OnCollision(GameObject other)
        {
            if (other is Asteroide)
                IsActive = false;
        }

        public bool CollidesWith(Asteroide ast)
        {
            float dx = x - ast.CenterX;
            float dy = y - ast.CenterY;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance < ast.collisionRadius;
        }
    }
}

