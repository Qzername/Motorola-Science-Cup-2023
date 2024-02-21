using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class Flipper : PhysicsObject
	{
		public override int PhysicsLayer => GameManager.MapPosition;
		private int _mapPosition = -1;
		private readonly Timer _bulletTimer = new();
		private readonly Timer _moveTimer = new();
		private int _mapChangeCount;

		private bool _atTheEnd;
		private int _chance = 4;
		private const float ZSpeed = 150f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition)
				return;

			if (other.Name == "Bullet")
				Destroy();
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.MapPosition;

			transform.Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z);

			_bulletTimer.Elapsed += TimerShoot;
			_bulletTimer.Interval = 2000; // Strzelaj co 2 sekundy
			_bulletTimer.Enabled = true;

			_moveTimer.Elapsed += TimerMove;
			_moveTimer.Interval = 1000;
			_moveTimer.Enabled = true;

			return new Setup()
			{
				Name = "Flipper",
				Shape = new PointShape(GameManager.Configuration.Flipper,
								new Point(MapManager.Instance.Elements[_mapPosition].Length / 2, -10, 0),
								new Point(MapManager.Instance.Elements[_mapPosition].Length / 2, 10, 0),
								new Point(MapManager.Instance.Elements[_mapPosition].Length / -2, -10, 0),
								new Point(MapManager.Instance.Elements[_mapPosition].Length / -2, 10, 0)),
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
			else if (!_atTheEnd)
			{
				_atTheEnd = true;
				_chance = 0;
				_bulletTimer.Enabled = false;
				_moveTimer.Interval = 500;
			}
		}

		void TimerMove(object? snder, ElapsedEventArgs e)
		{
			// 25% szansy na ruch w lewo lub prawo
			int random = GameManager.Rand.Next(0, _chance);
			if (random == 0)
			{
				int side = GameManager.Rand.Next(0, 2);

				if (side == 0)
				{
					if (_mapPosition != 0)
					{
						_mapPosition--;
						transform.Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z);
					}
					else if (GameManager.IsLevelClosed)
					{
						_mapPosition = MapManager.Instance.Elements.Count - 1;
						transform.Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z);
					}
				}
				else
				{
					if (_mapPosition != MapManager.Instance.Elements.Count - 1)
					{
						_mapPosition++;
						transform.Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z);
					}
					else if (GameManager.IsLevelClosed)
					{
						_mapPosition = 0;
						transform.Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z);
					}
				}

				Shape = new PointShape(GameManager.Configuration.Flipper,
					new Point(MapManager.Instance.Elements[_mapPosition].Length / 2, -10, 0),
					new Point(MapManager.Instance.Elements[_mapPosition].Length / 2, 10, 0),
					new Point(MapManager.Instance.Elements[_mapPosition].Length / -2, -10, 0),
					new Point(MapManager.Instance.Elements[_mapPosition].Length / -2, 10, 0));
				Rotate(MapManager.Instance.Elements[_mapPosition].Transform.Rotation);

				if (_atTheEnd)
				{
					_mapChangeCount++;

					if (_mapChangeCount > 16)
						Destroy();
				}
			}
		}

		void TimerShoot(object? sender, ElapsedEventArgs e)
		{
			var bullet = new BulletFlipper();
			window.Instantiate(bullet);
			bullet.Setup(_mapPosition, transform.Position.Z - 50);
		}

		void Destroy()
		{
			_bulletTimer.Enabled = false;
			_moveTimer.Enabled = false;
			window.Destroy(this);
		}
	}
}
