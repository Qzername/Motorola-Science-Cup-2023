using SkiaSharp;

namespace Tempest
{
	public class LevelConfiguration()
	{
		// https://www.arcade-history.com/?n=tempest-upright-model&page=detail&id=2865
		// Domyślne kolory to kolory dla pierwszych 16 poziomów
		public SKColor Tunnel = SKColors.Blue;
		public SKColor Player = SKColors.Yellow;
		public SKColor Spike = SKColors.White;
		public SKColor SuperZapper = SKColors.Yellow;
		public SKColor Flipper = SKColors.Red;
		public SKColor Tanker = SKColors.Purple;
		public SKColor Spiker = SKColors.Green;
		public SKColor Pulsar = SKColors.Empty; // Pojawiają się dopiero później w grze
		public SKColor Fuseball = SKColors.White;

		private bool _isClosed;
		public bool IsClosed
		{
			get => _isClosed;
			set => _isClosed = value;
		}

		private bool _moveFlipper;
		public bool MoveFlipper
		{
			get => _moveFlipper;
			set => _moveFlipper = value;
		}

		private bool _spawnTanker = true;
		public bool SpawnTanker
		{
			get => _spawnTanker;
			set => _spawnTanker = value;
		}

		private bool _spawnSpiker;
		public bool SpawnSpiker
		{
			get => _spawnSpiker;
			set => _spawnSpiker = value;
		}

		private bool _spawnFuseball;
		public bool SpawnFuseball
		{
			get => _spawnFuseball;
			set => _spawnFuseball = value;
		}

		public readonly int Length = 1700;
	}
}
