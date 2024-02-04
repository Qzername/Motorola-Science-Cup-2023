using Asteroids.Objects;
using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;
using System.Timers;
using VGE.Objects;

namespace Asteroids
{
    internal class GameWindow : Window
    {
        int obstacleRotationOffset = 30;
        // obstacleRotationOffset nie moze byc wiekszy niz 45, poniewaz wtedy przeszkoda moze byc poza ekranem

        Random rand = new();

		public GameWindow() : base()
        {
			GameManager.Setup(this);

            var physicsEngine = new PhysicsEngine(new PhysicsConfiguration()
            {
                LayerConfiguration = new Dictionary<int, int[]>()
                {
                    { 0, new int[] {1} }, //0 -> player
                    { 1, new int[] {0} }, //1 -> bullets && obstacles
                }
            });
            RegisterPhysicsEngine(physicsEngine);

			Instantiate(new Player());

			System.Timers.Timer obstacleTimer = new System.Timers.Timer();
			obstacleTimer.Elapsed += TimerSpawnObstacle;
			obstacleTimer.Interval = 5000; // Spawnuj Obstacle co 5 sekund
			obstacleTimer.Enabled = true;

			System.Timers.Timer ufoTimer = new System.Timers.Timer();
			ufoTimer.Elapsed += TimerSpawnUfo;
			ufoTimer.Interval = 30000; // Spawnuj UFO co 30 sekund
			ufoTimer.Enabled = true;
		}

        public override void Update(Canvas canvas) { }

        void TimerSpawnObstacle(object? sender, ElapsedEventArgs e)
        {
			// Co 10 puktow, spawnuj o 1 przeszkode wiecej
	        int spawnTimes = (int)Math.Ceiling((GameManager.Score != 0 ? GameManager.Score : 1) / 10M);

	        for (int i = 0; i < spawnTimes; i++)
	        {
		        int[] pos = GetRandomPosition();
				var obstacle = new Obstacle();
				Instantiate(obstacle);
				obstacle.Setup(new SKPoint(pos[0], pos[1]), pos[2]);
				// 0 -> x, 1 -> y, 2 -> rotation
			}
        }

		void TimerSpawnUfo(object? sender, ElapsedEventArgs e)
		{
			int[] pos = GetRandomPosition();
			var ufo = new UFO();
			Instantiate(ufo);
			ufo.Setup(new SKPoint(pos[0], pos[1]), pos[2]);
			// 0 -> x, 1 -> y, 2 -> rotation
		}

		int[] GetRandomPosition()
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
			
			Resolution res = GetResolution();

	        switch (side)
	        {
		        case 0:
			        x = 0;
			        y = rand.Next(0, res.Height);
			        rotation = rand.Next(0 - obstacleRotationOffset, 0 + obstacleRotationOffset + 1);
					break;
		        case 1:
			        x = res.Width;
			        y = rand.Next(0, res.Height);
					rotation = rand.Next(180 - obstacleRotationOffset, 180 + obstacleRotationOffset + 1);
					break;
		        case 2:
			        x = rand.Next(0, res.Width);
			        y = 0;
			        rotation = rand.Next(90 - obstacleRotationOffset, 90 + obstacleRotationOffset + 1);
					break;
		        case 3:
			        x = rand.Next(0, res.Width);
			        y = res.Height;
			        rotation = rand.Next(270 - obstacleRotationOffset, 270 + obstacleRotationOffset + 1);
					break;
		        default:
			        x = y = rotation = 0;
			        break;
	        }

            return [x, y, rotation];
		}
    }
}
