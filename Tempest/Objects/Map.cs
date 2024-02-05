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

            return new Setup()
            {
                Name = "Map",
                Position = new Point(0, 50, 200),
                Shape = new Shape(0, new Point(-50, 0, 0),
                                    new Point(50, 0, 0),
                                    new Point(50, 0, -150),
                                    new Point(-50, 0, -150)),
                Rotation = 0f,
                PerspectiveCenter = new Point(centerOfScreen.X, centerOfScreen.Y, 100f)
            };
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
