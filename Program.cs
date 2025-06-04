using System;
using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Tao.Sdl;

namespace MyGame
{
    class Program
    {
        static private Image background = Engine.LoadImage("assets/fondohalo.png");
        static private Image startScreen = Engine.LoadImage("assets/pantalla_inicio.png");

        enum GameState { Start, Playing, Finish }
        static GameState actualState = GameState.Start;

        static private Player player1;
        static private Player player2;
        static private Asteroides asteroid1;

        static private GameManager gameManager;

        static private DateTime lastTime;
        static public float DeltaTime;

        static private Sound backgroundMusic;

        static void Main(string[] args)
        {
            Engine.Initialize();

            // Initialize audio
            SdlMixer.Mix_OpenAudio(22050, (short)(int)Sdl.AUDIO_S16SYS, 2, 4096);
            SdlMixer.Mix_VolumeMusic(32); // Volume

            // Cargar música
            backgroundMusic = new Sound("assets/background.mp3");
            backgroundMusic.Play();

            player1 = new Player(
                PlayerId.One,
                Engine.LoadImage("assets/navehalo.png"),
                400, 450, 100f,
                Engine.KEY_A, Engine.KEY_D, Engine.KEY_W, Engine.KEY_S, Engine.KEY_G,
                Engine.LoadImage("assets/bullet.png"), 1
            );

            player2 = new Player(
                PlayerId.Two,
                Engine.LoadImage("assets/navehalo2.png"),
                1400, 450, 100f,
                Engine.KEY_LEFT, Engine.KEY_RIGHT, Engine.KEY_DOWN, Engine.KEY_UP, Engine.KEY_K,
                Engine.LoadImage("assets/bullet2.png"), -1
            );

            asteroid1 = new Asteroides();
            asteroid1.AsteroideDestruido += (ast) =>
            {
                Console.WriteLine("¡Asteroide destruido en: " + ast.x + ", " + ast.y + "!");
            };

            gameManager = GameManager.Instance;

            lastTime = DateTime.Now;

            while (true)
            {
                Input();
                DateTime actualTime = DateTime.Now;
                DeltaTime = (float)(actualTime - lastTime).TotalSeconds;
                lastTime = actualTime;
                Update();
                Render();
            }
        }

        static void Input()
        {
            if (actualState == GameState.Start)
            {
                Sdl.SDL_Event e;
                if (Sdl.SDL_PollEvent(out e) != 0 && e.type == Sdl.SDL_KEYDOWN)
                {
                    actualState = GameState.Playing;
                    gameManager.Reset();
                    lastTime = DateTime.Now;
                }
                return;
            }

            if (actualState == GameState.Playing && !gameManager.GameFinished())
            {
                player1.Input();
                player2.Input();
            }

            if (Engine.GetKey(Engine.KEY_ESC))
            {
                Environment.Exit(0);
            }
        }

        static void Update()
        {
            if (actualState == GameState.Playing && !gameManager.GameFinished())
            {
                player1.Update();
                player2.Update();

                asteroid1.Update();

                // Verificar colisiones con los jugadores
                asteroid1.CheckCollisionsWithPlayer(player1);
                asteroid1.CheckCollisionsWithPlayer(player2);

                asteroid1.CheckBulletCollisions(player1.Bullets, () => GameManager.Instance.SumarPuntosJugador1(100));
                asteroid1.CheckBulletCollisions(player2.Bullets, () => GameManager.Instance.SumarPuntosJugador2(100));

                gameManager.Update();

                if (gameManager.GameFinished())
                {
                   actualState = GameState.Finish;
                }
            }
        }
        static void Render()
        {
            Engine.Clear();
            Engine.Draw(background, 0, 0);

            if (actualState == GameState.Start)
            {
                Engine.Draw(startScreen, 0, 0);
                Engine.DrawText("METEOR RAIN", 275, 375, 0, 0, 255, gameManager.Font1);
                Engine.DrawText("PRESS ANY KEY TO START", 600, 800, 255, 255, 255, gameManager.Font);
            }
            else if (actualState == GameState.Playing)
            {
                player1.Draw();
                player2.Draw();
                asteroid1.Render();
                gameManager.Render();
            }
            else if (actualState == GameState.Finish)
            {
                gameManager.Render();
            }

            Engine.Show();
        }
    }
}