using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids.Objects
{
    public class Bullet : PhysicsObject
    {
		public const float Speed = 500f;

        public override int PhysicsLayer => (int)PhysicsLayers.Player;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Bullet",
                Shape = new Shape(0f, new SKPoint(0, 0), new SKPoint(5, 0)),
                Position = new SKPoint(0,0),
                Rotation = 0f,
            };
        }

        public void Setup(SKPoint position, float rotation)
        {
            transform.Position = position;
            Rotate(rotation);
        }

        public override void Update(float deltaTime)
        {
            float sin = MathF.Sin(transform.RotationRadians);
            float cos = MathF.Cos(transform.RotationRadians);
            
            float speedDelta = Speed * deltaTime;

            transform.Position.X += cos * speedDelta;
            transform.Position.Y += sin * speedDelta;
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name == "Obstacle" || other.Name == "UFO")
            {
                GameManager.Score++;
                window.Destroy(this);
            }
        }
    }
}
