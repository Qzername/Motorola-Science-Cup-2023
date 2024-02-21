using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class Tanker : PhysicsObject
	{
		public override int PhysicsLayer => GameManager.MapPosition;
		private int _mapPosition;
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
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z) + new Point(0, 0, 1600),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
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
			Destroy();

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

		void Destroy()
		{
			_bulletTimer.Enabled = false;
			window.Destroy(this);
		}
	}
}
