using System.Diagnostics;
using System.Timers;
using VGE;
using VGE.Graphics;
using VGE.Physics;
using VGE.Windows;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class EnemyManager : VectorObject
	{
		public static EnemyManager Instance;

		private readonly Timer _checkEnemiesTimer = new();
		private List<VectorObject> _enemies = new();
		private List<VectorObject> _flippersAtTheEnd = new();
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

		public async void SpawnEnemies()
		{
			GameManager.Instance.SpawningEnemies = true;
			_checkEnemiesTimer.Close();
			_checkEnemiesTimer.Enabled = false;
			int enemiesToSpawn = GameManager.Instance.EnemiesToSpawn;

			for (int i = 0; i < enemiesToSpawn; i++)
			{
				if (GameManager.Instance.StopGame)
					return;

				Enemies enemyChoice = (Enemies)GameManager.Instance.Rand.Next(0, Enum.GetNames(typeof(Enemies)).Length);

				VectorObject enemy = new Flipper();

				switch (enemyChoice)
				{
					case Enemies.Tanker:
						if (GameManager.Instance.LevelConfig.SpawnTanker)
							enemy = new Tanker();

						break;
					case Enemies.Spiker:
						if (GameManager.Instance.LevelConfig.SpawnSpiker)
							enemy = new Spiker();

						break;
					case Enemies.Fuseball:
						if (GameManager.Instance.LevelConfig.SpawnFuseball)
							enemy = new Fuseball();

						break;
				}

				_enemies.Add(enemy);
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

			_enemies.Add(flipper1);
			window.Instantiate(flipper1);

			_enemies.Add(flipper2);
			window.Instantiate(flipper2);
		}

		public override bool OverrideRender(Canvas canvas) => true;

		public override void Update(float delta)
		{
			if (window.KeyDown(Key.P) && !GameManager.Instance.ChangingMap)
				GameManager.Instance.ChangingMap = true;
			else if (!window.KeyDown(Key.P) && GameManager.Instance.ChangingMap)
			{
				GameManager.Instance.ChangingMap = false;

				GameManager.Instance.NextLevel();
				GameManager.Instance.StartLevel();
			}
		}

		public void DestroyEnemies(bool destroySpikes = false)
		{
			Debug.WriteLine(_enemies.Count);

			foreach (VectorObject enemy in _enemies.ToArray())
				window.Destroy(enemy);

			_enemies.Clear();

			foreach (VectorObject enemy in _flippersAtTheEnd.ToArray())
				window.Destroy(enemy);

			_flippersAtTheEnd.Clear();

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
					_flippersAtTheEnd.Add(enemy);

			_enemies.Remove(enemy);
		}

		void TimerCheckEnemies(object? sender, ElapsedEventArgs e)
		{
			if (_enemies.Count <= 0 && !GameManager.Instance.SpawningEnemies && !GameManager.Instance.StopGame)
			{
				GameManager.Instance.NextLevel();
				GameManager.Instance.StartLevel();
			}
		}
	}
}
