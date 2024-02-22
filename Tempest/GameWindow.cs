﻿using System.Timers;
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
			Spiker spiker = new();
			spiker.Setup(2);
			Instantiate(spiker);

			spawnEnemyTimer.Elapsed += TimerSpawnEnemy;
			spawnEnemyTimer.Interval = 1000; // Tworz wroga co sekunde
											 // spawnEnemyTimer.Enabled = true;
		}

		void TimerSpawnEnemy(object? sender, ElapsedEventArgs e)
		{
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
		}

		public void RestartLevel()
		{
			GameManager.MapPosition = 0;
			DestroyAll();

			Instantiate(new MapManager());
			Instantiate(new Player());
		}

		private bool _isChangingMap;

		public override void Update(Canvas canvas)
		{
			if (KeyDown(Key.P) && !_isChangingMap)
				_isChangingMap = true;
			else if (!KeyDown(Key.P) && _isChangingMap)
			{
				_isChangingMap = false;

				GameManager.NextLevel();
				RestartLevel();
			}
		}
	}
}
