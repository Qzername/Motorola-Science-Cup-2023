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
            Text text = new Text();
            Instantiate(text);
            text.Setup("Score: 12", 3, new SKPoint(10,10));
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}