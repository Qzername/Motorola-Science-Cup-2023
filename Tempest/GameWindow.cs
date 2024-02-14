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

            var obstacle1 = new Spike();
            Instantiate(obstacle1);
            obstacle1.Setup(0, 650f);

            var obstacle2 = new Spike();
            Instantiate(obstacle2);
            obstacle2.Setup(2, 750f);

            var obstacle3 = new Spike();
            Instantiate(obstacle3);
            obstacle3.Setup(4, 850f);
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}
