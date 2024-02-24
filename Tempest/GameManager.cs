using SkiaSharp;
using VGE;

namespace Tempest
{
	public static class GameManager
	{
		public static LevelConfiguration LevelConfig = new();

		public static Point[] CurrentLevel = Levels.Circle;

		public static int MapPosition;		
		public static int Lives = 4;
		public static int Score;

		public static bool StopGame;
		public static bool ChangingMap;

		public static bool SpawningEnemies;
		public static int EnemiesToSpawn = 5;
		public static int EnemiesOnScreen;

		public static Random Rand = new();

		public static void NextLevel()
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