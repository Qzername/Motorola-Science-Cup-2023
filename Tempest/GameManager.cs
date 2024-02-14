namespace Tempest
{
    public static class GameManager
	{
		static Configuration _configuration = new();
		public static Configuration Configuration
		{
			get => _configuration; 
			set => _configuration = value;
		}
	}
}