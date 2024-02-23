using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Asteroids.Objects
{
    public class Bullet : PhysicsObject
    {
		public float Speed = 500f;
		int length, maxLength = 150;

        public override int PhysicsLayer => (int)PhysicsLayers.Player;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Bullet",
                Shape = new PointShape(Point.Zero, new Point(0,0), new Point(5, 0)),
                Position = new Point(0,0),
                Rotation = Point.Zero,
            };
        }

        public void Setup(Point position, float rotation)
        {
            transform.Position = position;
            Rotate(new Point(0, 0, rotation));
        }

        public override void Update(float deltaTime)
        {
            float sin = MathF.Sin(-transform.RotationRadians.Z);
            float cos = MathF.Cos(-transform.RotationRadians.Z);
            
            float speedDelta = Speed * deltaTime;

            transform.Position.X += cos * speedDelta;
            transform.Position.Y += sin * speedDelta;

            Resolution res = window.GetResolution();

			if (transform.Position.X < 0)
				transform.Position = new Point(res.Width, transform.Position.Y);
			else if (transform.Position.X > res.Width)
				transform.Position = new Point(0, transform.Position.Y);
			else if (transform.Position.Y < 0)
				transform.Position = new Point(transform.Position.X, res.Height);
			else if (transform.Position.Y > res.Height)
				transform.Position = new Point(transform.Position.X, 0);
			// Jesli pocisk wyleci poza ekran, przenies go na przeciwna krawedz

			length++;
			if (length >= maxLength)
				Destroy();
			/*
				https://www.youtube.com/watch?v=BgloG8yt-jA
				Pociski znikaja po przeleceniu pewnej odleglosci - ustawienie maxLength na 150 najlepiej to odwzorowywuje
			*/
		}

		public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name == "Obstacle" || other.Name == "UFO")
            {
	            if (other is Obstacle)
	            {
		            switch ((other as Obstacle).Type)
		            {
						case ObstacleType.Small:
							GameManager.Instance.Score += 100;
							break;
						case ObstacleType.Medium:
							GameManager.Instance.Score += 50;
							break;
						case ObstacleType.Large:
							GameManager.Instance.Score += 20;
							break;
					}

					EnemySpawner.Instance.RemoveObstacle((Obstacle)other);
	            } 
	            else if (other is UFO)
	            {
					switch ((other as UFO).Type)
					{
						case UFOType.Large:
							GameManager.Instance.Score += 200;
							break;
						case UFOType.Small:
							GameManager.Instance.Score += 1000;
							break;
					}

                    EnemySpawner.Instance.RemoveUFO((UFO)other);
                }

                // "za kazde zdobyte 10 000 punktow gracz otrzymuje dodatkowe zycie"
                if (GameManager.Instance.Score >= GameManager.Instance.ScoreToGet)
                {
                    GameManager.Instance.Lives++;
					GameManager.Instance.ScoreToGet += 10000;
                }
				
				Destroy();
            }
        }

        void Destroy()
        {
	        GameManager.Instance.BulletsOnScreen--;
	        window.Destroy(this);
		}
    }
}
