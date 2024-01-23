using SkiaSharp;
using System.Diagnostics;
using VGL;
using VGL.Graphics;
using VGL.Physics;

namespace Asteroids
{
    internal class GameWindow : Window
    {
	    const float minGlideSpeed = 0f, maxGlideSpeed = 2.0f;
        float speed = 120, rotationSpeed = 90, bulletSpeed = 10, glideSpeed = minGlideSpeed;
        int score;

        bool lastSpaceState;
        
        PhysicsEngine physicsEngine;

        VectorObject player;
        List<VectorObject> obstacles = new();

        Shape bullet;
        List<VectorObject> bullets;

        Random rand = new();

		public GameWindow() : base()
		{
			player = new VectorObject("Player",
                new Shape(-90f, 
                        new SKPoint(0,0), 
                        new SKPoint(15,40), 
                        new SKPoint(30,0), 
                        new SKPoint(15,5)), 
                new SKPoint(preLaunchWidth / 2, preLaunchHeight / 2), 0f);

            bullets = new List<VectorObject>();

            physicsEngine = new PhysicsEngine(new PhysicsConfiguration()
            {
                LayerConfiguration =  new Dictionary<int, int[]>()
                {
                    { 0, new int[] {1} }, //0 -> player
                    { 1, new int[] {0} }, //1 -> bullets && obstacles
                }
            });

            physicsEngine.RegisterObject(1, player);

            physicsEngine.CollisionDetected += PhysicsEngine_CollisionDetected;
        }

        private void PhysicsEngine_CollisionDetected(VectorObject arg1, VectorObject arg2)
        {
	        if (arg1.Name == "Obstacle" && arg2.Name == "Bullet")
	        {
				score++;

                VectorObject? obstacle = obstacles.Where(x => x.Guid == arg1.Guid).FirstOrDefault();

                if (obstacle != null)
                {
					obstacles.Remove(obstacle);
					physicsEngine.UnRegisterObject(0, obstacle);

					VectorObject? bullet = bullets.Where(x => x.Guid == arg2.Guid).FirstOrDefault();

					if (bullet != null)
					{
                        bullets.Remove(bullet);
                        physicsEngine.UnRegisterObject(1, bullet);
					}
                }  
	        }
	        else if (arg1.Name == "Obstacle" && arg2.Name == "Player")
	        {
				// Game over
			}
        }

        public override void Update(Canvas canvas)
        {
			//player
			float speedDelta = speed * time.DeltaTime;
            float rotationDelta = rotationSpeed * time.DeltaTime;

            if (KeyDown(Key.LeftShift) || KeyDown(Key.RightShift))
				rotationDelta *= 2;

            if (KeyDown(Key.Left) || KeyDown(Key.A))
                player.Rotate(rotationDelta * -1f);
            else if (KeyDown(Key.Right) || KeyDown(Key.D))
                player.Rotate(rotationDelta);

            bool isSpacePressed = KeyDown(Key.Space);

            if (isSpacePressed && !lastSpaceState)
            {
                var bullet = new VectorObject("Bullet", new Shape(0f, new SKPoint(0, 0), new SKPoint(1, 0)),
                            player.Transform.Position + player.Shape.CompiledShape[0].EndPosition, //wykorzystuje tutaj shape gracza aby respic w drugim punkcie pocisk (zobacz shape gracza)
                            player.Transform.Rotation);

                bullets.Add(bullet);
                physicsEngine.RegisterObject(1, bullet);

                lastSpaceState = true;
            }
            else if (!isSpacePressed && lastSpaceState)
                lastSpaceState = false;

            float sin = MathF.Sin(player.Transform.RotationRadians);
            float cos = MathF.Cos(player.Transform.RotationRadians);

            if (KeyDown(Key.Up) || KeyDown(Key.W))
            {
				player.AddPosition(cos * speedDelta * glideSpeed, sin * speedDelta * glideSpeed);
                if (glideSpeed < maxGlideSpeed)
					glideSpeed += 0.05f;
            }
            else if (KeyDown(Key.Down) || KeyDown(Key.S))
            {
	            player.AddPosition(cos * speedDelta * glideSpeed, sin * speedDelta * glideSpeed);
	            if (glideSpeed - 0.025f > minGlideSpeed)
		            glideSpeed -= 0.025f;
	            else
		            glideSpeed = 0f;
			}
			else
            {
	            player.AddPosition(cos * speedDelta * glideSpeed, sin * speedDelta * glideSpeed);
	            if (glideSpeed - 0.01f > minGlideSpeed)
		            glideSpeed -= 0.01f;
	            else
		            glideSpeed = 0f;
            }
            
            if (player.Transform.Position.X < 0 || player.Transform.Position.X > Width)
	            player.SetPosition(Width / 2, Height / 2);
			else if (player.Transform.Position.Y < 0 || player.Transform.Position.Y > Height)
				player.SetPosition(Width / 2, Height / 2);

			player.Draw(canvas);

            // 1% szans, aby stworzyc przeszkode (kazda klatke)
            int chance = rand.Next(1, 101);
            if (chance == 100)
				SpawnObstacle();

			//obstacle
			for (int i = 0; i < obstacles.Count; i++)
			{
				sin = MathF.Sin(obstacles[i].Transform.RotationRadians);
				cos = MathF.Cos(obstacles[i].Transform.RotationRadians);

				obstacles[i].AddPosition(cos * 0.5f, sin * 0.5f);

				obstacles[i].Draw(canvas);
			}

			//bullets
			for (int i = 0; i < bullets.Count; i++)  
            {
                sin = MathF.Sin(bullets[i].Transform.RotationRadians);
                cos = MathF.Cos(bullets[i].Transform.RotationRadians);

                bullets[i].AddPosition(cos*bulletSpeed, sin*bulletSpeed);

                bullets[i].Draw(canvas);
            }

            // Debug.WriteLine(score);
        }

        void SpawnObstacle()
        {
			// Strony
			// 0 -> Lewo
			// 1 -> Prawo
			// 2 -> Gora
			// 3 -> Dol
			
			// Rotacja
			// 0 -> w Prawo
			// 180 -> w Lewo
			// 270 -> w Gore
			// 90 -> w Dol

			// maxValue w rand.Next music byc +1, aby oryginalny maxValue tez byl brany pod uwage
			
			int x, y, rotation, side = rand.Next(0, 3 + 1);

	        switch (side)
	        {
		        case 0:
			        x = 0;
			        y = rand.Next(0, Height);
			        rotation = rand.Next(-45, 45 + 1);
					// 0 - 45 = -45, 0 + 45 = 45
					break;
		        case 1:
			        x = Width;
			        y = rand.Next(0, Height);
					rotation = rand.Next(135, 225 + 1);
					// 180 - 45 = 135, 180 + 45 = 225
					break;
		        case 2:
			        x = rand.Next(0, Width);
			        y = 0;
			        rotation = rand.Next(90, 135 + 1);
			        // 90 - 45 = 45, 90 + 45 = 135
					break;
		        case 3:
			        x = rand.Next(0, Width);
			        y = Height;
			        rotation = rand.Next(225, 315 + 1);
			        // 270 - 45 = 225, 270 + 45 = 315
					break;
		        default:
			        x = y = rotation = 0;
			        break;
	        }

	        obstacles.Add(new VectorObject("Obstacle",
		        new Shape(0f,
			        new SKPoint(0, 0),
			        new SKPoint(0, 30),
			        new SKPoint(30, 30),
			        new SKPoint(30, 0)),
		        new SKPoint(x, y), rotation));
	        physicsEngine.RegisterObjects(0, obstacles);
		}
    }
}
