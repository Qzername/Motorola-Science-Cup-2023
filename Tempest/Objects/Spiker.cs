using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class Spiker : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;
		private readonly Timer _bulletTimer = new();
		private SpikerLine _spikerLine = new();

		private bool _turnAround;
		private const float ZSpeed = 350f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.Instance.StopGame)
				return;

			if (other.Name == "Bullet" && !_turnAround)
			{
				_turnAround = true;
				_spikerLine.IsDead = true;
			}
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.Instance.Rand.Next(0, MapManager.Instance.Elements.Count);

			if (transform.Position.Z == 0)
				transform.Position.Z = GameManager.Instance.LevelConfig.Length;

			_bulletTimer.Elapsed += TimerShoot;
			_bulletTimer.Interval = GameManager.Instance.Rand.Next(1000, 3001); // Strzelaj co 1 do 3 sekund
			_bulletTimer.Enabled = true;

			_spikerLine.Setup(_mapPosition);
			window.Instantiate(_spikerLine);

			return new Setup()
			{
				Name = "Spiker",
				Shape = new PointShape(GameManager.Instance.LevelConfig.Spiker,
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
			if (GameManager.Instance.StopGame)
				return;

			if (!_turnAround)
			{
				transform.Position.Z -= ZSpeed * delta;
				_spikerLine.Length += ZSpeed * delta;

				if (transform.Position.Z < 400)
				{
					Tanker tanker = new();
					tanker.Setup(_mapPosition);
					window.Instantiate(tanker);
					Die(false);
				}
			}
			else
			{
				transform.Position.Z += ZSpeed * delta;

				if (transform.Position.Z > 1600)
					Die(true);
			}

			if (!IsDead)
				_spikerLine.Shape = new PointShape(GameManager.Instance.LevelConfig.Spiker,
				new Point(0, 0, 0),
				new Point(0, 0, _spikerLine.Length));
		}

		void TimerShoot(object? sender, ElapsedEventArgs e)
		{
			if (GameManager.Instance.StopGame)
				return;

			_bulletTimer.Interval = GameManager.Instance.Rand.Next(1000, 3001); // Strzelaj co 1 do 3 sekund

			var bullet = new Spike();
			bullet.Setup(_mapPosition, transform.Position.Z);
			window.Instantiate(bullet);
		}

		void Die(bool killedByPlayer)
		{
			if (IsDead)
				return;

			if (killedByPlayer)
				GameManager.Instance.Score += 50;

			EnemyManager.Instance.EnemyDestroyed(this);
			IsDead = true;
			_spikerLine.IsDead = true;

			window.Destroy(this);
		}

		public override void OnDestroy()
		{
			_bulletTimer.Close();
			_bulletTimer.Enabled = false;
		}
	}
}
