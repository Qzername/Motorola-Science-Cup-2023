using SkiaSharp;
using System.Diagnostics;
using VGL;
using VGL.Graphics;

namespace Sandbox
{
    internal class GameWindow : Window
    {
        float x = 0;
        float speed = 50;

        VectorObject player;

        public GameWindow() : base()
        {
            player = new VectorObject(
            [
                new Line(0,0,30,80),
                new Line(30,80,60,0),
                new Line(60,0,30,10),
                new Line(30,10,0,0),
            ], new SKPoint(150,150), 60f);
        }

        public override void Update(Canvas canvas)
        {/*
            x += speed * time.DeltaTime;

            canvas.DrawLine(new Line(x, 0, 100, 100));
            canvas.DrawLine(new Line(0, x, 100, 100));
            canvas.DrawLine(new Line(0, 0, x, 100));
            canvas.DrawLine(new Line(0, 0, 100, x));
            canvas.DrawLine(new Line(x, x, 100, 100));*/

            player.Draw(canvas);
        }
    }
}
