using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Tempest.Objects
{
	public class Bullet : PhysicsObject
	{
		public override int PhysicsLayer => mapPosition;
		int mapPosition = 0; //prostokąt na którym jest gracz

		const float zSpeed = 750f;
		const float maxLength = 1600f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.Name == "Obstacle")
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
				Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z),
				Rotation = Point.Zero
			};
		}

		public void Setup(int mapPosition, float zPosition)
		{
			this.mapPosition = mapPosition;
			transform.Position.Z = zPosition;
			transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
		}

		public override void Update(float delta)
		{
			transform.Position.Z += zSpeed * delta;

			if (transform.Position.Z > maxLength)
				window.Destroy(this);
		}
	}
}
