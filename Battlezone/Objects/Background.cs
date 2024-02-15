using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;

namespace Battlezone.Objects
{
    public class Background : VectorObject
    {
        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Background",
                Position = Point.Zero,
                Rotation = Point.Zero,
                Shape = new PointShape(SKColors.Gray, new Point(0,225), new Point(1200,225))
            };
        }

        public override void Update(float delta)
        {
        }

        //tło jest tak na prawdę 2D
        public override bool OverrideRender(Canvas canvas)
        {
            foreach (var l in Shape.CompiledShape)
                canvas.DrawLine(new Line(l.StartPosition + transform.Position, l.EndPosition + transform.Position, Shape.CustomColor));

            return true;
        }
    }
}
