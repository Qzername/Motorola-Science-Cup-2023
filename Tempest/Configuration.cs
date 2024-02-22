using SkiaSharp;

namespace Tempest
{
	public class Configuration()
	{
		// https://www.arcade-history.com/?n=tempest-upright-model&page=detail&id=2865
		// Domyślne kolory to kolory dla pierwszych 16 poziomów
		public SKColor Tunnel = SKColors.Blue;
		public SKColor Player = SKColors.Yellow;
		public SKColor SuperZapper = SKColors.Yellow;
		public SKColor Flipper = SKColors.Red;
		public SKColor Tanker = SKColors.Purple;
		public SKColor Spiker = SKColors.Green;
		public SKColor Pulsar = SKColors.Empty; // Pojawiają się dopiero później w grze
		public SKColor Fuseball = SKColors.White;

		private bool _isLevelClosed;
		public bool IsLevelClosed
		{
			get => _isLevelClosed;
			set => _isLevelClosed = value;
		}

		private bool _tankerSpawn = true;
		public bool TankerSpawn
		{
			get => _tankerSpawn;
			set => _tankerSpawn = value;
		}

		private bool _spikerSpawn;
		public bool SpikerSpawn
		{
			get => _spikerSpawn;
			set => _spikerSpawn = value;
		}

		private bool _fuseballSpawn;
		public bool FuseballSpawn
		{
			get => _fuseballSpawn;
			set => _fuseballSpawn = value;
		}

		public readonly int LevelLength = 1700;
	}
}
