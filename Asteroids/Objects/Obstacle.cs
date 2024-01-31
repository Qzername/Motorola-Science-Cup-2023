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
		Large
	}
    
    public class Obstacle(ObstacleType obstacleType = ObstacleType.Large) : PhysicsObject
    {
        ObstacleType type = obstacleType;
		float speed = float.Parse($"0.{new Random().Next(3, 10)}");
        // Wylosuj float'a pomiedzy 0.3 a 0.9

		public override int PhysicsLayer =>(int)PhysicsLayers.Other;

        public override Setup Start()
        {
	        Random rand = new();
	        List<SKPoint> shapes = new();

			// Wylosuj liczbe punktow
	        int points = rand.Next(6, 13);

	        int maxLength = 0, minLength = 0;

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
				int length = rand.Next(minLength, maxLength);
				int offset = rand.Next(-5, 6);

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

        public void Setup(SKPoint position, float rotation)
        {
            transform.Position = position;
            Rotate(rotation);
        }

        public override void Update(float deltaTime)
        {
            var sin = MathF.Sin(transform.RotationRadians);
            var cos = MathF.Cos(transform.RotationRadians);

            transform.Position.X += cos * speed;
            transform.Position.Y += sin * speed;
            
            Resolution res = window.GetResolution();

			if (transform.Position.X < 0 || transform.Position.X > res.Width || transform.Position.Y < 0 || transform.Position.Y > res.Height)
				window.Destroy(this);
		}

        public override void OnCollisionEnter(PhysicsObject other)
        {
	        if (other.Name == "Bullet")
	        {
				window.Destroy(this);

				if (type == ObstacleType.Small)
					return;

                Random rand = new Random();
				ObstacleType newType = ObstacleType.Large;

				if (type == ObstacleType.Medium)
					newType = ObstacleType.Small;
				else if (type == ObstacleType.Large)
					newType = ObstacleType.Medium;

				Obstacle obstacle = new Obstacle(newType);
				window.Instantiate(obstacle);
				obstacle.Setup(Transform.Position, Transform.Rotation + rand.Next(30, 151));

				obstacle = new Obstacle(newType);
				window.Instantiate(obstacle);
				obstacle.Setup(Transform.Position, Transform.Rotation - rand.Next(30, 151));
	        }
        }
    }
}
