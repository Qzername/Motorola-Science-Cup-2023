using Asteroids.Objects;
using Asteroids.Objects.UI;
using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Objects;
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

        public override void Update(float delta)
        {
            bool isSpacePressed = window.KeyDown(Key.Space);
            bool spaceToggled = !isSpacePressed && isSpacePressedOld;
            isSpacePressedOld = isSpacePressed;

            if (Screen.MainMenu == CurrentScreen)
            {
                if(spaceToggled)
                {
                    Debug.WriteLine("tak");

                    Player = new Player();
                    window.Instantiate(Player);

                    CurrentScreen = Screen.Game;
                }

                return;
            }

            if(Screen.GameOver == CurrentScreen)
            {
                if (spaceToggled)
                {
                    isSpacePressedOld = false;
                    CurrentScreen = Screen.MainMenu;
                    Score = 0;
                    Lives = 3;
                }

                return;
            }

            if (Screen.Game == CurrentScreen && Lives == 0)
            {
                CurrentScreen = Screen.GameOver;
                window.Destroy(Player);
                return;
            }
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
