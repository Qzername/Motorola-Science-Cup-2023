using SkiaSharp;
using System.Diagnostics;
using System.Printing;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace Tempest.Objects
{
    public class MapElement : VectorObject
    {
        public override Setup Start()
        {
            return new Setup()
            {
                Name = "MapElement",
                Position = new Point(0,0,0),
                Shape = new PointShape(GameManager.Configuration.Tunnel,
                                    new Point(-50, 0,950),
                                    new Point(50, 0, 950),
                                    new Point(50, 0, -350),
                                    new Point(-50, 0, -350)),
                Rotation = Point.Zero,
            };
        }
        public override void Update(float delta)
        {
        }

        public void Setup(Point position)
        {
            transform.Position = position;
        }

        public Point GetCenterPosition()
        {
            var firstLine = Shape.CompiledShape[0];
            return transform.Position+
                    new Point((firstLine.StartPosition.X + firstLine.EndPosition.X) / 2,
                              (firstLine.EndPosition.Y + firstLine.StartPosition.Y)/2);
        }
    }
}
