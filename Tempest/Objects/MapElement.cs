using SkiaSharp;
using System.Diagnostics;
using System.Printing;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest.Objects
{
    public class MapElement : VectorObject
    {
        Resolution baseResolution;
        SKPoint centerOfScreen { get => new SKPoint(baseResolution.Width / 2f, baseResolution.Height / 2f); }

        const float zSpeed = 250f;

        Point perspectiveOffset;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "MapElement",
                Position = new Point(0,0),
                Shape = new Shape(0, SKColors.Blue,
                                    new Point(-50, 0,950),
                                    new Point(50, 0, 950),
                                    new Point(50, 0, -350),
                                    new Point(-50, 0, -350)),
                Rotation = 0,
                PerspectiveCenter = new Point(0, 0),
            };
        }
        public override void Update(float delta)
        {
            GetPerspective();
        }

        public void Setup(Point position, Point perspectiveOffset)
        {
            transform.Position = position;
            this.perspectiveOffset = perspectiveOffset;
        }

        public Point GetCenterPosition()
        {
            var firstLine = Shape.CompiledShape[0];
            return transform.Position+
                    new Point((firstLine.StartPosition.X + firstLine.EndPosition.X) / 2,
                              (firstLine.EndPosition.Y + firstLine.StartPosition.Y)/2);
        }

        void GetPerspective()
        {
            if (baseResolution == window.GetResolution())
                return;

            baseResolution = window.GetResolution();
            transform.PerspectiveCenter = new Point(centerOfScreen.X, centerOfScreen.Y, 0) + perspectiveOffset;
        }
    }
}
