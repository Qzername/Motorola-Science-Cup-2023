using Asteroids.Objects;
using SkiaSharp;
using VGE;
using VGE.Objects;

namespace Asteroids
{   public static class GameManager
    {
        static Text scoreText, livesText;

        public static void Setup(Window window)
        {
            scoreText = new Text();
            window.Instantiate(scoreText);
            scoreText.Setup("Score: 0", 2, new SKPoint(10,40));

            livesText = new Text(); 
            window.Instantiate(livesText);
            livesText.Setup("Lives: 3", 2, new SKPoint(10, 10));
        }

        static int _score = 0;
        public static int Score
        {
            get => _score;
            set
            {
                _score = value;
                //nie ma znaczenia czy literka jest mała czy duża btw
                scoreText.SetText("Score: " + value);
            }
        }

        static int _lives = 3;
        public static int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                livesText.SetText("Lives: "+ value);
            }
        }

		static Player _player;
		public static Player Player
		{
			get => _player;
			set => _player = value;
		}

		public static Random Rand = new Random();
	}
}
