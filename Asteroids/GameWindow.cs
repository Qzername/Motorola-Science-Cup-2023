using Asteroids.Objects;
using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids
{
    internal class GameWindow : Window
    {
        int score, obstacleRotationOffset = 30;
        // obstacleRotationOffset nie moze byc wiekszy niz 45, poniewaz wtedy przeszkoda moze byc poza ekranem

        Random rand = new();

		public GameWindow() : base()
        {
            var physicsEngine = new PhysicsEngine(new PhysicsConfiguration()
            {
                LayerConfiguration = new Dictionary<int, int[]>()
                {
                    { 0, new int[] {1} }, //0 -> player
                    { 1, new int[] {0} }, //1 -> bullets && obstacles
                }
            });
            physicsEngine.CollisionDetected += PhysicsEngine_CollisionDetected;
            RegisterPhysicsEngine(physicsEngine);

            var player = new Player(new Shape(-90f, 
                        new SKPoint(0,0), 
                        new SKPoint(15,40), 
                        new SKPoint(30,0), 
                        new SKPoint(15,5)), 
                new SKPoint(preLaunchWidth / 2, preLaunchHeight / 2), 0f);
			Instantiate(player, (int)PhysicsLayers.Player);
        }

        private void PhysicsEngine_CollisionDetected(VectorObject arg1, VectorObject arg2)
        {
            if (arg1.Name == "Bullet" && arg2.Name == "Obstacle")
	        {
				score++;

				Destroy(arg1);
				Destroy(arg2);
			}
	        else if (arg1.Name == "Obstacle" && arg2.Name == "Player")
	        {
				// Game over
			}
        }

        public override void Update(Canvas canvas)
        {
			Debug.WriteLine($"Score: {score}");

            // 1% szans, aby stworzyc przeszkode (kazda klatke)
            int chance = rand.Next(1, 101);
            if (chance == 100)
				SpawnObstacle();
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

			var obstacle = new Obstacle(
				new Shape(0f,
					new SKPoint(0, 0),
					new SKPoint(0, 30),
					new SKPoint(30, 30),
					new SKPoint(30, 0)),
				new SKPoint(x, y), rotation);

			Instantiate(obstacle, (int)PhysicsLayers.Other);
		}
    }
}
