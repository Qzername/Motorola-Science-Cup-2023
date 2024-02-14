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
            obstacle1.Setup(0, 650f);           
            Instantiate(obstacle1);

            var obstacle2 = new Spike();
            obstacle2.Setup(2, 750f);            
            Instantiate(obstacle2);

            var obstacle3 = new Spike();
            obstacle3.Setup(6, 850f);           
            Instantiate(obstacle3);
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}
