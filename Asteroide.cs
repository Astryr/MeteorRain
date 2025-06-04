using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Asteroide
    {
        public Image sprite;
        public float x, y;
        public float dx, dy;

        public float collisionRadius = 40f;

        public float X => x;
        public float Y => y;
        public float CenterX => x + 25f; 
        public float CenterY => y + 25f; 

        public Asteroide(Image img, float startX, float startY, float dirX, float dirY)
        {
            sprite = img;
            x = startX;
            y = startY;
            dx = dirX;
            dy = dirY;
        }

        public void Update()
        {
            x += dx;
            y += dy;
        }

        public void Draw()
        {
            Engine.Draw(sprite, x, y);
        }

        public bool IsOffScreen(int width, int height)
        {
            return x < -50 || x > width + 50 || y < -50 || y > height + 50;
        }
    }
}

