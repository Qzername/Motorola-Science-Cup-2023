using Battlezone.Objects;
using SkiaSharp;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Battlezone
{
    internal class GameWindow : Window
    {
        public GameWindow() : base()
        {
            Instantiate(new Cube());
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}