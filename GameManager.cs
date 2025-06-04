using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class GameManager
    {
        private IntPtr font;
        private IntPtr font1;
        private static GameManager instance;
        private float remainingTime = 60f; // segundos
        private bool finishGame = false;
        private Image finalScreen;

        private int scorePlayer1 = 0;
        private int scorePlayer2 = 0;

        public IntPtr Font => font;
        public IntPtr Font1 => font1;

        private GameManager()
        {
            finalScreen  = Engine.LoadImage("assets/gameover.jpg"); // Asegurate de tener esta imagen
            font = Engine.LoadFont2("assets/Minecraft.ttf", 36);
            font1 = Engine.LoadFont2("assets/Minecraft.ttf", 160);
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        public void Reset()
        {
            remainingTime = 60f;
            finishGame  = false;
            scorePlayer1 = 0;
            scorePlayer2  = 0;
        }

        public void Update()
        {
            if (finishGame)
                return;

            remainingTime -= Program.DeltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                finishGame = true;
            }
        }

        public void Render()
        {
            if (finishGame)
            {
                Engine.Draw(finalScreen, 0, 0); // Dibuja imagen final
                Engine.DrawText($"Player 1: {scorePlayer1} pts", 200, 700, 255, 255, 255, font);
                Engine.DrawText($"Player 2: {scorePlayer2} pts", 1100, 700, 255, 255, 255, font);
            }
            else
            {
                string tiempoTexto = $"Time: {Math.Ceiling(remainingTime)}";
                int anchoEstimado = tiempoTexto.Length * 20;
                int x = (1700 - anchoEstimado) / 2;
                Engine.DrawText(tiempoTexto, x, 30, 255, 255, 255, font);

                Engine.DrawText($"P1: {scorePlayer1}", 50, 30, 0, 255, 255, font);
                Engine.DrawText($"P2: {scorePlayer2}", 1500, 30, 255, 255, 0, font);
            }
        }

        public void SumarPuntosJugador1(int puntos)
        {
            scorePlayer1 += puntos;
        }

        public void SumarPuntosJugador2(int puntos)
        {
            scorePlayer2 += puntos;
        }

        public bool GameFinished()
        {
            return finishGame;
        }
    }
}
