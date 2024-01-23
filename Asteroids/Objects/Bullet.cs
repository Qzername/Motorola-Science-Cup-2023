using SkiaSharp;
using VGE;
using VGE.Graphics;

namespace Asteroids.Objects
{
    public class Bullet : VectorObject
    {
        const float BulletSpeed = 10f;

        public Bullet(string name, Shape shape, SKPoint position, float rotation) : base(name, shape, position, rotation)
        {
        }

        public override void Start()
        {
        }

        public override void Update(float deltaTime)
        {
            var sin = MathF.Sin(transform.RotationRadians);
            var cos = MathF.Cos(transform.RotationRadians);

            transform.Position.X += cos * BulletSpeed;
            transform.Position.Y += sin * BulletSpeed;
        }
    }
}
