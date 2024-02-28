using System.Diagnostics;
using System.Timers;
using VGE;
using VGE.Graphics;
using VGE.Physics;
using VGE.Windows;
using Timer = System.Timers.Timer;
using EnemiesEnum = Tempest.Enemies;

namespace Tempest.Objects
{
	public class EnemyManager : VectorObject
	{
		public static EnemyManager Instance;

		private readonly Timer _checkEnemiesTimer = new();
		public List<VectorObject> Enemies = new();
		public List<VectorObject> OtherEnemies = new();
		public List<VectorObject> Spikes = new();

		public override Setup Start()
		{
			_checkEnemiesTimer.Elapsed += TimerCheckEnemies;
			_checkEnemiesTimer.Interval = 2000;

			Instance = this;

			return new()
			{
				Name = "EnemyManager"
			};
		}

		public async Task SpawnEnemies()
		{
			GameManager.Instance.SpawningEnemies = true;
			_checkEnemiesTimer.Close();
			_checkEnemiesTimer.Enabled = false;
			int enemiesToSpawn = GameManager.Instance.EnemiesToSpawn;
			
			await Task.Delay(1000);

			for (int i = 0; i < enemiesToSpawn; i++)
			{
				if (GameManager.Instance.StopGame)
					return;

				EnemiesEnum enemyChoice = (EnemiesEnum)GameManager.Instance.Rand.Next(0, Enum.GetNames(typeof(Enemies)).Length);

				VectorObject enemy = new Flipper();

				switch (enemyChoice)
				{
					case EnemiesEnum.Tanker:
						if (GameManager.Instance.LevelConfig.SpawnTanker)
							enemy = new Tanker();

						break;
					case EnemiesEnum.Spiker:
						if (GameManager.Instance.LevelConfig.SpawnSpiker)
							enemy = new Spiker();

						break;
					case EnemiesEnum.Fuseball:
						if (GameManager.Instance.LevelConfig.SpawnFuseball)
							enemy = new Fuseball();

						break;
				}

				Enemies.Add(enemy);
				window.Instantiate(enemy);

				await Task.Delay(GameManager.Instance.Rand.Next(500, 2001));
			}

			GameManager.Instance.SpawningEnemies = false;
			_checkEnemiesTimer.Enabled = true;
			_checkEnemiesTimer.Start();
		}

		public void SplitTanker(Tanker tanker)
		{
			Flipper flipper1 = new Flipper();

			int flipper1MapPosition = tanker.PhysicsLayer;
			if (tanker.PhysicsLayer < MapManager.Instance.Elements.Count - 1)
				flipper1MapPosition += 1;

			flipper1.Setup(flipper1MapPosition, tanker.Transform.Position.Z);

			Flipper flipper2 = new Flipper();

			int flipper2MapPosition = tanker.PhysicsLayer;
			if (tanker.PhysicsLayer > 0)
				flipper2MapPosition -= 1;

			flipper2.Setup(flipper2MapPosition, tanker.Transform.Position.Z);

			if (tanker.Transform.Position.Z < 400)
			{
				OtherEnemies.Add(flipper1);
				OtherEnemies.Add(flipper2);
			}
			else
			{
				Enemies.Add(flipper1);
				Enemies.Add(flipper2);
			}

			window.Instantiate(flipper1);
			window.Instantiate(flipper2);
		}

		public override bool OverrideRender(Canvas canvas) => true;

		public override async void Update(float delta)
		{
			if (window.KeyDown(Key.P) && !GameManager.Instance.ChangingMap)
				GameManager.Instance.ChangingMap = true;
			else if (!window.KeyDown(Key.P) && GameManager.Instance.ChangingMap)
			{
				GameManager.Instance.ChangingMap = false;

				await GameManager.Instance.NextLevel();
				GameManager.Instance.StartLevel();
			}
		}

		public void DestroyEnemies(bool destroySpikes = false)
		{
			foreach (VectorObject enemy in Enemies.ToArray())
				window.Destroy(enemy);

			Enemies.Clear();

			foreach (VectorObject enemy in OtherEnemies.ToArray())
				window.Destroy(enemy);

			OtherEnemies.Clear();

			if (destroySpikes)
			{
				foreach (VectorObject spike in Spikes.ToArray())
					window.Destroy(spike);

				Spikes.Clear();
			}
		}

		public void EnemyDestroyed(PhysicsObject enemy)
		{
			if (enemy.IsDead)
				return;

			if (enemy is Flipper)
				if (((Flipper)enemy).AtTheEnd)
					OtherEnemies.Add(enemy);

			Enemies.Remove(enemy);
			SoundRegistry.Instance.Database["fire"].PlayFromStart();
		}

		async void TimerCheckEnemies(object? sender, ElapsedEventArgs e)
		{
			if (Enemies.Count <= 0 && !GameManager.Instance.SpawningEnemies && !GameManager.Instance.StopGame)
			{
				await GameManager.Instance.NextLevel();
				GameManager.Instance.StartLevel();
			}
		}
	}
}
