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
	    private float length, rotation;
        
        public override Setup Start()
        {
            return new Setup()
            {
                Name = "MapElement",
                Position = transform.Position,
                Shape = new PointShape(GameManager.Configuration.Tunnel,
                                    new Point(0, 0,950),
                                    new Point(length, 0, 950),
                                    new Point(length, 0, -350),
                                    new Point(0, 0, -350)),
                Rotation = new Point(0, 0, rotation),
            };
        }
        
        public override void Update(float delta)
        {
        }

        public void Setup(Point position, float length, float rotation)
        {
            transform.Position = position;
            this.length = length;
            this.rotation = rotation;
        }

        public Point GetCenterPosition()
        {
            var firstLine = Shape.CompiledShape[0];
            return transform.Position+
                    new Point((firstLine.StartPosition.X + firstLine.EndPosition.X) / 2,
                              (firstLine.EndPosition.Y + firstLine.StartPosition.Y) / 2);
        }
    }
}
