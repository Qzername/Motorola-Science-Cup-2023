using Battlezone.Objects;
using SkiaSharp;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Objects;
using VGE.Windows;

namespace Battlezone
{
    internal class GameWindow : Window
    {
        public GameWindow() : base(new Scene3D())
        {
            Instantiate(new Cube());
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}