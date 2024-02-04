using Tempest.Objects;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
    internal class GameWindow : Window
    {
        public GameWindow() 
        {
            Instantiate(new Map());
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}
