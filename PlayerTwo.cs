using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class PlayerTwo
    {
        private Image player2 = Engine.LoadImage("assets/navehalo2.png");

        private float posX2 = 1400;
        private float posY2 = 450; 

        private float angle2 = 0f; // Ángulo de rotación en grados
        private float rotationSpeed2 = 100f; // grados por segundo

        private List<BulletTwo> bullets = new List<BulletTwo>();
        public List<BulletTwo> GetBullets() => bullets;

        private float fireCooldown = 0.4f;      // Tiempo entre disparos (en segundos)
        private float timeSinceLastShot = 0f;   // Tiempo acumulado desde el último disparo

        private bool isFrozen = false;          // Indica si el jugador está congelado
        private float freezeTimer = 0f;         // Temporizador para la congelación

        private float width = 50;               // Ancho de la nave (aproximado)
        private float height = 50;              // Alto de la nave (aproximado)

        public PlayerTwo()
        {
        }

        public void Input()
        {
            if (isFrozen) return; // Si el jugador está congelado, no puede moverse

            // ROTACIÓN
            if (Engine.GetKey(Engine.KEY_LEFT))  // Para girar a la izquierda
            {
                angle2 -= rotationSpeed2 * (1f / 60f); // suposición de 60 FPS
            }
            if (Engine.GetKey(Engine.KEY_RIGHT))  // Para girar a la derecha
            {
                angle2 += rotationSpeed2 * (1f / 60f);
            }

            // MOVIMIENTO HACIA ADELANTE
            if (Engine.GetKey(Engine.KEY_DOWN))
            {
                posX2 += (float)Math.Cos(DegreesToRadians(angle2));
                posY2 += (float)Math.Sin(DegreesToRadians(angle2));
            }

            // MOVIMIENTO HACIA ATRÁS
            if (Engine.GetKey(Engine.KEY_UP))
            {
                posX2 -= (float)Math.Cos(DegreesToRadians(angle2));
                posY2 -= (float)Math.Sin(DegreesToRadians(angle2));
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
            if (Engine.GetKey(Engine.KEY_K) && timeSinceLastShot >= fireCooldown)
            {
                bullets.Add(new BulletTwo(posX2, posY2, angle2));
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
            Engine.DrawRotated(player2, posX2, posY2, angle2 * -1);

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
            return posX2 < asteroid.x + asteroid.dx &&
                   posX2 + width > asteroid.x &&
                   posY2 < asteroid.y + asteroid.dy &&
                   posY2 + height > asteroid.y;
        }

        private float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }
    }
}