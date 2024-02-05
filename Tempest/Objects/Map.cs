using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest.Objects
{
    public class Map : VectorObject
    {
        Resolution baseResolution;
        
        SKPoint centerOfScreen { get => new SKPoint(baseResolution.Width / 2f, baseResolution.Height / 2f); }

        const float zSpeed = 50f;

        public override Setup Start()
        {
            baseResolution = window.GetResolution();

            Debug.WriteLine(baseResolution.Width);

            return new Setup()
            {
                Name = "Map",
                Position = new Point(centerOfScreen.X, centerOfScreen.Y+10, 50),
                Shape = new Shape(0, new Point(10, 0, 0),
                                    new Point(60, 0, 0)),
                Rotation = 0f,
                PerspectiveCenter = new Point(centerOfScreen.X, centerOfScreen.Y, 1000f)
            };
        }

        public override void RefreshGraphics(Canvas canvas)
        {
            base.RefreshGraphics(canvas);

            foreach (var l in Shape.CompiledShape)
                canvas.DrawLine(new Line(l.StartPosition + transform.Position, l.EndPosition + transform.Position));
        }

        public override void Update(float delta)
        {
            baseResolution = window.GetResolution();

            if (window.KeyDown(Key.W))
                transform.Position.Z -= zSpeed * delta;
            else if(window.KeyDown(Key.S))
                transform.Position.Z += zSpeed * delta;

            if (window.KeyDown(Key.Left))
                transform.Position.X -= zSpeed * delta;
            else if (window.KeyDown(Key.Right))
                transform.Position.X += zSpeed * delta;

            if (window.KeyDown(Key.Up))
                transform.Position.Y -= zSpeed * delta;
            else if (window.KeyDown(Key.Down))
                transform.Position.Y += zSpeed * delta;
        }
    }
}
