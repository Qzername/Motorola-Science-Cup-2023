using Battlezone.Objects.UI;

namespace Battlezone
{
    public static class GameManager
    {
        static int score = 0;
        static int lives = 3;

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
    }
}
