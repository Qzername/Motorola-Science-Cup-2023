using Tempest.Objects;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
    internal class GameWindow : Window
    {
        public GameWindow() 
        {
            Instantiate(new MapManager());
            Instantiate(new Player());
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}
