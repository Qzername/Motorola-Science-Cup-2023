using Tempest.Objects;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
    internal class GameWindow : Window
    {
        public GameWindow() : base(new TempestScene())
        {
            GameManager.Configuration = new();
            RegisterPhysicsEngine(new TempestPhysicsEngine());

            Instantiate(new MapManager());
            Instantiate(new Player());
            Instantiate(new Flipper());
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}
