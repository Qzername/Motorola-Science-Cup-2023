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

        float z = 150f;
        const float zSpeed = 50f;

        public override Setup Start()
        {
            baseResolution = window.GetResolution();

            Debug.WriteLine(baseResolution.Width);

            return new Setup()
            {
                Name = "Map",
                Position = centerOfScreen,
                Shape = new Shape(0, new SKPoint(-50,-50), 
                                    new SKPoint(50, -50),
                                    new SKPoint(50, 50),
                                    new SKPoint(-50, 50)),
                Rotation = 0f,
            };
        }

        public override void Update(float delta)
        {
            baseResolution = window.GetResolution();
            transform.Position = centerOfScreen;

            if (window.KeyDown(Key.Up))
                z -= zSpeed * delta;
            else if(window.KeyDown(Key.Down))
                z += zSpeed * delta;
        }

        public override void RefreshGraphics(Canvas canvas)
        {
            foreach(var l in Shape.CompiledShape)
            {
                Line finalLine = new Line() 
                {
                    StartPosition = CalculatePerspective(transform.Position + l.StartPosition),
                    EndPosition = CalculatePerspective(transform.Position + l.EndPosition)
                };

                canvas.DrawLine(finalLine);
            }
        }

        SKPoint CalculatePerspective(SKPoint p)
        {
            SKPoint perspectivePoint = centerOfScreen;

            float alpha = MathF.Atan2(p.X - perspectivePoint.X, p.Y - perspectivePoint.Y);
            float beta = MathF.PI - alpha; //180 stopni - alpha

            float x = MathF.Sin(beta) * z;
            float y = MathF.Cos(beta) * z;

            return new SKPoint(perspectivePoint.X + x, perspectivePoint.Y + y);
        }
    }
}
