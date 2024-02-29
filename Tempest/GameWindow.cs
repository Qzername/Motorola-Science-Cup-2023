using Tempest.Objects;
using Tempest.Objects.UI;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
    internal class GameWindow : Window
    {
        SoundRegistry soundRegistry;

        static WindowConfiguration windowConfiguration = new WindowConfiguration()
        {
            Name = "Tempest",
            Size = new Point(1280, 720)
        };

        public GameWindow() : base(windowConfiguration, new TempestScene())
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
