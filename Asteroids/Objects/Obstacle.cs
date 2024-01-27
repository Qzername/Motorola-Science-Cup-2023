using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids.Objects
{
    public class Obstacle : PhysicsObject
    {
        const float ObstacleSpeed = 0.5f;

        public override int PhysicsLayer =>(int)PhysicsLayers.Other;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Obstacle",
                Shape = new Shape(0f,
                    new SKPoint(0, 0),
                    new SKPoint(0, 30),
                    new SKPoint(30, 30),
                    new SKPoint(30, 0)),
                Position = new SKPoint(0, 0),
                Rotation = 0f
            };
        }

        public void Setup(SKPoint position, float rotation)
        {
            transform.Position = position;
            Rotate(rotation);
        }

        public override void Update(float deltaTime)
        {
            var sin = MathF.Sin(transform.RotationRadians);
            var cos = MathF.Cos(transform.RotationRadians);

            transform.Position.X += cos * ObstacleSpeed;
            transform.Position.Y += sin * ObstacleSpeed;
            
            Resolution res = window.GetResolution();

			if (transform.Position.X < 0 || transform.Position.X > res.Width || transform.Position.Y < 0 || transform.Position.Y > res.Height)
				window.Destroy(this);
		}

        public override void OnCollisionEnter(PhysicsObject other)
        {
	        if (other.Name == "Bullet")
				window.Destroy(this);                
        }
    }
}
