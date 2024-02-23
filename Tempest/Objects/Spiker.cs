using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class Spiker : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;
		private SpikerLine _spikerLine = new();

		private bool _turnAround;
		private const float ZSpeed = 350f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.StopGame)
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
				_mapPosition = GameManager.Rand.Next(0, MapManager.Instance.Elements.Count);

			_spikerLine.Setup(_mapPosition);
			window.Instantiate(_spikerLine);

			return new Setup()
			{
				Name = "Spiker",
				Shape = new PointShape(GameManager.LevelConfig.Spiker,
								new Point(0, 20, 0),
								new Point(20, 0, 0),
								new Point(0, -20, 0),
								new Point(-20, 0, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z) + new Point(0, 0, GameManager.LevelConfig.Length),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition)
		{
			_mapPosition = mapPosition;
		}

		public override void Update(float delta)
		{
			if (GameManager.StopGame)
				return;

			if (GameManager.StopGame)
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
					Die();
				}
			}
			else
			{
				transform.Position.Z += ZSpeed * delta;

				if (transform.Position.Z > 1600)
					Die();
			}

			_spikerLine.Shape = new PointShape(GameManager.LevelConfig.Spiker,
				new Point(0, 0, 0),
				new Point(0, 0, _spikerLine.Length));
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
		}
	}
}
