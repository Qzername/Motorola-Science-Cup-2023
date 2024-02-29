using Tempest.Objects;
using Tempest.Objects.UI;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
	internal class GameWindow : Window
	{
		SoundRegistry soundRegistry;

		public GameWindow() : base(new TempestScene())
		{
			RegisterPhysicsEngine(new TempestPhysicsEngine());

			soundRegistry = new SoundRegistry();
			soundRegistry.InitializeSounds(this);

			Instantiate(new GameManager());
			Instantiate(new EnemyManager());
			Instantiate(new UIManager());
		}

		public override void Update(Canvas canvas)
		{

		}
	}
}
