using VGE;
using VGE.Graphics;

namespace Battlezone.Objects
{
	public class ObstacleGenerator : VectorObject
	{
		public static ObstacleGenerator Instance;

		public override Setup Start()
		{
			Instance = this;

			float offset = Settings.ObstacleGeneratorDistance / Settings.ChunkSize;

			Random rng = new Random();

			for (float x = -offset; x < offset; x++)
				for (float z = -offset; z < offset; z++)
				{
					if (MathF.Abs(x + z) < 1) //żeby się nie zrespiło w graczu
						continue;

					float chunkXPos = x * Settings.ChunkSize;
					float chunkZPos = z * Settings.ChunkSize;

					float realX = (Settings.ChunkSize * rng.NextSingle()) + chunkXPos;
					float realZ = (Settings.ChunkSize * rng.NextSingle()) + chunkZPos;

					int chanceForPowerUp = rng.Next(0, 100);

					if (chanceForPowerUp < 10) //10% szans na power upa
						window.Instantiate(new PowerUp(new(realX, 0, realZ)));
					else
						window.Instantiate(new Obstacle(new Point(realX, 0, realZ)));
				}

			return new Setup()
			{
				Name = "ObstacleGenerator",
			};
		}

		public override void Update(float delta)
		{
		}

		public override bool OverrideRender(Canvas canvas)
		{
			//nie rysuj nic
			return true;
		}
	}
}
