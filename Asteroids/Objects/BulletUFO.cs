using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
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
                Shape = new Shape(0f, new SKPoint(0, 0), new SKPoint(5, 0)),
                Position = new SKPoint(0,0),
                Rotation = 0f,
            };
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name == "Player")
				window.Destroy(this);
        }
    }
}
