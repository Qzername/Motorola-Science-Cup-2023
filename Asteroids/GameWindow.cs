using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids
{
    internal class GameWindow : Window
    {
	    const float minSpeed = 0, maxSpeed = 300;
        float speed = 0, rotationSpeed = 90, bulletSpeed = 10, prevRotation, prevRotationRadias;
        int score, obstacleRotationOffset = 30;
		// obstacleRotationOffset nie moze byc wiekszy niz 45, poniewaz wtedy przeszkoda moze byc poza ekranem

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
			Debug.WriteLine($"FPS: {1f / time.DeltaTime}");
			Debug.WriteLine($"Score: {score}");
			Debug.WriteLine($"Speed: {speed}");
			Debug.WriteLine($"Rotation: {player.Transform.Rotation}");
			Debug.WriteLine($"Previous Rotation: {prevRotation}");

			bool isSpacePressed = KeyDown(Key.Space);

            if (isSpacePressed && !lastSpaceState)
            {
                var bullet = new VectorObject("Bullet", 
	                new Shape(0f, 
		                new SKPoint(0, 0), 
		                new SKPoint(1, 0)),
			player.Transform.Position + player.Shape.CompiledShape[0].EndPosition,
	                player.Transform.Rotation);

                bullets.Add(bullet);
                physicsEngine.RegisterObject(1, bullet);

                lastSpaceState = true;
            }
            else if (!isSpacePressed && lastSpaceState)
                lastSpaceState = false;

            float sin = MathF.Sin(prevRotationRadias);
            float cos = MathF.Cos(prevRotationRadias);

			float speedDelta = speed * time.DeltaTime;
			float rotationDelta = rotationSpeed * time.DeltaTime;

			if (KeyDown(Key.LeftShift) || KeyDown(Key.RightShift))
				rotationDelta *= 2;

			if (KeyDown(Key.Left) || KeyDown(Key.A))
			{
				player.Rotate(rotationDelta * -1f);
			}
			else if (KeyDown(Key.Right) || KeyDown(Key.D))
			{
				player.Rotate(rotationDelta);
			}

			if (KeyDown(Key.Up) || KeyDown(Key.W))
            {
				// jezeli poprzednia zarejestrowana rotacja jest w tym samym kierunku co aktualna rotacja, to zwiekszaj predkosc
				if (ShouldSlow())
	            {
					if (speed - maxSpeed / 100 > minSpeed)
			            speed -= maxSpeed / 100;
					else
					{
						prevRotation = player.Transform.Rotation;
			            prevRotationRadias = player.Transform.RotationRadians;
						speed = minSpeed;
					}
	            }
	            else
	            {
		            prevRotation = player.Transform.Rotation;
					prevRotationRadias = player.Transform.RotationRadians;
					if (speed + maxSpeed / 100 < maxSpeed)
						speed += maxSpeed / 100;
					else
						speed = maxSpeed;	            
	            }
				
				player.AddPosition(cos * speedDelta, sin * speedDelta);
            }
            else if (KeyDown(Key.Down) || KeyDown(Key.S))
            {
	            player.AddPosition(cos * speedDelta, sin * speedDelta);
	            if (speed - maxSpeed / 500 > minSpeed)
		            speed -= maxSpeed / 500;
	            else
		            speed = minSpeed;
			}
			else
            {
	            player.AddPosition(cos * speedDelta, sin * speedDelta);
	            if (speed - maxSpeed / 1000 > minSpeed)
		            speed -= maxSpeed / 1000;
	            else
		            speed = minSpeed;
			}
            
            if (player.Transform.Position.X < 0)
				player.SetPosition(Width, player.Transform.Position.Y);
			else if (player.Transform.Position.X > Width)
				player.SetPosition(0, player.Transform.Position.Y);
			else if (player.Transform.Position.Y < 0)
				player.SetPosition(player.Transform.Position.X, Height);
			else if (player.Transform.Position.Y > Height)
				player.SetPosition(player.Transform.Position.X, 0);

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
			// 90 -> w Dol			
			// 270 -> w Gore

			// maxValue w rand.Next musi byc +1, aby oryginalny maxValue tez byl brany pod uwage
			
			int x, y, rotation, side = rand.Next(0, 3 + 1);

	        switch (side)
	        {
		        case 0:
			        x = 0;
			        y = rand.Next(0, Height);
			        rotation = rand.Next(0 - obstacleRotationOffset, 0 + obstacleRotationOffset + 1);
					break;
		        case 1:
			        x = Width;
			        y = rand.Next(0, Height);
					rotation = rand.Next(180 - obstacleRotationOffset, 180 + obstacleRotationOffset + 1);
					break;
		        case 2:
			        x = rand.Next(0, Width);
			        y = 0;
			        rotation = rand.Next(90 - obstacleRotationOffset, 90 + obstacleRotationOffset + 1);
					break;
		        case 3:
			        x = rand.Next(0, Width);
			        y = Height;
			        rotation = rand.Next(270 - obstacleRotationOffset, 270 + obstacleRotationOffset + 1);
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

        bool ShouldSlow()
        {
			float tPrevRotation = prevRotation;
			float tRotation = player.Transform.Rotation;
			
	        if (tPrevRotation < 0)
		        tPrevRotation = 360 + tPrevRotation;
			if (tRotation < 0)
				tRotation = 360 + tRotation;

			if (tRotation - 45 <= tPrevRotation && tPrevRotation <= tRotation + 45)
				return false;

			return true;
        }
    }
}
