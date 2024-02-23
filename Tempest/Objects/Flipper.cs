using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class Flipper : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;
		private readonly Timer _moveTimer = new();
		private int _mapChangeCount;

		private bool _atTheEnd;
		private int _chance = 4;
		private const float ZSpeed = 300f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.StopGame)
				return;

			if (other.Name == "Bullet")
				Die();
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.Rand.Next(0, MapManager.Instance.Elements.Count);

			if (transform.Position.Z == 0)
				transform.Position.Z = GameManager.LevelConfig.Length;

			_moveTimer.Elapsed += TimerMove;
			_moveTimer.Interval = GameManager.Rand.Next(250, 2001);
			_moveTimer.Enabled = true;

			return new Setup()
			{
				Name = "Flipper",
				Shape = new PointShape(GameManager.LevelConfig.Flipper,
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
			if (GameManager.StopGame)
				return;

			if (transform.Position.Z > 400)
				transform.Position.Z -= ZSpeed * delta;
			else if (!_atTheEnd)
			{
				_atTheEnd = true;
				_chance = 0;
				_moveTimer.Interval = GameManager.Rand.Next(100, 501);
			}
		}

		void TimerMove(object? snder, ElapsedEventArgs e)
		{
			if (GameManager.StopGame)
				return;

			if (!_atTheEnd)
				_moveTimer.Interval = GameManager.Rand.Next(100, 501);
			else
				_moveTimer.Interval = GameManager.Rand.Next(250, 2001);

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
					else if (GameManager.LevelConfig.IsClosed)
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
					else if (GameManager.LevelConfig.IsClosed)
					{
						_mapPosition = 0;
						transform.Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z);
					}
				}

				Shape = new PointShape(GameManager.LevelConfig.Flipper,
					new Point(MapManager.Instance.Elements[_mapPosition].Length / 2, -10, 0),
					new Point(MapManager.Instance.Elements[_mapPosition].Length / 2, 10, 0),
					new Point(MapManager.Instance.Elements[_mapPosition].Length / -2, -10, 0),
					new Point(MapManager.Instance.Elements[_mapPosition].Length / -2, 10, 0));
				Rotate(MapManager.Instance.Elements[_mapPosition].Transform.Rotation);

				if (_atTheEnd)
				{
					_mapChangeCount++;

					if (_mapChangeCount > 8)
						Die();
				}
			}
		}

		void Die()
		{
			if (IsDead)
				return;

			((GameWindow)window).EnemyKilled(this);
			IsDead = true;

			window.Destroy(this);
		}

		public override void OnDestroy()
		{
			_moveTimer.Close();
			_moveTimer.Enabled = false;
		}
	}
}
