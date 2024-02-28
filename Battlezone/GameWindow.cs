using Battlezone.Objects;
using Battlezone.Objects.Enemies;
using Battlezone.Objects.UI;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Physics;
using VGE.Windows;

namespace Battlezone
{
	internal class GameWindow : Window
	{
		SoundRegistry soundRegistry;

		public GameWindow() : base(new Scene3D(Settings.RenderDistance))
		{
			PhysicsConfiguration configuration = new PhysicsConfiguration()
			{
				LayerConfiguration = new Dictionary<int, int[]>()
				{
					{ 0, [1,2] }, //0 -> player
                    { 1, [0,2] }, //1 -> enemies 
                    { 2, [1,2] }, //2 -> obstacles
                }
			};

			RegisterPhysicsEngine(new PhysicsEngine(configuration));

			soundRegistry = new SoundRegistry();
			soundRegistry.InitializeSounds(this);

            Instantiate(new EnemySpawner());

            Instantiate(new Background());
			Instantiate(new ObstacleGenerator());

            Instantiate(new UIManager());

            Instantiate(new GameManager());
		}

		public override void Update(Canvas canvas)
		{
		}
	}
}