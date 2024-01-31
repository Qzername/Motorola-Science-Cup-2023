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

	        int points = rand.Next(6, 13);
			float angle = 360f / points;

			for (int i = 1; i <= points; i++)
			{
				int length = rand.Next(15, 31);

				float sin = MathF.Sin(angle * i);
				float cos = MathF.Cos(angle * i);

				float x = sin * length;
				float y = cos * length;

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
