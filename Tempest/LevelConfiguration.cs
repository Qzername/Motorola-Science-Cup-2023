using SkiaSharp;

namespace Tempest
{
	public class LevelConfiguration()
	{
		// https://www.arcade-history.com/?n=tempest-upright-model&page=detail&id=2865
		// Domyślne kolory to kolory dla pierwszych 16 poziomów
		public SKColor Tunnel = SKColors.Blue;
		public SKColor Player = SKColors.Yellow;
		public SKColor Spike = SKColors.White; // Pociski wrogow nie zmianiaja koloru
		public SKColor Flipper = SKColors.Red;
		public SKColor Tanker = SKColors.Purple;
		public SKColor Spiker = SKColors.Green;
		public readonly SKColor Fuseball = SKColors.White; // Fuseball nie zmienia koloru
		public readonly SKColor Fire = SKColors.Orange;

		public bool MoveFlipper = false;

		public bool SpawnTanker;
		public bool SpawnSpiker;
		public bool SpawnFuseball;
		public bool SpawnFire = true;

		public readonly int Length = 1700;

		public void ChangeColorScheme()
		{
			if (Tunnel == SKColors.Blue)
			{
				Tunnel = SKColors.Red;
				Player = SKColors.Green;
				Flipper = SKColors.Purple;
				Tanker = SKColors.Blue;
				Spiker = SKColors.Cyan;
			}
			else if (Tunnel == SKColors.Red)
			{
				Tunnel = SKColors.Yellow;
				Player = SKColors.Blue;
				Flipper = SKColors.Green;
				Tanker = SKColors.Cyan;
				Spiker = SKColors.Red;
			}
			else if (Tunnel == SKColors.Yellow)
			{
				Tunnel = SKColors.Cyan;
				Player = SKColors.Blue;
				Flipper = SKColors.Green;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Red;
			}
			else if (Tunnel == SKColors.Cyan)
			{
				Tunnel = SKColors.Empty;
				Player = SKColors.Yellow;
				Flipper = SKColors.Red;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Green;
			}
			else if (Tunnel == SKColors.Empty)
			{
				Tunnel = SKColors.Green;
				Player = SKColors.Red;
				Flipper = SKColors.Yellow;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Blue;
			}
			else if (Tunnel == SKColors.Green)
			{
				Tunnel = SKColors.Blue;
				Player = SKColors.Yellow;
				Flipper = SKColors.Red;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Green;
			}
		}
	}
}
