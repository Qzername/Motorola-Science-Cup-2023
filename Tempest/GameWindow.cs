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
		private List<VectorObject> _enemies = new();

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
				Enemies enemyChoice = (Enemies)GameManager.Rand.Next(0, Enum.GetNames(typeof(Enemies)).Length);

				VectorObject enemy = new Flipper();

				switch (enemyChoice)
				{
					case Enemies.Tanker:
						if (GameManager.LevelConfig.SpawnTanker)
							enemy = new Tanker();

						break;
					case Enemies.Spiker:
						if (GameManager.LevelConfig.SpawnSpiker)
							enemy = new Spiker();

						break;
					case Enemies.Fuseball:
						if (GameManager.LevelConfig.SpawnFuseball)
							enemy = new Fuseball();

						break;
				}

				_enemies.Add(enemy);
				Instantiate(enemy);

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
			if (_enemies.Count == 0)
				return;

			VectorObject[] enemies = _enemies.ToArray();

			foreach (VectorObject enemy in enemies)
				if (enemy is Flipper || enemy is Fuseball || enemy is Spiker || enemy is Tanker)
					Destroy(enemy);
		}

		public void EnemyDestroyed(PhysicsObject enemy)
		{
			if (enemy.IsDead)
				return;

			GameManager.EnemiesOnScreen--;
			_enemies.Remove(enemy);
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
