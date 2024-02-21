using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class Fuseball : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition)
				return;

			if (other.Name == "Bullet")
				window.Destroy(this);
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.MapPosition;

			return new Setup()
			{
				Name = "Fuseball",
				Shape = new PointShape(GameManager.Configuration.Fuseball,
								new Point(0, 20, 0),
								new Point(20, 0, 0),
								new Point(0, -20, 0),
								new Point(-20, 0, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z) + new Point(0, 0, 1600),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition)
		{
			_mapPosition = mapPosition;
		}

		public override void Update(float delta)
		{
		}
	}
}
