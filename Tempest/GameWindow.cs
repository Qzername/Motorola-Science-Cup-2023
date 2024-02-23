﻿using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Timers;
using Tempest.Objects;
using VGE;
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
			int enemiesToSpawn = GameManager.EnemiesToSpawn;

			for (int i = 0; i < enemiesToSpawn; i++)
			{
				if (GameManager.StopGame)
					return;

				Instantiate(new Flipper());

				/* Enemies enemy = (Enemies)GameManager.Rand.Next(0, Enum.GetNames(typeof(Enemies)).Length);

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
				} */

				await Task.Delay(GameManager.Rand.Next(500, 2001));
			}
		}

		public async void StartLevel()
		{
			GameManager.StopGame = true;

			await Task.Delay(500);
			DestroyAll();
			await Task.Delay(500);

			GameManager.EnemiesKilled = 0;
			GameManager.MapPosition = 0;

			if (GameManager.Lives <= 0)
				Close();

			Instantiate(new MapManager());
			Instantiate(new Player());

			GameManager.StopGame = false;

			await Task.Delay(1000);
			await Task.Run(SpawnEnemies);
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
				StartLevel();
			}
		}

		public void EnemyKilled(PhysicsObject enemy)
		{
			if (enemy.IsDead)
				return;

			GameManager.EnemiesKilled++;
			Debug.WriteLine($"Enemy killed: {enemy.Guid}");

			if (GameManager.EnemiesKilled >= GameManager.EnemiesToSpawn)
			{
				GameManager.NextLevel();
				StartLevel();
			}
		}
	}
}
