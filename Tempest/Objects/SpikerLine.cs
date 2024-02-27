using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class SpikerLine : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition;

		public float Length;

		private const float ZSpeed = 350f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.Instance.StopGame)
				return;

			if (other.Name == "Bullet")
			{
				Length -= 100;
				Shape = new PointShape(GameManager.Instance.LevelConfig.Spiker,
					new Point(0, 0, 0),
					new Point(0, 0, Length));
				transform.Position.Z += 100;

				if (Length <= 0)
					window.Destroy(this);
			}
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "SpikerLine",
				Shape = new PointShape(GameManager.Instance.LevelConfig.Spiker,
								new Point(0, 0, 0),
								new Point(0, 0, Length)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z) + new Point(0, 0, GameManager.Instance.LevelConfig.Length + 25),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition)
		{
			_mapPosition = mapPosition;
		}

		public override void Update(float delta)
		{
			if (GameManager.Instance.StopGame)
				return;

			if (transform.Position.Z > 400 && !IsDead)
				transform.Position.Z -= ZSpeed * delta;
		}
	}
}
