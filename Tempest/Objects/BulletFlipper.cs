using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Tempest.Objects
{
	public class BulletFlipper : PhysicsObject
	{
		public override int PhysicsLayer => mapPosition;
		private int mapPosition;

		const float zSpeed = 250f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.Name == "Bullet")
				window.Destroy(this);
			else if (other.Name == "Player")
				return;
				// Zrob funkcje w GameManager lub GameWindow, ktora restartuje poziom
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "BulletFlipper",
				Shape = new PointShape(GameManager.Configuration.Flipper,
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
			transform.Position.Z -= zSpeed * delta;

			if (transform.Position.Z < 400)
				window.Destroy(this);
		}
	}
}
