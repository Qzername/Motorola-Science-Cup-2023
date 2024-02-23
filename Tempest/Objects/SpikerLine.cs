using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class SpikerLine : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition;

		private float _length;
		public float Length
		{
			get => _length;
			set => _length = value;
		}

		private const float ZSpeed = 350f;

		private bool _isDead;
		public bool IsDead
		{
			get => _isDead;
			set => _isDead = value;
		}

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.StopGame)
				return;

			if (other.Name == "Bullet")
			{
				_length -= 100;
				Shape = new PointShape(GameManager.LevelConfig.Spiker,
					new Point(0, 0, 0),
					new Point(0, 0, _length));
				transform.Position.Z += 100;

				if (_length <= 0)
					window.Destroy(this);
			}
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "SpikerLine",
				Shape = new PointShape(GameManager.LevelConfig.Spiker,
								new Point(0, 0, 0),
								new Point(0, 0, _length)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z) + new Point(0, 0, GameManager.LevelConfig.Length + 100),
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

			if (transform.Position.Z > 400 && !_isDead)
				transform.Position.Z -= ZSpeed * delta;
		}
	}
}
