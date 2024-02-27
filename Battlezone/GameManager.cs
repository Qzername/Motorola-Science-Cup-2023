using Battlezone.Objects;
using Battlezone.Objects.Enemies;
using Battlezone.Objects.UI;
using HML;
using HML.Objects;
using VGE;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Battlezone
{
	public class GameManager : VectorObject
	{
		public static GameManager Instance;

		int score = 0;
		int lives = 3;
		bool isReloading = false;

		public int Score
		{
			get => score;
			set
			{
				score = value;
				UIManager.Instance.RefreshUI();
			}
		}

		public int Lives
		{
			get => lives;
			set
			{
				lives = value;
				UIManager.Instance.RefreshUI();
			}
		}

		public bool IsReloading
		{
			get => isReloading;
			set
			{
				isReloading = value;
				UIManager.Instance.RefreshUI();
			}
		}

		Screen currentScreen;

		public Player Player;

        public override Setup Start()
        {
			currentScreen = Screen.MainMenu;

			Instance = this;

			UIManager.Instance.ChangeUIStatus(currentScreen);

			return new()
			{
				Name = "GameManager"
			};
        }

		bool lastSpaceStatus = false;

		float gameoverMax = 2.5f;
		float gameoverTimer = 0f;

        public override void Update(float delta)
        {
			bool currentSpaceStatus = window.KeyDown(Key.Space);

			if(currentScreen == Screen.MainMenu)
				if(lastSpaceStatus && !currentSpaceStatus)
				{
					currentScreen = Screen.Game;

					Lives = 3;
					Score = 0;
					IsReloading =false;

					EnemySpawner.Instance.SpawnEnemies = true;

					Player = new Player();
					window.Instantiate(Player);

					UIManager.Instance.ChangeUIStatus(currentScreen);
				}

			if(currentScreen == Screen.Game && Lives == 0)
			{
				currentScreen = Screen.GameOver;

                EnemySpawner.Instance.SpawnEnemies = false;
                EnemySpawner.Instance.DestroyAllObjects();

                window.Destroy(Player);
				Player = null;

				UIManager.Instance.ChangeUIStatus(currentScreen);
				lastSpaceStatus = false;
            }

			if (currentScreen == Screen.GameOver)
			{
				gameoverTimer += delta;

                if (gameoverTimer > gameoverMax)
                {
                    if (Score != 0 && HighscoreManager.IsNewHighscore(Score))
                    {
                        currentScreen = Screen.HighScore;
                        window.Instantiate(new NameSetter(FinishedWritingName));
                    }
                    else
                        currentScreen = Screen.MainMenu;

                    UIManager.Instance.ChangeUIStatus(currentScreen);

					gameoverTimer = 0f;
                }
            }

            lastSpaceStatus = currentSpaceStatus;
        }

		public void FinishedWritingName(string name)
		{
			HighscoreManager.SetScore(new Highscore() { Name = name, Score = score });
			currentScreen = Screen.MainMenu;

            UIManager.Instance.ChangeUIStatus(currentScreen);
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
