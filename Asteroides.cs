using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Asteroides
    {
        private Image asteroidImg = Engine.LoadImage("assets/asteroide.png");
        private List<Asteroide> listaAsteroides = new List<Asteroide>();
        private Random rand = new Random();
        private int screenWidth = 1700;
        private int screenHeight = 900;

        public event Action<Asteroide> AsteroideDestruido;

        public Asteroides()
        {
        }

        public void Update()
        {
            // Crear nuevo asteroide aleatorio con una probabilidad
            if (rand.NextDouble() < 0.02)
            {
                CrearAsteroide();
            }

            // Actualizar todos los asteroides y eliminar los que salen de pantalla
            for (int i = listaAsteroides.Count - 1; i >= 0; i--)
            {
                listaAsteroides[i].Update();

                if (listaAsteroides[i].IsOffScreen(screenWidth, screenHeight))
                    listaAsteroides.RemoveAt(i);
            }
        }

        public void Render()
        {
            foreach (var ast in listaAsteroides)
            {
                ast.Draw();
            }
        }

        private void CrearAsteroide()
        {
            int borde = rand.Next(4);
            float x = 0, y = 0, dx = 0, dy = 0;
            float speed = 2.0f;

            switch (borde)
            {
                case 0: // Izquierda
                    x = -50;
                    y = rand.Next(screenHeight);
                    dx = speed;
                    dy = (float)(rand.NextDouble() * 2 - 1);
                    break;
                case 1: // Derecha
                    x = screenWidth + 50;
                    y = rand.Next(screenHeight);
                    dx = -speed;
                    dy = (float)(rand.NextDouble() * 2 - 1);
                    break;
                case 2: // Arriba
                    x = rand.Next(screenWidth);
                    y = -50;
                    dx = (float)(rand.NextDouble() * 2 - 1);
                    dy = speed;
                    break;
                case 3: // Abajo
                    x = rand.Next(screenWidth);
                    y = screenHeight + 50;
                    dx = (float)(rand.NextDouble() * 2 - 1);
                    dy = -speed;
                    break;
            }

            listaAsteroides.Add(new Asteroide(asteroidImg, x, y, dx, dy));
        }

        public void CheckCollisionsWithPlayer(Player player)
        {
            foreach (var asteroid in listaAsteroides)
            {
                if (player.CollidesWith(asteroid))
                {
                    player.Freeze();
                    break;
                }
            }
        }

        public void CheckBulletCollisions(List<Bullet> bullets, Action sumarPuntos)
        {
            for (int i = listaAsteroides.Count - 1; i >= 0; i--)
            {
                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    if (bullets[j].CollidesWith(listaAsteroides[i]))
                    {
                        Asteroide ast = listaAsteroides[i];
                        bullets.RemoveAt(j);
                        listaAsteroides.RemoveAt(i);
                        sumarPuntos();
                        AsteroideDestruido?.Invoke(ast); // Lanzar el evento
                        break;
                    }
                }
            }
        }
    }
}