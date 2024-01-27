using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids.Objects
{
    public class Bullet : PhysicsObject
    {
        const float BulletSpeed = 10f;

        public override int PhysicsLayer => (int)PhysicsLayers.Player;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Bullet",
                Shape = new Shape(0f, new SKPoint(0, 0), new SKPoint(10, 0)),
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
            var sin = MathF.Sin(transform.RotationRadians);
            var cos = MathF.Cos(transform.RotationRadians);

            transform.Position.X += cos * BulletSpeed;
            transform.Position.Y += sin * BulletSpeed;
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name == "Obstacle")
            {
                GameManager.Score++;
                window.Destroy(this);
            }
        }
    }
}
