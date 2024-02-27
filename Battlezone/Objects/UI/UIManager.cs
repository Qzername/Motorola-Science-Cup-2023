using HML.Objects;
using SkiaSharp;
using System.Windows.Automation;
using VGE;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Battlezone.Objects.UI
{
	public class UIManager : VectorObject
	{
		public static UIManager Instance;

		Text score, gameOver, newHighscore, startInfo;
		WeaponScope reloading;
		LiveCounter lives;
		Radar radar;

		Scoreboard scoreboard;

		Resolution currentResolution;

		public override Setup Start()
		{
			Instance = this;

			score = new Text("Score: 0000", 2, new SKPoint(10, 42));
			window.Instantiate(score);

			lives = new LiveCounter();
			window.Instantiate(lives);

			reloading = new WeaponScope();
			window.Instantiate(reloading);

			radar = new Radar();
			window.Instantiate(radar);

			scoreboard = new Scoreboard();
			window.Instantiate(scoreboard);
			
			gameOver = new Text("Game over", 2f, new Point(400,225));
			window.Instantiate(gameOver);

			var resolution = window.GetResolution();

            startInfo = new Text("PRESS SPACE TO START", 2, new Point(resolution.Width / 2 - 212, resolution.Height - 65f));
            window.Instantiate(startInfo);

            newHighscore = new Text("your score is\namong the best scores\nWrite your name\nUse W and S to choose letter\nSpace to confirm letter", 2f, new Point(resolution.Width / 2 - 288f, resolution.Height / 2 - 120f));
            window.Instantiate(newHighscore);

            return new()
			{
				Name = "UIManager"
			};
		}

		public override void Update(float delta)
		{
			var resolution = window.GetResolution();

			if (resolution != currentResolution)
			{
				currentResolution = resolution;
				RefreshUI();
			}
		}

		/// <summary>
		/// Odświeżenie UI, wszystkich statystyk
		/// </summary>
		public void RefreshUI()
		{
			score.SetText($"Score      {GameManager.Instance.Score}");

			reloading.IsReloading = GameManager.Instance.IsReloading;

			//ustawianie pozycji
			score.SetPosition(new Point(currentResolution.Width * 0.75f-104, score.Transform.Position.Y));
			lives.SetPosition(new Point(currentResolution.Width * 0.75f-104, lives.Transform.Position.Y));
			gameOver.SetPosition(new Point(currentResolution.Width * .5f-85, currentResolution.Height * .5f));
            newHighscore.SetPosition(new Point(currentResolution.Width * .5f - 288f, currentResolution.Height * .5f - 120f));
            startInfo.SetPosition(new Point(currentResolution.Width / 2 - 212, currentResolution.Height - 65f));
        }

        public void ChangeUIStatus(Screen screen)
		{
			//main menu
			scoreboard.ShowScoreboard = false;
            startInfo.IsEnabled = false;

            //game
            score.IsEnabled = false;
			lives.IsEnabled = false;
			radar.IsEnabled = false;
			reloading.IsEnabled = false;

			//game over
			gameOver.IsEnabled = false;

			//highscore
			newHighscore.IsEnabled = false;

			switch(screen)
			{
				case Screen.MainMenu:
					scoreboard.Refresh();
                    scoreboard.ShowScoreboard = true;

					startInfo.IsEnabled = true;
					break;
				case Screen.Game:
					score.IsEnabled = true;
					lives.IsEnabled = true;
					radar.IsEnabled = true;
                    reloading.IsEnabled = true;
                    break;
				case Screen.GameOver:
					gameOver.IsEnabled = true;
					break;
				case Screen.HighScore:
					newHighscore.IsEnabled = true;
					break;
			}

            RefreshUI();
		}

		public override bool OverrideRender(Canvas canvas)
		{
			return true;
		}
	}
}
