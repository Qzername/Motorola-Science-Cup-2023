namespace Tempest
{
    public static class GameManager
	{
		private static Configuration _configuration = new();
		public static Configuration Configuration
		{
			get => _configuration; 
			set => _configuration = value;
		}

		private static bool _isLevelClosed;
		public static bool IsLevelClosed
		{
			get => _isLevelClosed; 
			set => _isLevelClosed = value;
		}

		private static int mapPosition;
		public static int MapPosition
		{
			get => mapPosition; 
			set => mapPosition = value;
		}
		
		public static Random Rand = new();
	}
}