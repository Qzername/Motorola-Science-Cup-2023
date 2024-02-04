using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids.Objects
{
	public enum ObstacleType
	{
		Small,
		Medium,
		Large,
		Random
	}
    
    public class Obstacle(ObstacleType obstacleType = ObstacleType.Random) : PhysicsObject
    {
        ObstacleType type = obstacleType;
        float speed = GameManager.Rand.Next(50, 201);
        // Wylosuj predkosc pomiedzy 50 a 200

		public override int PhysicsLayer =>(int)PhysicsLayers.Other;

        public override Setup Start()
        {
	        List<SKPoint> shapes = new();

			// Wylosuj liczbe punktow
	        int points = GameManager.Rand.Next(6, 13);

	        int maxLength = 0, minLength = 0;

	        if (type == ObstacleType.Random)
		        type = (ObstacleType)GameManager.Rand.Next(0, 3);

			// Ustaw min/max dlugosci punktow w zaleznosci od typu
	        switch (type)
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

	        float radian = MathF.PI / 180;

			for (int i = 0; i < points; i++)
			{
				// Wylosuj odleglosc punktu od srodka i offset (aby ksztalt byl nieregularny)
				int length = GameManager.Rand.Next(minLength, maxLength);
				int offset = GameManager.Rand.Next(-5, 6);

				// Oblicz sin i cos
				float sin = MathF.Sin(360f * i / points * radian);
				float cos = MathF.Cos(360f * i / points * radian);

				float x = sin * length + offset;
				float y = cos * length + offset;

				shapes.Add(new SKPoint(x, y));
			}

			return new Setup()
            {
                Name = "Obstacle",
                Shape = new Shape(0f, shapes.ToArray()),
                Position = new SKPoint(0, 0),
                Rotation = 0f
            };
        }

		// Ustaw pozycje i rotacje Obstacle
        public void Setup(SKPoint position, float rotation)
        {
            transform.Position = position;
            Rotate(rotation);
        }

        public override void Update(float deltaTime)
        {
            float sin = MathF.Sin(transform.RotationRadians);
            float cos = MathF.Cos(transform.RotationRadians);
            // Oblicz sin i cos uzywajac obecnej rotacji

			float speedDelta = speed * deltaTime;

			transform.Position.X += cos * speedDelta;
            transform.Position.Y += sin * speedDelta;
            // Zaktualizuj pozycje

			Resolution res = window.GetResolution();

			if (transform.Position.X < 0 || transform.Position.X > res.Width || transform.Position.Y < 0 || transform.Position.Y > res.Height)
				window.Destroy(this);
			// Jezeli Obstacle wyleci poza ekran, usun je
		}

		public override void OnCollisionEnter(PhysicsObject other)
        {
	        if (other.Name == "Bullet")
	        {
				window.Destroy(this);

				if (type == ObstacleType.Small)
					return;

				ObstacleType newType = ObstacleType.Large;

				if (type == ObstacleType.Medium)
					newType = ObstacleType.Small;
				else if (type == ObstacleType.Large)
					newType = ObstacleType.Medium;

				Obstacle obstacle = new Obstacle(newType);
				window.Instantiate(obstacle);
				obstacle.Setup(Transform.Position, Transform.Rotation + GameManager.Rand.Next(30, 151));

				obstacle = new Obstacle(newType);
				window.Instantiate(obstacle);
				obstacle.Setup(Transform.Position, Transform.Rotation - GameManager.Rand.Next(30, 151));
				// Podziel obstacle na dwa mniejsze kawalki - jezeli jest najmniejszy, usun go
			}
		}
    }
}
