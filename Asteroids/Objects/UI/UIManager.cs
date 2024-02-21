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

        Text scoreText, copyright;
        Text mainMenu, gameOver;

        public override Setup Start()
        {
            Instance = this;

            scoreText = new Text("0", 2, new SKPoint(200, 10), Text.TextAlignment.Right);
            window.Instantiate(scoreText);

            window.Instantiate(new LifeCounter(new Point(140, 60)));

            resolution = window.GetResolution();
            copyright = new Text("COPYRIGHT WBRT:TS 2024", 1, new Point(resolution.Width/2-120, resolution.Height - 5f));
            window.Instantiate(copyright);

            mainMenu = new Text("MAIN MENU", 2f, new Point(resolution.Width / 2, resolution.Height / 2));
            window.Instantiate(mainMenu);

            gameOver = new Text("GAME OVER", 2f, new Point(resolution.Width / 2, resolution.Height / 2));
            window.Instantiate(gameOver);

            ChangeScreen(Screen.MainMenu);

            return new Setup()
            {
                Name = "UIManager"
            };
        }

        public void RefreshUI()
        {
            scoreText.SetText(GameManager.Instance.Score.ToString());
        }

        public override void Update(float delta)
        {
            resolution = window.GetResolution();

            copyright.SetPosition(new Point(resolution.Width / 2-120, resolution.Height - 30));
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }

        public void ChangeScreen(Screen screen)
        {
            mainMenu.IsEnabled = false;
            gameOver.IsEnabled = false;

            switch(screen)
            {
                case Screen.MainMenu:
                    mainMenu.IsEnabled = true;
                    break;
                case Screen.Game:
                    break;
                case Screen.GameOver:
                    gameOver.IsEnabled = true;
                    break;
            }
        }
    }
}
