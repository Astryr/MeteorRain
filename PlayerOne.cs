using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class PlayerOne
    {
        private Image player1 = Engine.LoadImage("assets/navehalo.png");

        private float posX = 400;
        private float posY = 450;

        private float angle = 0f; // Ángulo de rotación en grados
        private float rotationSpeed = 100f; // grados por segundo

        private List<Bullet> bullets = new List<Bullet>();
        public List<Bullet> GetBullets() => bullets;

        private float fireCooldown = 0.4f;      // Tiempo entre disparos (en segundos)
        private float timeSinceLastShot = 0f;   // Tiempo acumulado desde el último disparo

        private bool isFrozen = false;          // Indica si el jugador está congelado
        private float freezeTimer = 0f;         // Temporizador para la congelación

        private float width = 50;               // Ancho de la nave (aproximado)
        private float height = 50;              // Alto de la nave (aproximado)

        public PlayerOne()
        {
        }

        public void Input()
        {
            if (isFrozen) return; // Si el jugador está congelado, no puede moverse

            // ROTACIÓN
            if (Engine.GetKey(Engine.KEY_A))
            {
                angle -= rotationSpeed * (1f / 60); // suponiendo 60 FPS
            }
            if (Engine.GetKey(Engine.KEY_D))
            {
                angle += rotationSpeed * (1f / 60);
            }

            // MOVIMIENTO HACIA ADELANTE
            if (Engine.GetKey(Engine.KEY_S))
            {
                posX -= (float)Math.Cos(DegreesToRadians(angle));
                posY -= (float)Math.Sin(DegreesToRadians(angle));
            }

            // MOVIMIENTO HACIA ATRÁS
            if (Engine.GetKey(Engine.KEY_W))
            {
                posX += (float)Math.Cos(DegreesToRadians(angle));
                posY += (float)Math.Sin(DegreesToRadians(angle));
            }
        }

        public void Update()
        {
            if (isFrozen)
            {
                // Actualizar el temporizador de congelación
                freezeTimer += Program.DeltaTime;
                if (freezeTimer >= 3f) // Si han pasado 3 segundos
                {
                    isFrozen = false; // El jugador ya no está congelado
                    freezeTimer = 0f; // Resetear el temporizador
                }
                return; // Si está congelado, no se actualiza el movimiento ni disparos
            }

            Input();

            // Actualizar temporizador de disparo
            timeSinceLastShot += Program.DeltaTime;

            // Disparo (tecla espacio con cooldown)
            if (Engine.GetKey(Engine.KEY_G) && timeSinceLastShot >= fireCooldown)
            {
                bullets.Add(new Bullet(posX, posY, angle));
                timeSinceLastShot = 0f; // Reiniciar cooldown
            }

            // Actualizar balas
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();
                if (!bullets[i].IsActive)
                    bullets.RemoveAt(i);
            }
        }

        public void Render()
        {
            Engine.DrawRotated(player1, posX, posY, angle * -1);

            foreach (var bullet in bullets)
            {
                bullet.Render();
            }
        }

        public void Freeze()
        {
            isFrozen = true; // Congelar al jugador
            freezeTimer = 0f; // Reiniciar el temporizador
        }

        public bool CollidesWith(Asteroide asteroid)
        {
            // Colisión de AABB
            return posX < asteroid.x + asteroid.dx &&
                   posX + width > asteroid.x &&
                   posY < asteroid.y + asteroid.dy &&
                   posY + height > asteroid.y;
        }

        private float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }
    }
}