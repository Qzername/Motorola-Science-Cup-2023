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
        
        PhysicsEngine physicsEngine;

        VectorObject player;
        List<VectorObject> obstacles = new();

        Shape bullet;
        List<VectorObject> bullets;

        bool lastSpaceState = false;

        int score = 0;

        public GameWindow() : base()
        {
            player = new VectorObject("Player",
                new Shape(-90f, 
                        new SKPoint(0,0), 
                        new SKPoint(15,40), 
                        new SKPoint(30,0), 
                        new SKPoint(15,5)), 
                new SKPoint(150,150), 0f);

            obstacles.Add(new VectorObject("Obstacle",
	            new Shape(0f,
		            new SKPoint(0, 0),
		            new SKPoint(0, 30),
		            new SKPoint(30, 30),
		            new SKPoint(30, 0)),
	            new SKPoint(30, 30), 0f));

            bullets = new List<VectorObject>();

            physicsEngine = new PhysicsEngine(new PhysicsConfiguration()
            {
                LayerConfiguration =  new Dictionary<int, int[]>()
                {
                    { 0, new int[] {1} }, //0 -> player
                    { 1, new int[] {0} }, //1 -> bullets && obstacles
                }
            });

            physicsEngine.RegisterObjects(0, obstacles);
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

                    // Ponizej jest kod do spawnowania nowych przeszkod (test)
					Random rand = new Random();
                    int x = rand.Next(0, 100);
                    int y = rand.Next(0, 100);

					obstacles.Add(new VectorObject("Obstacle",
						new Shape(0f,
							new SKPoint(0, 0),
							new SKPoint(0, 30),
							new SKPoint(30, 30),
							new SKPoint(30, 0)),
						new SKPoint(x, y), 0f));
                    physicsEngine.RegisterObjects(0, obstacles);
                }  
	        }
        }

        public override void Update(Canvas canvas)
        {
            //Ten kod jest testowy jkbc więc pozmieniaj wszystko według swoich upodobań

            //player
            float speedDelta = speed * time.DeltaTime;
            float rotationDelta = rotationSpeed * time.DeltaTime;

            if (KeyDown(Key.Left) || KeyDown(Key.A))
                player.Rotate(rotationDelta * -1f);
            else if (KeyDown(Key.Right) || KeyDown(Key.D))
                player.Rotate(rotationDelta);

            bool isSpacePressed = KeyDown(Key.Space);

            if (isSpacePressed && !lastSpaceState)
            {
                var bullet = new VectorObject("Bullet", new Shape(0f, new SKPoint(0, 0), new SKPoint(10, 0)),
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
            else
            {
	            player.AddPosition(cos * speedDelta * glideSpeed, sin * speedDelta * glideSpeed);
	            if (glideSpeed - 0.01f > minGlideSpeed)
		            glideSpeed -= 0.01f;
	            else
		            glideSpeed = 0f;
            }

            player.Draw(canvas);

            //obstacle
            foreach (VectorObject obstacle in obstacles)
            {
				obstacle.Draw(canvas);
			}
        
            //bullets
            for(int i = 0; i < bullets.Count; i++)  
            {
                sin = MathF.Sin(bullets[i].Transform.RotationRadians);
                cos = MathF.Cos(bullets[i].Transform.RotationRadians);

                bullets[i].AddPosition(cos*bulletSpeed, sin*bulletSpeed);

                bullets[i].Draw(canvas);
            }

            // Debug.WriteLine(score);
        }
    }
}
