using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Resources;

namespace Battlezone.Objects.UI
{
    public class LiveCounter : VectorObject
    {
        IShape tankShape;

        public bool IsEnabled;

        public override Setup Start()
        {
            tankShape = ResourcesHandler.GetShape("battlezone_shapes", "tank_icon");

            return new Setup()
            {
                Name = "LiveCounter",
                Position = new Point(0, 10)
            };
        }

        public override void Update(float delta)
        {
        }

        public void SetPosition(Point position)
        {
            transform.Position = position;
        }

        public override bool OverrideRender(Canvas canvas)
        {
            if (!IsEnabled)
                return true;

            for (int i = 0; i < GameManager.Instance.Lives; i++)
            {
                Point offset = new Point(i * (tankShape.BottomRight.X + 5), 0);

                foreach (var line in tankShape.CompiledShape)
                    canvas.DrawLine(new Line(transform.Position + line.StartPosition + offset, transform.Position + line.EndPosition + offset, SKColors.Green));
            }

            return true;
        }
    }
}
