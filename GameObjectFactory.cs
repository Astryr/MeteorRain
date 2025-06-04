using System;
using System.Collections.Generic;

namespace MyGame
{
    public static class GameObjectFactory
    {
        private static List<Asteroide> listaAsteroides = new List<Asteroide>();
        private static List<Bullet> bullets = new List<Bullet>();

        public static Asteroide CrearAsteroide(Image img, float x, float y, float dx, float dy)
        {
            var asteroide = new Asteroide();
            asteroide.Reset(x, y, dx, dy, img);
            listaAsteroides.Add(asteroide);
            return asteroide;
        }

        public static Bullet CrearBullet(float x, float y, float angle, Image img, int direction)
        {
            var bullet = new Bullet(x, y, angle, img, direction);
            bullets.Add(bullet);
            return bullet;
        }

        public static Player CrearPlayer(PlayerId id, Image sprite, float startX, float startY, float rotationSpeed,
                                         int keyLeft, int keyRight, int keyForward, int keyBackward, int keyShoot,
                                         Image bulletImg, int bulletDirection)
        {
            
            return new Player(id, sprite, startX, startY, rotationSpeed,
                              keyLeft, keyRight, keyForward, keyBackward, keyShoot,
                              bulletImg, bulletDirection);
        }
    }

    public class Game
    {
        private Player player1;
        private Player player2;

        public void InitializePlayers()
        {
            player1 = GameObjectFactory.CrearPlayer(
                PlayerId.One,
                Engine.LoadImage("assets/navehalo.png"),
                400, 450, 100f,
                Engine.KEY_A, Engine.KEY_D, Engine.KEY_W, Engine.KEY_S, Engine.KEY_G,
                Engine.LoadImage("assets/bullet.png"), 1
            );

            player2 = GameObjectFactory.CrearPlayer(
                PlayerId.Two,
                Engine.LoadImage("assets/navehalo2.png"),
                1400, 450, 100f,
                Engine.KEY_LEFT, Engine.KEY_RIGHT, Engine.KEY_DOWN, Engine.KEY_UP, Engine.KEY_K,
                Engine.LoadImage("assets/bullet2.png"), -1
            );
        }
    }
}