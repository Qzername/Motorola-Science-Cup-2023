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
		public SKColor SuperZapper = SKColors.Yellow;
		public SKColor Flipper = SKColors.Red;
		public SKColor Tanker = SKColors.Purple;
		public SKColor Spiker = SKColors.Green;
		public SKColor Pulsar = SKColors.Empty; // Pojawiają się dopiero później w grze
		public readonly SKColor Fuseball = SKColors.White; // Fuseball nie zmienia koloru

		public bool IsClosed = true;
		public bool MoveFlipper = false;
		
		public bool SpawnTanker = true;
		public bool SpawnSpiker;
		public bool SpawnFuseball;

		public readonly int Length = 1700;

		public void ChangeColorScheme()
		{
			if (Tunnel == SKColors.Blue)
			{
				Tunnel = SKColors.Red;
				Player = SKColors.Green;
				SuperZapper = SKColors.Cyan;
				Flipper = SKColors.Purple;
				Tanker = SKColors.Blue;
				Spiker = SKColors.Cyan;
				Pulsar = SKColors.Yellow;
			}
			else if (Tunnel == SKColors.Red)
			{
				Tunnel = SKColors.Yellow;
				Player = SKColors.Blue;
				SuperZapper = SKColors.Blue;
				Flipper = SKColors.Green;
				Tanker = SKColors.Cyan;
				Spiker = SKColors.Red;
				Pulsar = SKColors.Blue;
			}
			else if (Tunnel == SKColors.Yellow)
			{
				Tunnel = SKColors.Cyan;
				Player = SKColors.Blue;
				SuperZapper = SKColors.Red;
				Flipper = SKColors.Green;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Red;
				Pulsar = SKColors.Yellow;
			}
			else if (Tunnel == SKColors.Cyan)
			{
				Tunnel = SKColors.Empty;
				Player = SKColors.Yellow;
				SuperZapper = SKColors.White;
				Flipper = SKColors.Red;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Green;
				Pulsar = SKColors.Cyan;
			}
			else if (Tunnel == SKColors.Empty)
			{
				Tunnel = SKColors.Green;
				Player = SKColors.Red;
				SuperZapper = SKColors.Purple;
				Flipper = SKColors.Yellow;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Blue;
				Pulsar = SKColors.Yellow;
			}
			else if (Tunnel == SKColors.Green)
			{
				Tunnel = SKColors.Blue;
				Player = SKColors.Yellow;
				SuperZapper = SKColors.Yellow;
				Flipper = SKColors.Red;
				Tanker = SKColors.Purple;
				Spiker = SKColors.Green;
				Pulsar = SKColors.Empty;
			}
		}
	}
}
