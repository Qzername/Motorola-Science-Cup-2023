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
    public class Flipper : PhysicsObject
    {
	    public override int PhysicsLayer => GameManager.MapPosition;
	    private int mapPosition = -1;
	    System.Timers.Timer bulletTimer = new();
	    System.Timers.Timer moveTimer = new();
	    private int mapChangeCount;

	    private bool atTheEnd;
	    private int chance = 4;
	    private const float zSpeed = 150f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != mapPosition)
				return;

			if (other.Name == "Bullet")
				Destroy();
			else if (other.Name == "Player")
				return;
				// Zrob funkcje w GameManager lub GameWindow, ktora restartuje poziom
		}

        public override Setup Start()
        {
			if (mapPosition == -1)
				mapPosition = GameManager.MapPosition;

			transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);

			bulletTimer.Elapsed += TimerShoot;
			bulletTimer.Interval = 2000; // Strzelaj co 2 sekundy
			bulletTimer.Enabled = true;
			
            moveTimer.Elapsed += TimerMove;
            moveTimer.Interval = 1000;
            moveTimer.Enabled = true;

			return new Setup()
            {
                Name = "Flipper",
                Shape = new PointShape(GameManager.Configuration.Flipper,
                                new Point(MapManager.Instance.Elements[mapPosition].Length / 2, -10,0),
								new Point(MapManager.Instance.Elements[mapPosition].Length / 2, 10, 0),
								new Point(MapManager.Instance.Elements[mapPosition].Length / -2, -10,0),
								new Point(MapManager.Instance.Elements[mapPosition].Length / -2, 10, 0)),
                Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z),
                Rotation = MapManager.Instance.Elements[mapPosition].Transform.Rotation
            };
        }

		public void Setup(int mapPosition, float zPosition = 1600)
		{
			this.mapPosition = mapPosition;
			transform.Position.Z = zPosition;
		}

		public override void Update(float delta)
        {
	        if (transform.Position.Z > 400)
				transform.Position.Z -= zSpeed * delta;
	        else if (!atTheEnd)
	        {
		        atTheEnd = true;
		        chance = 0;
		        bulletTimer.Enabled = false;
				moveTimer.Interval = 500;
	        }
		}

        void TimerMove(object? snder, ElapsedEventArgs e)
        {
			// 25% szansy na ruch w lewo lub prawo
	        int random = GameManager.Rand.Next(0, chance);
	        if (random == 0)
	        {
		        int side = GameManager.Rand.Next(0, 2);
				
		        if (side == 0)
		        {
			        if (mapPosition != 0)
			        {
				        mapPosition--;
				        transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
			        }
			        else if (GameManager.IsLevelClosed)
			        {
				        mapPosition = MapManager.Instance.Elements.Count - 1;
				        transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
			        }
		        }
		        else
		        {
			        if (mapPosition != MapManager.Instance.Elements.Count - 1)
			        {
				        mapPosition++;
				        transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
			        }
			        else if (GameManager.IsLevelClosed)
			        {
				        mapPosition = 0;
				        transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
			        }
		        }

				Shape = new PointShape(GameManager.Configuration.Flipper,
					new Point(MapManager.Instance.Elements[mapPosition].Length / 2, -10, 0),
					new Point(MapManager.Instance.Elements[mapPosition].Length / 2, 10, 0),
					new Point(MapManager.Instance.Elements[mapPosition].Length / -2, -10, 0),
					new Point(MapManager.Instance.Elements[mapPosition].Length / -2, 10, 0));
				Rotate(MapManager.Instance.Elements[mapPosition].Transform.Rotation);

				if (atTheEnd)
				{
					mapChangeCount++;

					if (mapChangeCount > 16)
						Destroy();					
				}
	        }
		}

		void TimerShoot(object? sender, ElapsedEventArgs e)
		{
			var bullet = new BulletFlipper();
			window.Instantiate(bullet);
			bullet.Setup(mapPosition, transform.Position.Z - 50);
		}

		void Destroy()
		{
			bulletTimer.Enabled = false;
			moveTimer.Enabled = false;
			window.Destroy(this);
		}
	}
}
