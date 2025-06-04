using MyGame;
using System;
using System.Collections.Generic;

public enum PlayerId
{
    One,
    Two
}

public class Player
{
    private Image sprite;
    private float posX, posY;
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

    public Player(PlayerId id, Image sprite, float startX, float startY, float rotationSpeed,
                  int keyLeft, int keyRight, int keyForward, int keyBackward, int keyShoot,
                  Image bulletImg, int bulletDirection)
    {
        this.playerId = id;
        this.sprite = sprite;
        this.posX = startX;
        this.posY = startY;
        this.angle = 0f;
        this.rotationSpeed = rotationSpeed;
        this.keyLeft = keyLeft;
        this.keyRight = keyRight;
        this.keyForward = keyForward;
        this.keyBackward = keyBackward;
        this.keyShoot = keyShoot;
        this.bulletImg = bulletImg;
        this.bulletDirection = bulletDirection;
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
        float nextX = posX, nextY = posY;
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

        posX = nextX;
        posY = nextY;
    }

    public void Update()
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
            bullets.Add(new Bullet(posX, posY, angle, bulletImg, bulletDirection));
            timeSinceLastShot = 0f;
        }

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i].Update();
            if (!bullets[i].IsActive)
                bullets.RemoveAt(i);
        }
    }

    public void Render()
    {
        Engine.DrawRotated(sprite, posX, posY, angle * -1);
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
        return posX < asteroid.x + asteroid.dx &&
               posX + width > asteroid.x &&
               posY < asteroid.y + asteroid.dy &&
               posY + height > asteroid.y;
    }

    public List<Bullet> GetBullets() => bullets;

    private float DegreesToRadians(float degrees)
    {
        return degrees * (float)Math.PI / 180f;
    }
}