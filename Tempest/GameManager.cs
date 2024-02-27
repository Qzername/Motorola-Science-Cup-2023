using System.Diagnostics;
using HML;
using HML.Objects;
using Tempest.Objects;
using Tempest.Objects.UI;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
	public class GameManager : VectorObject
	{
		public static GameManager Instance;

		public Random Rand = new();

		public LevelConfiguration LevelConfig = new();
		public Point[] CurrentLevel = Levels.Circle;
		public Player Player;
		public MapManager MapManager;

		private bool _isSpacePressedOld;
		private float _gameOverTimer;
		private NameSetter _nameSetter;

		public int MapPosition;
		public int Lives = 4;

		private int _score;
		public int Score
		{
			get => _score;
			set
			{
				_score = value;
				UIManager.Instance.RefreshUI();
			}
		}

		public bool StopGame;
		public bool ChangingMap;

		public bool SpawningEnemies;
		public int EnemiesToSpawn = 5;

		private Screen _currentScreen;
		public Screen CurrentScreen
		{
			get => _currentScreen;
			set
			{
				if (UIManager.Instance is not null)
					UIManager.Instance.ChangeScreen(value);

				if (value is Screen.Game)
					StartLevel();

				_currentScreen = value;
			}
		}

		public override Setup Start()
		{
			Instance = this;
			CurrentScreen = Screen.MainMenu;

			return new()
			{
				Name = "GameManager"
			};
		}

		public override void Update(float delta)
		{
			bool isSpacePressed = window.KeyDown(Key.Space);
			bool spaceToggled = !isSpacePressed && _isSpacePressedOld;
			_isSpacePressedOld = isSpacePressed;

			if (Screen.MainMenu == CurrentScreen)
			{
				if (spaceToggled)
					CurrentScreen = Screen.Game;

				return;
			}

			if (Screen.GameOver == CurrentScreen)
			{
				_gameOverTimer += delta;

				if (_gameOverTimer < 3)
					return;

				if (Score != 0 && HighscoreManager.IsNewHighscore(Score))
				{
					CurrentScreen = Screen.Highscore;

					_nameSetter = new NameSetter(FinishedWritingName);
					window.Instantiate(_nameSetter);
				}
				else
				{
					CurrentScreen = Screen.MainMenu;
					Score = 0;
					Lives = 4;
				}

				return;
			}

			if (Screen.Game == CurrentScreen && Lives == 0)
			{
				StopGame = true;
				CurrentScreen = Screen.GameOver;
				_gameOverTimer = 0f;

				window.Destroy(Player);
				Player = null;
				window.Destroy(MapManager);
				MapManager = null;
			}
		}

		public void FinishedWritingName(string name)
		{
			HighscoreManager.SetScore(new Highscore()
			{
				Name = name,
				Score = Score
			});

			CurrentScreen = Screen.MainMenu;
			Score = 0;
			Lives = 4;
		}

		public override bool OverrideRender(Canvas canvas) => true;

		public async void StartLevel()
		{
			StopGame = true;

			await Task.Delay(1000);
			DestroyEverything();

			SpawningEnemies = false;
			MapPosition = 0;

			MapManager = new MapManager();
			window.Instantiate(MapManager);
			Player = new Player();
			window.Instantiate(Player);

			StopGame = false;

			await Task.Run(EnemyManager.Instance.SpawnEnemies);
		}

		private void DestroyEverything()
		{
			EnemyManager.Instance.DestroyEnemies(true);
			window.Destroy(Player);
			Player = null;
			window.Destroy(MapManager);
			MapManager = null;
		}

		public void NextLevel()
		{
			EnemiesToSpawn += 3;

			if (CurrentLevel == Levels.Circle) // Square
			{
				LevelConfig.IsClosed = true;
				LevelConfig.MoveFlipper = true;
				CurrentLevel = Levels.Square;
			}
			else if (CurrentLevel == Levels.Square) // Plus
			{
				LevelConfig.IsClosed = true;
				CurrentLevel = Levels.Plus;
			}
			else if (CurrentLevel == Levels.Plus) // BowTie
			{
				LevelConfig.IsClosed = true;
				LevelConfig.SpawnSpiker = true;
				CurrentLevel = Levels.BowTie;
			}
			else if (CurrentLevel == Levels.BowTie) // StylizedCross
			{
				LevelConfig.IsClosed = true;
				CurrentLevel = Levels.StylizedCross;
			}
			else if (CurrentLevel == Levels.StylizedCross) // Triangle
			{
				LevelConfig.IsClosed = true;
				CurrentLevel = Levels.Triangle;
			}
			else if (CurrentLevel == Levels.Triangle) // Clover
			{
				LevelConfig.IsClosed = true;
				CurrentLevel = Levels.Clover;
			}
			else if (CurrentLevel == Levels.Clover) // V
			{
				LevelConfig.IsClosed = false;
				CurrentLevel = Levels.V;
			}
			else if (CurrentLevel == Levels.V) // Steps
			{
				LevelConfig.IsClosed = false;
				CurrentLevel = Levels.Steps;
			}
			else if (CurrentLevel == Levels.Steps) // U
			{
				LevelConfig.IsClosed = false;
				CurrentLevel = Levels.U;
			}
			else if (CurrentLevel == Levels.U) // CompletelyFlat
			{
				LevelConfig.IsClosed = false;
				LevelConfig.SpawnFuseball = true;
				CurrentLevel = Levels.CompletelyFlat;
			}
			else if (CurrentLevel == Levels.CompletelyFlat) // Heart
			{
				LevelConfig.IsClosed = true;
				CurrentLevel = Levels.Heart;
			}
			else if (CurrentLevel == Levels.Heart) // Star
			{
				LevelConfig.IsClosed = true;
				CurrentLevel = Levels.Star;
			}
			else if (CurrentLevel == Levels.Star) // W
			{
				LevelConfig.IsClosed = false;
				CurrentLevel = Levels.W;
			}
			else if (CurrentLevel == Levels.W) // Fan
			{
				LevelConfig.IsClosed = false;
				CurrentLevel = Levels.Fan;
			}
			else if (CurrentLevel == Levels.Fan) // Infinity
			{
				LevelConfig.IsClosed = true;
				CurrentLevel = Levels.Infinity;
			}
			else if (CurrentLevel == Levels.Infinity) // Circle
			{
				LevelConfig.IsClosed = true;
				LevelConfig.ChangeColorScheme();
				CurrentLevel = Levels.Circle;
			}
		}
	}
}