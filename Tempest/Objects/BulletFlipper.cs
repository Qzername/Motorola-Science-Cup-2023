using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class BulletFlipper : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition;

		private const float ZSpeed = 450f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.StopGame)
				return;

			if (other.Name == "Bullet")
				window.Destroy(this);
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "BulletFlipper",
				Shape = new PointShape(GameManager.LevelConfig.Flipper,
								new Point(10, 10, 0),
								new Point(10, -10, 0),
								new Point(-10, -10, 0),
								new Point(-10, 10, 0)),
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
			if (GameManager.StopGame)
				return;

			transform.Position.Z -= ZSpeed * delta;
			Rotate(new Point(0, 0, 10));

			if (transform.Position.Z < 400)
				window.Destroy(this);
		}
	}
}
