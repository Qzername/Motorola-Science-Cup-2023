using Asteroids.Objects.Animations;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Asteroids.Objects
{
    /// <summary>
    /// Sinus - przeciwnik który szybko porusza się w góre i w dół i przemierza całą plansze
    /// </summary>
    public class Sinus : PhysicsObject
    {
        public override int PhysicsLayer => (int)PhysicsLayers.Other;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name == "Bullet")
            {
                window.Instantiate(new Explosion(transform.Position));
                window.Destroy(this);
            }
        }

        float speed = 150f, range= 40f;

        float realPosition;
        float secondPosition;

        bool isHorizontal;

        public override Setup Start()
        {
            Random rng = new Random();

            var resolution = window.GetResolution();

            isHorizontal = Convert.ToBoolean(rng.Next(0, 2));
            Point position = Point.Zero;

            if (isHorizontal)
                secondPosition = rng.Next(0, resolution.Height);
            else
                secondPosition = rng.Next(0, resolution.Width);

            realPosition = 0f;

            return new()
            {
                Name = "Sinus",
                Position = position,
                Shape = new PointShape([new(-5,-5), new (-5,5), new(5,5), new(5,-5)])
            };
        }

        public override void Update(float delta)
        {
            realPosition += delta * speed;

            Rotate(new Point(0,0, delta * 40));

            if (isHorizontal)
                Horizontal();
            else
                Vertical();
        }

        void Horizontal()
        {
            var res = window.GetResolution();

            if (realPosition > res.Width)
                window.Destroy(this);

            transform.Position = new Point(realPosition,secondPosition + MathF.Sin(realPosition * MathTools.Deg2rad*5) * range);
        }

        void Vertical()
        {
            var res = window.GetResolution();

            if (realPosition > res.Height)
                window.Destroy(this);

            transform.Position = new Point(secondPosition + MathF.Sin(realPosition * MathTools.Deg2rad * 5) * range, realPosition);

        }
    }
}
