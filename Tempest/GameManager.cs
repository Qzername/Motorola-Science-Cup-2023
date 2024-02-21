using VGE;

namespace Tempest
{
	public static class GameManager
	{
		private static Configuration _configuration = new();
		public static Configuration Configuration
		{
			get => _configuration;
			set => _configuration = value;
		}

		private static bool _isLevelClosed;
		public static bool IsLevelClosed
		{
			get => _isLevelClosed;
			set => _isLevelClosed = value;
		}

		private static int _mapPosition;
		public static int MapPosition
		{
			get => _mapPosition;
			set => _mapPosition = value;
		}

		private static bool _flipperSpawn = true;
		public static bool FlipperSpawn
		{
			get => _flipperSpawn;
			set => _flipperSpawn = value;
		}

		private static bool _tankerSpawn = true;
		public static bool TankerSpawn
		{
			get => _tankerSpawn;
			set => _tankerSpawn = value;
		}

		private static bool _spikerSpawn;
		public static bool SpikerSpawn
		{
			get => _spikerSpawn;
			set => _spikerSpawn = value;
		}

		private static bool _fuseballSpawn;
		public static bool FuseballSpawn
		{
			get => _fuseballSpawn;
			set => _fuseballSpawn = value;
		}

		private static Point[] currentLevel = Levels.Infinity;
		public static Point[] CurrentLevel
		{
			get => currentLevel;
			set => currentLevel = value;
		}

		public static Random Rand = new();

		public static void NextLevel()
		{
			if (currentLevel == Levels.Circle)
				currentLevel = Levels.Square;
			else if (currentLevel == Levels.Square)
				currentLevel = Levels.Plus;
			else if (currentLevel == Levels.Plus)
				currentLevel = Levels.BowTie;
			else if (currentLevel == Levels.BowTie)
				currentLevel = Levels.StylizedCross;
			else if (currentLevel == Levels.StylizedCross)
				currentLevel = Levels.Triangle;
			else if (currentLevel == Levels.Triangle)
				currentLevel = Levels.Clover;
			else if (currentLevel == Levels.Clover)
				currentLevel = Levels.V;
			else if (currentLevel == Levels.V)
				currentLevel = Levels.Steps;
			else if (currentLevel == Levels.Steps)
				currentLevel = Levels.U;
			else if (currentLevel == Levels.U)
				currentLevel = Levels.CompletelyFlat;
			else if (currentLevel == Levels.CompletelyFlat)
				currentLevel = Levels.Heart;
			else if (currentLevel == Levels.Heart)
				currentLevel = Levels.Star;
			else if (currentLevel == Levels.Star)
				currentLevel = Levels.W;
			else if (currentLevel == Levels.W)
				currentLevel = Levels.Fan;
			else if (currentLevel == Levels.Fan)
				currentLevel = Levels.Infinity;
			else if (currentLevel == Levels.Infinity)
				currentLevel = Levels.Circle;
		}
	}
}