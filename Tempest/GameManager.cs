using HML;
using HML.Objects;
using Tempest.Objects;
using Tempest.Objects.UI;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
    public class GameManager : VectorObject
    {
        public static GameManager Instance;

        public Random Rand = new();

        public LevelConfiguration LevelConfig = new();
        public Level CurrentLevel = Levels.Circle;
        private int _currentLevelIndex = 1;
        public int CurrentLevelIndex
        {
            get => _currentLevelIndex;
            set
            {
                _currentLevelIndex = value;
                UIManager.Instance.RefreshUI();
            }
        }
        public Player Player;
        public MapManager MapManager;

        private bool _isSpacePressedOld;
        private float _gameOverTimer;
        private NameSetter _nameSetter;

        public int MapPosition;
        public int Lives = 4;

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                UIManager.Instance.RefreshUI();
            }
        }

        public bool StopGame;
        public bool ChangingMap;

        public bool SpawningEnemies;
        public int EnemiesToSpawn = 5;

        private Screen _currentScreen;
        public Screen CurrentScreen
        {
            get => _currentScreen;
            set
            {
                if (UIManager.Instance is not null)
                    UIManager.Instance.ChangeScreen(value);

                if (value is Screen.Game)
                    StartLevel();

                _currentScreen = value;
            }
        }

        public override Setup Start()
        {
            Instance = this;
            CurrentScreen = Screen.MainMenu;

            return new()
            {
                Name = "GameManager"
            };
        }

        public override void Update(float delta)
        {
            bool isSpacePressed = window.KeyDown(Key.Space);
            bool spaceToggled = !isSpacePressed && _isSpacePressedOld;
            _isSpacePressedOld = isSpacePressed;

            if (Screen.MainMenu == CurrentScreen)
            {
                if (spaceToggled)
                    CurrentScreen = Screen.Game;

                return;
            }

            if (Screen.GameOver == CurrentScreen)
            {
                _gameOverTimer += delta;

                if (_gameOverTimer < 3)
                    return;

                if (Score != 0 && HighscoreManager.IsNewHighscore(Score))
                {
                    CurrentScreen = Screen.Highscore;

                    _nameSetter = new NameSetter(FinishedWritingName);
                    window.Instantiate(_nameSetter);
                }
                else
                {
                    CurrentScreen = Screen.MainMenu;
                    Score = 0;
                    Lives = 4;
                    CurrentLevelIndex = 1;
                    CurrentLevel = Levels.Circle;
                }

                return;
            }

            if (Screen.Game == CurrentScreen && Lives == 0)
            {
                StopGame = true;
                CurrentScreen = Screen.GameOver;
                _gameOverTimer = 0f;

                window.Destroy(Player);
                Player = null;
                window.Destroy(MapManager);
                MapManager = null;
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
            Lives = 4;
            CurrentLevelIndex = 1;
            CurrentLevel = Levels.Circle;
        }

        public override bool OverrideRender(Canvas canvas) => true;

        public async void StartLevel(bool wait = false)
        {
            StopGame = true;

            if (wait)
                await Task.Delay(3000);
            DestroyEverything();

            TempestScene.Instance.PerspectiveOffset = new(0, 0, 0);

            if (CurrentScreen == Screen.GameOver)
                return;

            SpawningEnemies = false;
            MapPosition = 0;

            MapManager = new MapManager();
            window.Instantiate(MapManager);
            Player = new Player();
            window.Instantiate(Player);

            StopGame = false;

            await Task.Run(EnemyManager.Instance.SpawnEnemies);
        }

        private void DestroyEverything()
        {
            EnemyManager.Instance.DestroyEnemies(true);
            window.Destroy(Player);
            Player = null;
            window.Destroy(MapManager);
            MapManager = null;
        }

        public async Task NextLevel()
        {
            for (int i = 1; i <= 10; i++)
            {
                TempestScene.Instance.PerspectiveOffset = new(0, 0, 1000 * i);
                await Task.Delay(50);
            }

            EnemiesToSpawn += 3;

            if (Levels.Circle.Indexes.Contains(CurrentLevelIndex)) // Square
                CurrentLevel = Levels.Square;
            else if (Levels.Square.Indexes.Contains(CurrentLevelIndex)) // Plus
                CurrentLevel = Levels.Plus;
            else if (Levels.Plus.Indexes.Contains(CurrentLevelIndex)) // BowTie
                CurrentLevel = Levels.BowTie;
            else if (Levels.BowTie.Indexes.Contains(CurrentLevelIndex)) // StylizedCross
                CurrentLevel = Levels.StylizedCross;
            else if (Levels.StylizedCross.Indexes.Contains(CurrentLevelIndex)) // Triangle
                CurrentLevel = Levels.Triangle;
            else if (Levels.Triangle.Indexes.Contains(CurrentLevelIndex)) // Clover
                CurrentLevel = Levels.Clover;
            else if (Levels.Clover.Indexes.Contains(CurrentLevelIndex)) // V
                CurrentLevel = Levels.V;
            else if (Levels.V.Indexes.Contains(CurrentLevelIndex)) // Steps
                CurrentLevel = Levels.Steps;
            else if (Levels.Steps.Indexes.Contains(CurrentLevelIndex)) // U
                CurrentLevel = Levels.U;
            else if (Levels.U.Indexes.Contains(CurrentLevelIndex)) // CompletelyFlat
                CurrentLevel = Levels.CompletelyFlat;
            else if (Levels.CompletelyFlat.Indexes.Contains(CurrentLevelIndex)) // Heart
                CurrentLevel = Levels.Heart;
            else if (Levels.Heart.Indexes.Contains(CurrentLevelIndex)) // Star
                CurrentLevel = Levels.Star;
            else if (Levels.Star.Indexes.Contains(CurrentLevelIndex)) // W
                CurrentLevel = Levels.W;
            else if (Levels.W.Indexes.Contains(CurrentLevelIndex)) // Fan
                CurrentLevel = Levels.Fan;
            else if (Levels.Fan.Indexes.Contains(CurrentLevelIndex)) // Infinity
                CurrentLevel = Levels.Infinity;
            else if (Levels.Infinity.Indexes.Contains(CurrentLevelIndex)) // Circle
            {
                LevelConfig.ChangeColorScheme();
                CurrentLevel = Levels.Circle;
            }
            else
                CurrentLevel = Levels.List[Rand.Next(0, Levels.List.Count + 1)];

            CurrentLevelIndex++;

            if (CurrentLevelIndex == 2)
                LevelConfig.MoveFlipper = true;
            else if (CurrentLevelIndex == 3)
                LevelConfig.SpawnTanker = true;
            else if (CurrentLevelIndex == 4)
                LevelConfig.SpawnSpiker = true;
            else if (CurrentLevelIndex == 5)
                LevelConfig.SpawnFire = true;
            else if (CurrentLevelIndex == 11)
                LevelConfig.SpawnFuseball = true;
        }
    }
}