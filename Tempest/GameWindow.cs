using System.Diagnostics;
using System.Timers;
using Tempest.Objects;
using VGE;
using VGE.Graphics;
using VGE.Physics;
using VGE.Windows;
using Timer = System.Timers.Timer;

namespace Tempest
{
	internal class GameWindow : Window
	{
		private readonly Timer _checkEnemiesTimer = new();

		public GameWindow() : base(new TempestScene())
		{
			GameManager.LevelConfig = new();
			RegisterPhysicsEngine(new TempestPhysicsEngine());

			StartLevel();

			_checkEnemiesTimer.Elapsed += TimerCheckEnemies;
			_checkEnemiesTimer.Interval = 2000;
		}

		async void SpawnEnemies()
		{
			GameManager.SpawningEnemies = true;
			_checkEnemiesTimer.Close();
			_checkEnemiesTimer.Enabled = false;
			int enemiesToSpawn = GameManager.EnemiesToSpawn;

			for (int i = 0; i < enemiesToSpawn; i++)
			{
				if (GameManager.StopGame)
					return;

				GameManager.EnemiesOnScreen++;
				Enemies enemy = (Enemies)GameManager.Rand.Next(0, Enum.GetNames(typeof(Enemies)).Length);

				switch (enemy)
				{
					case Enemies.Flipper:
						Instantiate(new Flipper());
						break;
					case Enemies.Tanker:
						if (GameManager.LevelConfig.SpawnTanker)
							Instantiate(new Tanker());
						else
							Instantiate(new Flipper());

						break;
					case Enemies.Spiker:
						if (GameManager.LevelConfig.SpawnSpiker)
							Instantiate(new Spiker());
						else
							Instantiate(new Flipper());

						break;
					case Enemies.Fuseball:
						if (GameManager.LevelConfig.SpawnFuseball)
							Instantiate(new Fuseball());
						else
							Instantiate(new Flipper());

						break;
				}

				await Task.Delay(GameManager.Rand.Next(500, 2001));
			}

			GameManager.SpawningEnemies = false;
			_checkEnemiesTimer.Enabled = true;
			_checkEnemiesTimer.Start();
		}

		public async void StartLevel()
		{
			GameManager.StopGame = true;

			await Task.Delay(500);
			DestroyAll();
			await Task.Delay(500);
			DestroyAll();
			// Upewnij sie, czy wszystko zostalo usuniete :)

			GameManager.SpawningEnemies = false;
			GameManager.EnemiesOnScreen = 0;
			GameManager.MapPosition = 0;

			if (GameManager.Lives <= 0)
				Close();

			Instantiate(new MapManager());
			Instantiate(new Player());

			GameManager.StopGame = false;

			await Task.Run(SpawnEnemies);
		}

		public override void Update(Canvas canvas)
		{
			if (KeyDown(Key.P) && !GameManager.ChangingMap)
				GameManager.ChangingMap = true;
			else if (!KeyDown(Key.P) && GameManager.ChangingMap)
			{
				GameManager.ChangingMap = false;

				GameManager.NextLevel();
				StartLevel();
			}
		}

		public void DestroyEnemies()
		{
			if (Objects.Count == 0)
				return;

			VectorObject[] objectsCopy = Objects.ToArray();

			foreach (VectorObject obj in objectsCopy)
				if (obj is Flipper || obj is Fuseball || obj is Spiker || obj is Tanker)
					Destroy(obj);
		}

		public void EnemyDestroyed(PhysicsObject enemy)
		{
			if (enemy.IsDead)
				return;

			GameManager.EnemiesOnScreen--;
		}

		void TimerCheckEnemies(object? sender, ElapsedEventArgs e)
		{
			if (GameManager.EnemiesOnScreen <= 0 && !GameManager.SpawningEnemies && !GameManager.StopGame)
			{
				GameManager.NextLevel();
				StartLevel();
			}			
		}
	}
}
