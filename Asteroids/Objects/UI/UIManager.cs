using HML;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Asteroids.Objects.UI
{
    public class UIManager : VectorObject
    {
        public static UIManager Instance;

        Resolution resolution;

        Text scoreText, higestscore, copyright;
        Text gameOver, newHighscore;

        Scoreboard scoreboard;

        public override Setup Start()
        {
            Instance = this;

            scoreText = new Text("00", 2, new SKPoint(200, 10), Text.TextAlignment.Right);
            window.Instantiate(scoreText);

            window.Instantiate(new LifeCounter(new Point(140, 60)));

            resolution = window.GetResolution();
            copyright = new Text("COPYRIGHT WBRT:TS 2024", 1, new Point(resolution.Width/2-120, resolution.Height - 5f));
            window.Instantiate(copyright);

            gameOver = new Text("GAME OVER", 2f, new Point(resolution.Width / 2, resolution.Height / 2));
            window.Instantiate(gameOver);

            newHighscore = new Text("your highscore is among the best scores", 2f, new Point(resolution.Width / 2, resolution.Height / 2));
            window.Instantiate(newHighscore);

            higestscore = new Text("00", 1.5f, new Point(resolution.Width / 2 - 19, 10));
            window.Instantiate(higestscore);

            scoreboard = new Scoreboard();
            window.Instantiate(scoreboard);
            
            ChangeScreen(Screen.MainMenu);

            return new Setup()
            {
                Name = "UIManager"
            };
        }

        public void RefreshUI()
        {
            scoreText.SetText(GameManager.Instance.Score == 0 ? "00" : GameManager.Instance.Score.ToString());
        }

        public override void Update(float delta)
        {
            resolution = window.GetResolution();

            copyright.SetPosition(new Point(resolution.Width / 2-120, resolution.Height - 30));
            RefreshHighestScore();
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }

        public void ChangeScreen(Screen screen)
        {
            gameOver.IsEnabled = false;
            newHighscore.IsEnabled = false;
            scoreboard.ShowScoreboard = false;

            switch (screen)
            {
                case Screen.MainMenu:
                    RefreshHighestScore();
                    scoreboard.ShowScoreboard = true;
                    scoreboard.Refresh();
                    break;
                case Screen.Game:
                    break;
                case Screen.GameOver:
                    gameOver.IsEnabled = true;
                    break;
                case Screen.Highscore:
                    newHighscore.IsEnabled = true;
                    break;
            }
        }

        void RefreshHighestScore()
        {
            var scores = HighscoreManager.GetScores();
            string higiestScore = "0";

            if (scores.Length != 0)
                higiestScore = HighscoreManager.GetScores()[0].Score.ToString();

            if (higiestScore == "0")
                higiestScore = "00";

            higestscore.SetText(higiestScore);
            higestscore.SetPosition(new Point(resolution.Width / 2 - (9.5f * higiestScore.Length), 10));
        }
    }
}
