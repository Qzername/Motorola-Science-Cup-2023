using Asteroids.Objects.Animations;
using SkiaSharp;
using System.Diagnostics;
using System.Timers;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Asteroids.Objects
{
	public enum UFOType
	{
		Small,
		Large,
		Random
	}
	
	public class UFO(UFOType ufoType = UFOType.Random) : PhysicsObject
    {
	    public UFOType Type = ufoType;
		const float speed = 150f;
        float setRotationRadians;

		public override int PhysicsLayer => (int)PhysicsLayers.Other;

        public override void OnCollisionEnter(PhysicsObject other)
        {
	        if (other.Name == "Bullet")
            {
                window.Instantiate(new Explosion(transform.Position));
                window.Destroy(this);             
	        }
		}

        public override Setup Start()
        {
	        List<Point> shapes = new();
			
			if (Type == UFOType.Random)
				Type = (UFOType)GameManager.Rand.Next(0, 2);

			switch (Type)
			{
				case UFOType.Large:
					shapes.Add(new Point(32.5f, 0));
					shapes.Add(new Point(40, 10));
					shapes.Add(new Point(50, 15));
					shapes.Add(new Point(40, 20));
					shapes.Add(new Point(10, 20));
					shapes.Add(new Point(0, 15));
					shapes.Add(new Point(10, 10));
					shapes.Add(new Point(17.5f, 0));
					shapes.Add(new Point(32.5f, 0));
					shapes.Add(new Point(40, 10));
					shapes.Add(new Point(50, 15));
					shapes.Add(new Point(0, 15));
					shapes.Add(new Point(10, 10));
					shapes.Add(new Point(40, 10));
					break;
				case UFOType.Small:
					shapes.Add(new Point(16.25f, 0));
					shapes.Add(new Point(20, 5));
					shapes.Add(new Point(25, 7.5f));
					shapes.Add(new Point(20, 10));
					shapes.Add(new Point(5, 10));
					shapes.Add(new Point(0, 7.5f));
					shapes.Add(new Point(5, 5));
					shapes.Add(new Point(8.75f, 0));
					shapes.Add(new Point(16.25f, 0));
					shapes.Add(new Point(20, 5));
					shapes.Add(new Point(25, 7.5f));
					shapes.Add(new Point(0, 7.5f));
					shapes.Add(new Point(5, 5));
					shapes.Add(new Point(20, 5));
					break;
			}

			return new Setup()
            {
                Name = "UFO",
                Shape = new PointShape(shapes.ToArray()),
                Position = new Point(0, 0),
                Rotation = Point.Zero,
            };
        }

		public void Setup(Point position, float rotation)
		{
			transform.Position = position;
			setRotationRadians = rotation * MathTools.Deg2rad;
		}

		float timer;

        public override void Update(float deltaTime)
        {
			timer += deltaTime;

			if(timer > 2f && GameManager.Instance.Player is not null)
            {
                Point playerCenter = new Point(GameManager.Instance.Player.Transform.Position.X + GameManager.Instance.Player.Shape.Center.X,
                    GameManager.Instance.Player.Transform.Position.Y + GameManager.Instance.Player.Shape.Center.Y);

                float rotation = MathF.Atan2(
                    (playerCenter.Y - transform.Position.Y),
                    (playerCenter.X - transform.Position.X))
                    * MathTools.Rad2deg;

                var bullet = new BulletUFO();
                window.Instantiate(bullet);
                bullet.Setup(transform.Position, -rotation);
				timer = 0f;
            }

			float sin = MathF.Sin(setRotationRadians);
			float cos = MathF.Cos(setRotationRadians);
			// Oblicz sin i cos uzywajac ustawionej rotacji (setRotationRadians, oryginalna rotacja UFO to zawsze 0)

			float speedDelta = speed * deltaTime;
            
			transform.Position.X += cos * speedDelta;
			transform.Position.Y += sin * speedDelta;
			// Zaktualizuj pozycje

			Resolution res = window.GetResolution();

			if (transform.Position.X < 0)
				transform.Position = new Point(res.Width, transform.Position.Y);
			else if (transform.Position.X > res.Width)
				transform.Position = new Point(0, transform.Position.Y);
			else if (transform.Position.Y < 0)
				transform.Position = new Point(transform.Position.X, res.Height);
			else if (transform.Position.Y > res.Height)
				transform.Position = new Point(transform.Position.X, 0);
			// Jesli UFO wyleci poza ekran, przenies je na przeciwna krawedz
		}
    }
}
