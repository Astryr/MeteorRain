using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Asteroides : IUpdatable, IDrawable
    {
        private Image asteroidImg = Engine.LoadImage("assets/asteroide.png");
        private Image asteroidImg2 = Engine.LoadImage("assets/asteroide2.png");
        private ObjectPool<Asteroide> poolAsteroide;
        private Random rand = new Random();
        private int screenWidth = 1700;
        private int screenHeight = 900;

        public event Action<Asteroide> AsteroideDestruido;

        public Asteroides()
        {
            poolAsteroide = new ObjectPool<Asteroide>(() => new Asteroide(), 15);
        }

        public void Update()
        {
            if (rand.NextDouble() < 0.02)
                CrearAsteroide();

            foreach (var ast in poolAsteroide.ActiveObjects)
            {
                ast.Update();
                if (ast.IsOffScreen(screenWidth, screenHeight))
                    poolAsteroide.Release(ast);
            }
        }

        public void Draw()
        {
            foreach (var ast in poolAsteroide.ActiveObjects)
                ast.Draw();
        }

        private void CrearAsteroide()
        {
            int borde = rand.Next(4);
            float x = 0, y = 0, dx = 0, dy = 0;
            float speed = 2.0f;

            switch (borde)
            {
                case 0: x = -50; y = rand.Next(screenHeight); dx = speed; dy = (float)(rand.NextDouble() * 2 - 1); break;
                case 1: x = screenWidth + 50; y = rand.Next(screenHeight); dx = -speed; dy = (float)(rand.NextDouble() * 2 - 1); break;
                case 2: x = rand.Next(screenWidth); y = -50; dx = (float)(rand.NextDouble() * 2 - 1); dy = speed; break;
                case 3: x = rand.Next(screenWidth); y = screenHeight + 50; dx = (float)(rand.NextDouble() * 2 - 1); dy = -speed; break;
            }

            // 50% de probabilidad de cada tipo
            Asteroide ast;
            if (rand.NextDouble() < 0.5)
            {
                ast = poolAsteroide.Get(x, y, dx, dy, asteroidImg);
                ast.Tipo = AsteroideTipo.Normal;
            }
            else
            {
                ast = poolAsteroide.Get(x, y, dx, dy, asteroidImg2);
                ast.Tipo = AsteroideTipo.Especial;
            }
        }

        public void CheckCollisionsWithPlayer(Player player)
        {
            foreach (var asteroid in poolAsteroide.ActiveObjects)
                if (player.CollidesWith(asteroid)) { player.Freeze(); break; }
        }

        public void CheckBulletCollisions(List<Bullet> bullets, Action sumarPuntos)
        {
            foreach (var ast in poolAsteroide.ActiveObjects)
            {
                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    if (bullets[j].CollidesWith(ast))
                    {
                        bullets.RemoveAt(j);
                        poolAsteroide.Release(ast);
                        sumarPuntos();
                        AsteroideDestruido?.Invoke(ast);
                        break;
                    }
                }
            }
        }
    }
}