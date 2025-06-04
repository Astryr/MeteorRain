using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Bullet : GameObject
    {
        private float speed = 10f;
        private float angle;
        private int direction;
        public bool IsActive { get; private set; } = true;

        public Bullet(float startX, float startY, float angleDegrees, Image img, int direction = 1)
            : base(img, startX, startY, 0, 0)
        {
            this.angle = angleDegrees;
            this.direction = direction;
            float rad = angleDegrees * (float)Math.PI / 180f;
            dx = (float)Math.Cos(rad) * speed * direction;
            dy = (float)Math.Sin(rad) * speed * direction;
        }

        public override void Update()
        {
            base.Update();
            if (x < 0 || x > 1700 || y < 0 || y > 900)
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

