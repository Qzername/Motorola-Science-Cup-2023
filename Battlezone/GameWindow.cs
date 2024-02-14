using Battlezone.Objects;
using SkiaSharp;
using VGE;
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
            Instantiate(new Player());

            Instantiate(new Cube(new Point(0,0,100)));
            Instantiate(new Cube(new Point(0,0,-100)));
            Instantiate(new Cube(new Point(100,0,0)));
            Instantiate(new Cube(new Point(-100,0,0)));
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}