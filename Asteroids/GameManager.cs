using Asteroids.Objects;
using Asteroids.Objects.UI;
using HML;
using HML.Objects;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Asteroids
{
    public class GameManager : VectorObject
    {
		public static GameManager Instance;
        public static Random Rand = new();

        Screen _currentScreen;
        public Screen CurrentScreen
        {
            get => _currentScreen;
            set
            {
                if(UIManager.Instance is not null)
                    UIManager.Instance.ChangeScreen(value);

                EnemySpawner.Instance.RefreshSpawner(value != Screen.GameOver);
                _currentScreen = value;
            }
        }

        int _score = 0;
        public int Score 
        { 
            get => _score;
            set 
            { 
                _score = value; 
                UIManager.Instance.RefreshUI(); 
            } 
        }
		
        public int Lives = 3;
		public Player Player;
		public int ScoreToGet = 10000;
		public int BulletsOnScreen;

        NameSetter nameSetter;

        public override Setup Start()
        {
            Instance = this;
            CurrentScreen = Screen.MainMenu;

            return new()
            {
                Name = "GameManager"
            };
        }

        bool isSpacePressedOld;

        float gameOverTimer;

        public override void Update(float delta)
        {
            bool isSpacePressed = window.KeyDown(Key.Space);
            bool spaceToggled = !isSpacePressed && isSpacePressedOld;
            isSpacePressedOld = isSpacePressed;

            if (Screen.MainMenu == CurrentScreen)
            {
                if(spaceToggled)
                {
                    if(Player is null)
                    {
                        Player = new Player();
                        window.Instantiate(Player);
                    }

                    CurrentScreen = Screen.Game;
                }

                return;
            }

            if(Screen.GameOver == CurrentScreen)
            {
                gameOverTimer += delta;

                if (gameOverTimer < 3)
                    return;

                if (Score != 0 && HighscoreManager.IsNewHighscore(Score))
                {
                    CurrentScreen = Screen.Highscore;

                    nameSetter = new NameSetter(FinishedWritingName);
                    window.Instantiate(nameSetter);
                }
                else
                {
                    CurrentScreen = Screen.MainMenu;
                    Score = 0;
                    Lives = 3;
                }

                return;
            }

            if (Screen.Game == CurrentScreen && Lives == 0)
            {
                CurrentScreen = Screen.GameOver;
                gameOverTimer = 0f;
                window.Destroy(Player);
                Player = null;
                return;
            }
        }

        public void FinishedWritingName(string name)
        {
            HighscoreManager.SetScore(new Highscore()
            {
                Name = name,
                Score = Score
            });

            CurrentScreen = Screen.MainMenu;
            Score = 0;
            Lives = 3;
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
