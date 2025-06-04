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
    public class Player : GameObject
    {
        private float angle;
        private float rotationSpeed;
        private List<Bullet> bullets = new List<Bullet>();
        private float fireCooldown = 0.4f;
        private float timeSinceLastShot = 0f;
        private bool isFrozen = false;
        private float freezeTimer = 0f;
        private float width = 50, height = 50;
        private PlayerId playerId;

        // Controles personalizados
        private int keyLeft, keyRight, keyForward, keyBackward, keyShoot;
        private Image bulletImg;
        private int bulletDirection;

        // Tamaño de pantalla
        private const float SCREEN_WIDTH = 1700f;
        private const float SCREEN_HEIGHT = 900f;

        public Collider Collider { get; }

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
            Collider.OnCollision += obj => { /* lógica de colisión */ };
        }

        public void Input()
        {
            if (isFrozen) return;

            // ROTACIÓN
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

            // Colisión con bordes de pantalla
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
            Engine.DrawRotated(sprite, x, y, angle * -1);
            foreach (var bullet in bullets)
                bullet.Draw();
        }

        public void Freeze()
        {
            isFrozen = true;
            freezeTimer = 0f;
        }

        public bool CollidesWith(Asteroide asteroid)
        {
            return x < asteroid.x + asteroid.dx &&
                   x + width > asteroid.x &&
                   y < asteroid.y + asteroid.dy &&
                   y + height > asteroid.y;
        }

        public List<Bullet> Bullets => bullets;

        private float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }
    }
}