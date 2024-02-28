using HML;
using HML.Objects;
using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Tempest.Objects.UI
{
	public class UIManager : VectorObject
	{
		public static UIManager Instance;

		private Resolution _resolution;

		private Text _scoreText, _levelText, _zapperText, _highestScore, _copyright, _startInfo, _gameOver, _newHighScore;
		private LifeCounter _lifeCounter;
		private Scoreboard _scoreboard;

		public override Setup Start()
		{
			Instance = this;

			_scoreText = new Text("00", 2, new SKPoint(140, 10), Text.TextAlignment.Right);
			window.Instantiate(_scoreText);

			_lifeCounter = new LifeCounter(new Point(60, 60));
			window.Instantiate(_lifeCounter);

			_levelText = new Text("Level 1", 2, new SKPoint(40, 70));
			window.Instantiate(_levelText);

			_zapperText = new Text("Zapper: Yes", 2, new SKPoint(40, 100));
			window.Instantiate(_zapperText);

			_resolution = window.GetResolution();
			_copyright = new Text("COPYRIGHT WBRT:TS 2024", 1, new Point(_resolution.Width / 2f - 120, _resolution.Height - 30));
			window.Instantiate(_copyright);

			_startInfo = new Text("PRESS SPACE TO START", 2, new Point(_resolution.Width / 2f - 212, _resolution.Height - 65f));
			window.Instantiate(_startInfo);

			_gameOver = new Text("GAME OVER", 2f, new Point(_resolution.Width / 2f - 85, _resolution.Height / 2f));
			window.Instantiate(_gameOver);

			_newHighScore = new Text("your score is\namong the best scores\nWrite your name\nUse W and S to choose letter\nSpace to confirm letter", 2f, new Point(_resolution.Width / 2f - 288f, _resolution.Height / 2f - 120f));
			window.Instantiate(_newHighScore);

			_highestScore = new Text("00", 1.5f, new Point(_resolution.Width / 2f - 19, 10));
			window.Instantiate(_highestScore);

			_scoreboard = new Scoreboard();
			window.Instantiate(_scoreboard);

			ChangeScreen(Screen.MainMenu);

			return new Setup()
			{
				Name = "UIManager"
			};
		}

		public void RefreshUI()
		{
			_scoreText.SetText(GameManager.Instance.Score == 0 ? "00" : GameManager.Instance.Score.ToString());			
			_levelText.SetText($"Level {GameManager.Instance.CurrentLevelIndex}");
			_zapperText.SetText($"Zapper: {(GameManager.Instance.Player.SuperZapper.IsUsed ? "No" : "Yes")}");
		}

		public override void Update(float delta)
		{
			var currRes = window.GetResolution();

			if (currRes.Width == _resolution.Width && currRes.Height == _resolution.Height)
				return;

			_resolution = currRes;

			_copyright.SetPosition(new Point(_resolution.Width / 2f - 120, _resolution.Height - 30));
			_gameOver.SetPosition(new Point(_resolution.Width / 2f - 85, _resolution.Height / 2f));
			_startInfo.SetPosition(new Point(_resolution.Width / 2f - 212, _resolution.Height - 65f));

			_newHighScore.SetPosition(new Point(_resolution.Width / 2f - 288f, _resolution.Height / 2f - 120f));

			RefreshHighestScore();
		}

		public override bool OverrideRender(Canvas canvas)
		{
			return true;
		}

		public void ChangeScreen(Screen screen)
		{
			_scoreText.IsEnabled = false;
			_lifeCounter.IsEnabled = false;
			_levelText.IsEnabled = false;
			_zapperText.IsEnabled = false;
			_gameOver.IsEnabled = false;
			_newHighScore.IsEnabled = false;
			_scoreboard.ShowScoreboard = false;
			_startInfo.IsEnabled = false;

			switch (screen)
			{
				case Screen.MainMenu:
					RefreshHighestScore();
					_scoreboard.ShowScoreboard = true;
					_scoreboard.Refresh();
					_startInfo.IsEnabled = true;
					break;
				case Screen.Game:
					_scoreText.IsEnabled = true;
					_lifeCounter.IsEnabled = true;
					_levelText.IsEnabled = true;
					_zapperText.IsEnabled = true;
					break;
				case Screen.GameOver:
					_gameOver.IsEnabled = true;
					break;
				case Screen.Highscore:
					_newHighScore.IsEnabled = true;
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

			_highestScore.SetText(higiestScore);
			_highestScore.SetPosition(new Point(_resolution.Width / 2f - 9.5f * higiestScore.Length, 10));
		}
	}
}
