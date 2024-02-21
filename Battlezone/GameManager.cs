using Battlezone.Objects.UI;

namespace Battlezone
{
	public static class GameManager
	{
		static int score = 0;
		static int lives = 3;
		static bool isReloading = false;

		public static int Score
		{
			get => score;
			set
			{
				score = value;
				UIManager.Instance.RefreshUI();
			}
		}

		public static int Lives
		{
			get => lives;
			set
			{
				lives = value;
				UIManager.Instance.RefreshUI();
			}
		}

		public static bool IsReloading
		{
			get => isReloading;
			set
			{
				isReloading = value;
				UIManager.Instance.RefreshUI();
			}
		}
	}
}
