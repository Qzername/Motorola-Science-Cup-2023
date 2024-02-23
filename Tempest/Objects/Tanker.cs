using System.Timers;
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

		// Strzelaj co 1 do 2 sekund
		private const int MinShootDelay = 1000;
		private const int MaxShootDelay = 2001;

		private const float ZSpeed = 200f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.StopGame)
				return;

			if (other.Name == "Bullet")
				Split();
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.Rand.Next(0, MapManager.Instance.Elements.Count);
			
			if (transform.Position.Z == 0)
				transform.Position.Z = GameManager.LevelConfig.Length;

			_bulletTimer.Elapsed += TimerShoot;
			_bulletTimer.Interval = GameManager.Rand.Next(MinShootDelay, MaxShootDelay); 
			_bulletTimer.Enabled = true;

			return new Setup()
			{
				Name = "Tanker",
				Shape = new PointShape(GameManager.LevelConfig.Tanker,
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
			if (GameManager.StopGame)
				return;

			if (transform.Position.Z > 400)
				transform.Position.Z -= ZSpeed * delta;
			else
				Split();
		}

		void TimerShoot(object? sender, ElapsedEventArgs e)
		{
			if (GameManager.StopGame)
				return;

			_bulletTimer.Interval = GameManager.Rand.Next(MinShootDelay, MaxShootDelay);

			var bullet = new BulletTanker();
			bullet.Setup(_mapPosition, transform.Position.Z);			
			window.Instantiate(bullet);
		}

		void Split()
		{
			Flipper flipper1 = new Flipper();

			int flipper1MapPosition = _mapPosition;
			if (_mapPosition < MapManager.Instance.Elements.Count - 1)
				flipper1MapPosition += 1;

			flipper1.Setup(flipper1MapPosition, transform.Position.Z);
			GameManager.EnemiesOnScreen++;

			Flipper flipper2 = new Flipper();

			int flipper2MapPosition = _mapPosition;
			if (_mapPosition > 0)
				flipper2MapPosition -= 1;

			flipper2.Setup(flipper2MapPosition, transform.Position.Z);
			GameManager.EnemiesOnScreen++;

			window.Instantiate(flipper1);
			window.Instantiate(flipper2);

			Die();
		}

		void Die()
		{
			if (IsDead)
				return;
			
			((GameWindow)window).EnemyDestroyed(this);
			IsDead = true;

			window.Destroy(this);
		}

		public override void OnDestroy()
		{
			_bulletTimer.Close();
			_bulletTimer.Enabled = false;
		}
	}
}
