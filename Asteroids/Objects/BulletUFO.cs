using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Asteroids.Objects
{
    public class BulletUFO : Bullet
    {
		public override int PhysicsLayer => (int)PhysicsLayers.Other;

        public override Setup Start()
        {
			Speed = 350f;
            
			return new Setup()
            {
                Name = "BulletUFO",
                Shape = new PointShape(Point.Zero, new Point(0, 0), new Point(7, 0)),
                Position = new Point(0,0),
                Rotation = Point.Zero,
            };
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name == "Player")
				window.Destroy(this);
        }
    }
}
