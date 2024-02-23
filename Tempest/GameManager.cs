using VGE;

namespace Tempest
{
	public static class GameManager
	{
		private static LevelConfiguration _levelConfig = new();
		public static LevelConfiguration LevelConfig
		{
			get => _levelConfig;
			set => _levelConfig = value;
		}

		private static int _mapPosition;
		public static int MapPosition
		{
			get => _mapPosition;
			set => _mapPosition = value;
		}

		private static Point[] _currentLevel = Levels.Circle;
		public static Point[] CurrentLevel
		{
			get => _currentLevel;
			set => _currentLevel = value;
		}

		private static int _lives = 4;
		public static int Lives
		{
			get => _lives;
			set => _lives = value;
		}

		private static bool _stopGame;
		public static bool StopGame
		{
			get => _stopGame;
			set => _stopGame = value;
		}

		private static bool _changingMap;
		public static bool ChangingMap
		{
			get => _changingMap;
			set => _changingMap = value;
		}
		
		private static bool _spawningEnemies;
		public static bool SpawningEnemies
		{
			get => _spawningEnemies;
			set => _spawningEnemies = value;
		}

		private static int _enemiesToSpawn = 5;
		public static int EnemiesToSpawn
		{
			get => _enemiesToSpawn;
			set => _enemiesToSpawn = value;
		}

		private static int _enemiesOnScreen;
		public static int EnemiesOnScreen
		{
			get => _enemiesOnScreen;
			set => _enemiesOnScreen = value;
		}

		public static Random Rand = new();

		public static void NextLevel()
		{
			_enemiesToSpawn += 5;

			if (_currentLevel == Levels.Circle)
			{
				LevelConfig.MoveFlipper = true;
				_currentLevel = Levels.Square;
			}
			else if (_currentLevel == Levels.Square)
				_currentLevel = Levels.Plus;
			else if (_currentLevel == Levels.Plus)
			{
				LevelConfig.SpawnSpiker = true;
				_currentLevel = Levels.BowTie;				
			}
			else if (_currentLevel == Levels.BowTie)
				_currentLevel = Levels.StylizedCross;
			else if (_currentLevel == Levels.StylizedCross)
				_currentLevel = Levels.Triangle;
			else if (_currentLevel == Levels.Triangle)
				_currentLevel = Levels.Clover;
			else if (_currentLevel == Levels.Clover)
				_currentLevel = Levels.V;
			else if (_currentLevel == Levels.V)
				_currentLevel = Levels.Steps;
			else if (_currentLevel == Levels.Steps)
				_currentLevel = Levels.U;
			else if (_currentLevel == Levels.U)
				_currentLevel = Levels.CompletelyFlat;
			else if (_currentLevel == Levels.CompletelyFlat)
				_currentLevel = Levels.Heart;
			else if (_currentLevel == Levels.Heart)
				_currentLevel = Levels.Star;
			else if (_currentLevel == Levels.Star)
				_currentLevel = Levels.W;
			else if (_currentLevel == Levels.W)
				_currentLevel = Levels.Fan;
			else if (_currentLevel == Levels.Fan)
				_currentLevel = Levels.Infinity;
			else if (_currentLevel == Levels.Infinity)
				_currentLevel = Levels.Circle;
		}
	}
}