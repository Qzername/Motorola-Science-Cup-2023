using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class Bullet : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition;

		private const float ZSpeed = 900f;
		private const float MaxLength = 1600f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition)
				return;

			string[] names = { "BulletTanker", "Flipper", "Tanker", "Spiker", "SpikerLine", "Fuseball" };

			if (names.Any(x => other.Name.Contains(x)))
				window.Destroy(this);
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "Bullet",
				Shape = new PointShape(GameManager.Configuration.Player,
								new Point(5, 5, 0),
								new Point(5, -5, 0),
								new Point(-5, -5, 0),
								new Point(-5, 5, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z),
				Rotation = Point.Zero
			};
		}

		public void Setup(int mapPosition, float zPosition)
		{
			_mapPosition = mapPosition;
			transform.Position.Z = zPosition;
			transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
		}

		public override void Update(float delta)
		{
			transform.Position.Z += ZSpeed * delta;

			if (transform.Position.Z > MaxLength)
				window.Destroy(this);
		}
	}
}
