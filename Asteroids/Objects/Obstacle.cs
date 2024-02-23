using Asteroids.Objects.Animations;
using SkiaSharp;
using System.Diagnostics;
using System.Linq.Expressions;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Asteroids.Objects
{
    public enum ObstacleType
	{
		Small,
		Medium,
		Large,
		Random
	}
    
    public class Obstacle(float speedboost, ObstacleType obstacleType = ObstacleType.Random) : PhysicsObject
    {
        public ObstacleType Type = obstacleType;
        float speed = GameManager.Rand.Next(50 + Convert.ToInt32(speedboost), 101 + Convert.ToInt32(speedboost));
        // Wylosuj predkosc pomiedzy 50 a 200

		public override int PhysicsLayer =>(int)PhysicsLayers.Other;

        public override Setup Start()
        {
	        List<Point> shapes = new();

			// Wylosuj liczbe punktow
	        int points = GameManager.Rand.Next(6, 13);

	        int maxLength = 0, minLength = 0;

	        if (Type == ObstacleType.Random)
		        Type = (ObstacleType)GameManager.Rand.Next(0, 3);

			// Ustaw min/max dlugosci punktow w zaleznosci od typu
	        switch (Type)
	        {
				case ObstacleType.Large:
					maxLength = 41;
					minLength = 30;
					break;
				case ObstacleType.Medium:
					maxLength = 31;
					minLength = 20;
					break;
				case ObstacleType.Small:
					maxLength = 21;
					minLength = 10;
					break;
	        }
			
			for (int i = 0; i < points; i++)
			{
				// Wylosuj odleglosc punktu od srodka i offset (aby ksztalt byl nieregularny)
				int length = GameManager.Rand.Next(minLength, maxLength);
				int offset = GameManager.Rand.Next(-5, 6);

				// Oblicz sin i cos
				float sin = MathF.Sin(360f * i / points * MathTools.Deg2rad);
				float cos = MathF.Cos(360f * i / points * MathTools.Deg2rad);

				float x = sin * length + offset;
				float y = cos * length + offset;

				shapes.Add(new Point(x, y));
			}

			return new Setup()
            {
                Name = "Obstacle",
                Shape = new PointShape(shapes.ToArray()),
                Position = new Point(0, 0),
                Rotation = Point.Zero
            };
        }

		// Ustaw pozycje i rotacje Obstacle
        public void Setup(Point position, float rotation)
        {
            transform.Position = position;
            Rotate(new Point(0,0,rotation));
        }

        public override void Update(float deltaTime)
        {
            float sin = MathF.Sin(transform.RotationRadians.Z);
            float cos = MathF.Cos(transform.RotationRadians.Z);
            // Oblicz sin i cos uzywajac obecnej rotacji

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
			// Jesli Obstacle wyleci poza ekran, przenies je na przeciwna krawedz
		}

		public override void OnCollisionEnter(PhysicsObject other)
        {
	        if (other.Name == "Bullet")
            {
                window.Instantiate(new Explosion(transform.Position));
                window.Destroy(this);

				if (Type == ObstacleType.Small)
					return;

				ObstacleType newType = ObstacleType.Large;

				if (Type == ObstacleType.Medium)
					newType = ObstacleType.Small;
				else if (Type == ObstacleType.Large)
					newType = ObstacleType.Medium;

				Obstacle obstacle = new Obstacle(speedboost, newType);
				window.Instantiate(obstacle);
				obstacle.Setup(Transform.Position, Transform.Rotation.Z + GameManager.Rand.Next(30, 151));

				EnemySpawner.Instance.RegisterObstacle(obstacle);

				obstacle = new Obstacle(speedboost, newType);
				window.Instantiate(obstacle);
				obstacle.Setup(Transform.Position, Transform.Rotation.Z - GameManager.Rand.Next(30, 151));
                // Podziel obstacle na dwa mniejsze kawalki - jezeli jest najmniejszy, usun go

                EnemySpawner.Instance.RegisterObstacle(obstacle);
            }
		}
    }
}
