using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class BulletTwo
    {
        private Image bulletImg = Engine.LoadImage("assets/bullet2.png");
        public float X, Y;
        private float speed = 10f;
        private float angle;
        private float dx, dy;

        public bool IsActive { get; private set; } = true;

        public BulletTwo(float startX, float startY, float angleDegrees)
        {
            X = startX;
            Y = startY;
            angle = angleDegrees;

            float rad = angleDegrees * (float)Math.PI / 180f;
            dx = (float)Math.Cos(rad) * speed;
            dy = (float)Math.Sin(rad) * speed;
        }

        public void Update()
        {
            X -= dx;
            Y -= dy;

            if (X < 0 || X > 1700 || Y < 0 || Y > 900)
                IsActive = false;
        }

        public void Render()
        {
            Engine.Draw(bulletImg, X, Y);
        }

        public bool CollidesWith(Asteroide ast)
        {
            float dx = X - ast.CenterX;
            float dy = Y - ast.CenterY;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            return distance < ast.collisionRadius;
        }
    }
}