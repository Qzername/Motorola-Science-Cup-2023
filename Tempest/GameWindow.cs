using System.Diagnostics;
using Tempest.Objects;
using VGE.Graphics;
using VGE.Physics;
using VGE.Windows;

namespace Tempest
{
	internal class GameWindow : Window
	{
		public GameWindow() : base(new TempestScene())
		{
			GameManager.LevelConfig = new();
			RegisterPhysicsEngine(new TempestPhysicsEngine());

			StartLevel();
		}

		async void SpawnEnemies()
		{
			GameManager.SpawningEnemies = true;
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

				await Task.Delay(GameManager.Rand.Next(500, 1001));
			}

			GameManager.SpawningEnemies = false;
		}

		public async void StartLevel()
		{
			GameManager.StopGame = true;

			await Task.Delay(500);
			DestroyAll();
			await Task.Delay(500);

			GameManager.SpawningEnemies = false;
			GameManager.EnemiesOnScreen = 0;
			GameManager.MapPosition = 0;

			if (GameManager.Lives <= 0)
				Close();

			Instantiate(new MapManager());
			Instantiate(new Player());

			GameManager.StopGame = false;

			await Task.Delay(1000);
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

		public void EnemyDestroyed(PhysicsObject enemy)
		{
			if (enemy.IsDead)
				return;

			GameManager.EnemiesOnScreen--;

			if (GameManager.EnemiesOnScreen <= 0 && !GameManager.SpawningEnemies)
			{
				GameManager.NextLevel();
				StartLevel();
			}
		}
	}
}
