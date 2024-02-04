using SkiaSharp;
using System.Diagnostics;
using System.Timers;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids.Objects
{
    public class UFO : PhysicsObject
    {
        const float speed = 50f;
        float setRotationRadians;
        System.Timers.Timer bulletTimer = new System.Timers.Timer();

		public override int PhysicsLayer => (int)PhysicsLayers.Other;

        public override void OnCollisionEnter(PhysicsObject other)
        {
	        if (other.Name == "Bullet")
	        {
		        bulletTimer.Enabled = false;
				window.Destroy(this);             
	        }
		}

        public override Setup Start()
        {
	        bulletTimer.Elapsed += TimerShoot;
	        bulletTimer.Interval = 2000; // Strzelaj co 2 sekundy
	        bulletTimer.Enabled = true;

			return new Setup()
            {
                Name = "UFO",
                Shape = new Shape(0f,
						new SKPoint(32.5f, 0),
						new SKPoint(40, 10),
                        new SKPoint(50, 15),
                        new SKPoint(40, 20),
                        new SKPoint(10, 20),
                        new SKPoint(0, 15),
                        new SKPoint(10, 10),
                        new SKPoint(17.5f, 0),
                        new SKPoint(32.5f, 0),
                        new SKPoint(40, 10),
                        new SKPoint(50, 15),
						new SKPoint(0, 15),
						new SKPoint(10, 10),
						new SKPoint(40, 10)),
                Position = new SKPoint(0, 0),
                Rotation = 0f,
            };
        }

		public void Setup(SKPoint position, float rotation)
		{
			transform.Position = position;
			setRotationRadians = rotation * MathTools.Deg2rad;
		}

        void TimerShoot(object? sender, ElapsedEventArgs e)
        {
            SKPoint playerCenter = new SKPoint(GameManager.Player.Transform.Position.X + GameManager.Player.Shape.Center.X,
                GameManager.Player.Transform.Position.Y + GameManager.Player.Shape.Center.Y);

            float rotation = MathF.Atan2(
                (playerCenter.Y - transform.Position.Y),
                (playerCenter.X - transform.Position.X))
                * MathTools.Rad2deg;

            var bullet = new BulletUFO();
            window.Instantiate(bullet);
            bullet.Setup(transform.Position, rotation);
        }

        public override void Update(float deltaTime)
        {
			float sin = MathF.Sin(setRotationRadians);
			float cos = MathF.Cos(setRotationRadians);
			// Oblicz sin i cos uzywajac ustawionej rotacji (setRotationRadians, oryginalna rotacja UFO to zawsze 0)

			float speedDelta = speed * deltaTime;
            
			transform.Position.X += cos * speedDelta;
			transform.Position.Y += sin * speedDelta;
			// Zaktualizuj pozycje

			Resolution res = window.GetResolution();

			if (transform.Position.X < 0)
				transform.Position = new SKPoint(res.Width, transform.Position.Y);
			else if (transform.Position.X > res.Width)
				transform.Position = new SKPoint(0, transform.Position.Y);
			else if (transform.Position.Y < 0)
				transform.Position = new SKPoint(transform.Position.X, res.Height);
			else if (transform.Position.Y > res.Height)
				transform.Position = new SKPoint(transform.Position.X, 0);
			// Jesli UFO wyleci poza ekran, przenies je na przeciwna krawedz
		}
    }
}
