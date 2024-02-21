using System.Timers;
using Tempest.Objects;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
	internal class GameWindow : Window
	{
		System.Timers.Timer spawnEnemyTimer = new();

		public GameWindow() : base(new TempestScene())
		{
			GameManager.Configuration = new();
			RegisterPhysicsEngine(new TempestPhysicsEngine());

			Instantiate(new MapManager());
			Instantiate(new Player());

			spawnEnemyTimer.Elapsed += TimerSpawnEnemy;
			spawnEnemyTimer.Interval = 1000; // Tworz wroga co sekunde
			spawnEnemyTimer.Enabled = true;
		}

		void TimerSpawnEnemy(object? sender, ElapsedEventArgs e)
		{

		}

		public void RestartLevel()
		{
			GameManager.MapPosition = 0;
			DestroyAll();

			Instantiate(new MapManager());
			Instantiate(new Player());
		}

		public override void Update(Canvas canvas)
		{
			if (KeyDown(Key.P))
			{
				GameManager.NextLevel();
				RestartLevel();
			}
		}
	}
}
