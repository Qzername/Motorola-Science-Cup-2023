﻿using SkiaSharp;
using System.Diagnostics;
using System.Timers;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Tempest.Objects
{
    public class Tanker : PhysicsObject
    {
	    public override int PhysicsLayer => GameManager.MapPosition;
	    private int mapPosition;
	    System.Timers.Timer bulletTimer = new();

	    private const float zSpeed = 200f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != mapPosition)
				return;

			if (other.Name == "Bullet")
				Split();
			else if (other.Name == "Player")
				return;
				// Zrob funkcje w GameManager lub GameWindow, ktora restartuje poziom
		}

        public override Setup Start()
        {
            mapPosition = GameManager.MapPosition;

			bulletTimer.Elapsed += TimerShoot;
			bulletTimer.Interval = 2000; // Strzelaj co 2 sekundy
			bulletTimer.Enabled = true;

			return new Setup()
            {
                Name = "Tanker",
                Shape = new PointShape(GameManager.Configuration.Tanker,
                                new Point(MapManager.Instance.Elements[mapPosition].Length / 2, -10,0),
								new Point(MapManager.Instance.Elements[mapPosition].Length / 2, 10, 0),
								new Point(MapManager.Instance.Elements[mapPosition].Length / -2, -10,0),
								new Point(MapManager.Instance.Elements[mapPosition].Length / -2, 10, 0)),
                Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z) + new Point(0,0,1600),
                Rotation = MapManager.Instance.Elements[mapPosition].Transform.Rotation
            };
        }

        public override void Update(float delta)
        {
	        if (transform.Position.Z > 400)
				transform.Position.Z -= zSpeed * delta;
	        else
				Split();
		}

		void TimerShoot(object? sender, ElapsedEventArgs e)
		{
			var bullet = new BulletTanker();
			window.Instantiate(bullet);
			bullet.Setup(mapPosition, transform.Position.Z);
		}

		void Split()
		{
			Destroy();

			Flipper flipper1 = new Flipper();
			flipper1.Setup(mapPosition + 1, transform.Position.Z);
			Flipper flipper2 = new Flipper();
			flipper2.Setup(mapPosition - 1, transform.Position.Z);
			window.Instantiate(flipper1);
			window.Instantiate(flipper2);
		}

		void Destroy()
		{
			bulletTimer.Enabled = false;
			window.Destroy(this);
		}
	}
}
