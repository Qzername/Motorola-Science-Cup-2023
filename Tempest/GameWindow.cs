using Tempest.Objects;
using Tempest.Objects.UI;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
	internal class GameWindow : Window
	{
		public GameWindow() : base(new TempestScene())
		{
			RegisterPhysicsEngine(new TempestPhysicsEngine());

			Instantiate(new GameManager());
			Instantiate(new EnemyManager());
			Instantiate(new UIManager());
		}

		public override void Update(Canvas canvas)
		{

		}
	}
}
