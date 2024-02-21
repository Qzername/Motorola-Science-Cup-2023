﻿using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class Tanker : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;
		private readonly Timer _bulletTimer = new();

		private const float ZSpeed = 200f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition)
				return;

			if (other.Name == "Bullet")
				Split();
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.MapPosition;

			_bulletTimer.Elapsed += TimerShoot;
			_bulletTimer.Interval = 2000; // Strzelaj co 2 sekundy
			_bulletTimer.Enabled = true;

			return new Setup()
			{
				Name = "Tanker",
				Shape = new PointShape(GameManager.Configuration.Tanker,
								new Point(0, 20, 0),
								new Point(20, 0, 0),
								new Point(0, -20, 0),
								new Point(-20, 0, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition, float zPosition = 1600)
		{
			_mapPosition = mapPosition;
			transform.Position.Z = zPosition;
		}

		public override void Update(float delta)
		{
			if (transform.Position.Z > 400)
				transform.Position.Z -= ZSpeed * delta;
			else
				Split();
		}

		void TimerShoot(object? sender, ElapsedEventArgs e)
		{
			var bullet = new BulletTanker();
			window.Instantiate(bullet);
			bullet.Setup(_mapPosition, transform.Position.Z);
		}

		void Split()
		{
			window.Destroy(this);

			Flipper flipper1 = new Flipper();

			int flipper1MapPosition = _mapPosition;
			if (_mapPosition < MapManager.Instance.Elements.Count - 1)
				flipper1MapPosition += 1;

			flipper1.Setup(flipper1MapPosition, transform.Position.Z);

			Flipper flipper2 = new Flipper();

			int flipper2MapPosition = _mapPosition;
			if (_mapPosition > 0)
				flipper2MapPosition -= 1;

			flipper2.Setup(flipper2MapPosition, transform.Position.Z - 50);

			window.Instantiate(flipper1);
			window.Instantiate(flipper2);
		}

		public override void OnDestroy()
		{
			_bulletTimer.Close();
			_bulletTimer.Enabled = false;
		}
	}
}
