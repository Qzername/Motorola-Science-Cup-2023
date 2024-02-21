using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;


namespace Tempest.Objects
{
	public class Spike : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		int _mapPosition;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition)
				return;

			if (other.Name == "Bullet")
				window.Destroy(this);
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "Obstacle",
				Shape = new PointShape(GameManager.Configuration.Spike,
								new Point(-20, 0, 0),
								new Point(0, -20, 0),
								new Point(20, 0, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition, float zPosition)
		{
			this._mapPosition = mapPosition;
			transform.Position.Z = zPosition;
			transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
		}

		public override void Update(float delta)
		{
		}
	}

}
