using System.Timers;
using Tempest.Objects;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
	internal class GameWindow : Window
	{
		public GameWindow() : base(new TempestScene())
		{
			GameManager.Configuration = new();
			RegisterPhysicsEngine(new TempestPhysicsEngine());

			StartLevel();
		}

		void SpawnEnemies()
		{
			int enemiesToSpawn = GameManager.Configuration.EnemiesToSpawn;

			for (int i = 0; i < enemiesToSpawn; i++)
			{
				GameManager.Configuration.EnemiesToSpawn--;
				Enemies enemy = (Enemies)GameManager.Rand.Next(0, Enum.GetNames(typeof(Enemies)).Length);

				switch (enemy)
				{
					case Enemies.Flipper:
						Instantiate(new Flipper());
						break;
					case Enemies.Tanker:
						if (GameManager.Configuration.TankerSpawn)
							Instantiate(new Tanker());
						else
							Instantiate(new Flipper());

						break;
					case Enemies.Spiker:
						if (GameManager.Configuration.SpikerSpawn)
							Instantiate(new Spiker());
						else
							Instantiate(new Flipper());

						break;
					case Enemies.Fuseball:
						if (GameManager.Configuration.FuseballSpawn)
							Instantiate(new Fuseball());
						else
							Instantiate(new Flipper());

						break;
				}

				Thread.Sleep(GameManager.Rand.Next(250, 1001));
			}
		}

		public void StartLevel(bool destroyObjects = false)
		{
			GameManager.MapPosition = 0;
			if (destroyObjects)
				DestroyAll();

			Thread.Sleep(5000);

			if (GameManager.Lives <= 0)
				Close();

			Instantiate(new MapManager());
			Instantiate(new Player());
			// SpawnEnemies();
		}

		private bool _isChangingMap;

		public override void Update(Canvas canvas)
		{
			if (KeyDown(Key.P) && !_isChangingMap)
				_isChangingMap = true;
			else if (!KeyDown(Key.P) && _isChangingMap)
			{
				_isChangingMap = false;

				// GameManager.NextLevel();
				// StartLevel();
			}
		}
	}
}
