using SkiaSharp;
using VGE;
using VGE.Graphics;

namespace Asteroids.Objects
{
    public class Obstacle : VectorObject
    {
        const float ObstacleSpeed = 0.5f;

        public Obstacle(string name, Shape shape, SKPoint position, float rotation) : base(name, shape, position, rotation)
        {
        }

        public override void Start()
        {
        }

        public override void Update(float deltaTime)
        {
            var sin = MathF.Sin(transform.RotationRadians);
            var cos = MathF.Cos(transform.RotationRadians);

            transform.Position.X += cos * ObstacleSpeed;
            transform.Position.Y += sin * ObstacleSpeed;
        }
    }
}
