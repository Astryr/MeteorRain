using MyGame;
using System;
using System.Collections.Generic;

public enum PlayerId
{
    One,
    Two
}

namespace MyGame
{
    public class Player : GameObject, IUpdatable, IDrawable, ICollidable
    {
        private float angle;
        private float rotationSpeed;
        private List<Bullet> bullets = new List<Bullet>();
        private float fireCooldown = 0.4f;
        private float timeSinceLastShot = 0f;
        private bool isFrozen = false;
        private float freezeTimer = 0f;
        private float width = 50, height = 55;
        private PlayerId playerId;

        // Controles personalizados
        private int keyLeft, keyRight, keyForward, keyBackward, keyShoot;
        private Image bulletImg;
        private int bulletDirection;

        // Tama�o de pantalla
        private const float SCREEN_WIDTH = 1700f;
        private const float SCREEN_HEIGHT = 900f;

        public Collider Collider { get; }

        public float Angle => angle;

        public Player(PlayerId id, Image sprite, float startX, float startY, float rotationSpeed,
                      int keyLeft, int keyRight, int keyForward, int keyBackward, int keyShoot,
                      Image bulletImg, int bulletDirection)
            : base(sprite, startX, startY, 0, 0)
        {
            this.playerId = id;
            this.angle = 0f;
            this.rotationSpeed = rotationSpeed;
            this.keyLeft = keyLeft;
            this.keyRight = keyRight;
            this.keyForward = keyForward;
            this.keyBackward = keyBackward;
            this.keyShoot = keyShoot;
            this.bulletImg = bulletImg;
            this.bulletDirection = bulletDirection;

            Collider = new Collider(this);
            Collider.OnCollision += OnCollision;
        }

        public void Input()
        {
            if (isFrozen) return;

            // ROTACI�N
            if (Engine.GetKey(keyLeft))
                angle -= rotationSpeed * (1f / 60f);
            if (Engine.GetKey(keyRight))
                angle += rotationSpeed * (1f / 60f);

            // MOVIMIENTO
            float nextX = x, nextY = y;
            if (Engine.GetKey(keyForward))
            {
                nextX += (float)Math.Cos(DegreesToRadians(angle));
                nextY += (float)Math.Sin(DegreesToRadians(angle));
            }
            if (Engine.GetKey(keyBackward))
            {
                nextX -= (float)Math.Cos(DegreesToRadians(angle));
                nextY -= (float)Math.Sin(DegreesToRadians(angle));
            }

            // Colisi�n con bordes de pantalla
            if (nextX < 0) nextX = 0;
            if (nextX > SCREEN_WIDTH - width) nextX = SCREEN_WIDTH - width;
            if (nextY < 0) nextY = 0;
            if (nextY > SCREEN_HEIGHT - height) nextY = SCREEN_HEIGHT - height;

            x = nextX;
            y = nextY;
        }

        public override void Update()
        {
            if (isFrozen)
            {
                freezeTimer += Program.DeltaTime;
                if (freezeTimer >= 3f)
                {
                    isFrozen = false;
                    freezeTimer = 0f;
                }
                // Actualizar balas aunque la nave est� congelada
                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    bullets[i].Update();
                    if (!bullets[i].IsActive)
                        bullets.RemoveAt(i);
                }
                return;
            }

            Input();

            timeSinceLastShot += Program.DeltaTime;

            if (Engine.GetKey(keyShoot) && timeSinceLastShot >= fireCooldown)
            {
                bullets.Add(new Bullet(x, y, angle, bulletImg, bulletDirection));
                timeSinceLastShot = 0f;
            }

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();
                if (!bullets[i].IsActive)
                    bullets.RemoveAt(i);
            }
        }

        public override void Draw()
        {
            // Dibuja el jugador rotado
            Engine.DrawRotated(sprite, x, y, angle * -1);
            // Dibuja las balas
            foreach (var bullet in bullets)
                bullet.Draw();
        }

        public void OnCollision(GameObject other)
        {
            if (other is Asteroide)
                Freeze();
        }

        public void Freeze()
        {
            isFrozen = true;
            freezeTimer = 0f;
        }

        public bool CollidesWith(Asteroide asteroid)
        {
            // Centro del asteroide
            float circleX = asteroid.CenterX;
            float circleY = asteroid.CenterY;
            float radius = asteroid.collisionRadius;

            // Bordes del rect�ngulo (nave)
            float rectX = x;
            float rectY = y;
            float rectW = width;
            float rectH = height;

            // Encuentra el punto m�s cercano del rect�ngulo al centro del c�rculo
            float closestX = Math.Max(rectX, Math.Min(circleX, rectX + rectW));
            float closestY = Math.Max(rectY, Math.Min(circleY, rectY + rectH));

            // Distancia desde el centro del c�rculo al punto m�s cercano
            float dx = circleX - closestX;
            float dy = circleY - closestY;

            return (dx * dx + dy * dy) < (radius * radius);
        }

        public List<Bullet> Bullets => bullets;

        private float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }
    }
}