using System.Security.Cryptography.Xml;
using VGE;
using VGE.Graphics;

namespace Asteroids.Objects.Animations
{
    public class Explosion : VectorObject
    {
        const float animationSpeed = 50f;
        const float animationTime = 0.5f;

        float distance;
        float timer;

        Point[] points;

        Point startPosition;

        public Explosion(Point startPosition)
        {
            this.startPosition  = startPosition;
        }

        public override Setup Start()
        {
            points = new Point[10];
            var rng = new Random();

            for(int i = 0; i < points.Length;i++)
            {
                //X = distance, Z = rotation
                points[i] = new Point(rng.Next(5, 15), 0,rng.Next(0,360));
            }

            return new Setup()
            {
                Name = "Explosion",
                Position = startPosition
            };
        }

        public override void Update(float delta)
        {
            distance += delta * animationSpeed;
            timer += delta;

            if (timer > animationTime)
                window.Destroy(this);
        }

        public override bool OverrideRender(Canvas canvas)
        {
            foreach(var point in points)
            {
                float sin = MathF.Sin(-point.Z * MathTools.Deg2rad);
                float cos = MathF.Cos(-point.Z * MathTools.Deg2rad);

                float x = cos * (distance + point.X);
                float y = sin * (distance + point.X);

                float x2 = cos * (distance + point.X + 5);
                float y2 = sin * (distance + point.X + 5);

                canvas.DrawLine(new Line(new Point(x, y)  +  transform.Position, 
                                         new Point(x2, y2) + transform.Position));
            }

            return true;
        }
    }
}
