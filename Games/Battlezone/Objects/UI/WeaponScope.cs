using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Resources;
using VGE.Windows;

namespace Battlezone.Objects.UI
{
    public class WeaponScope : VectorObject
    {
        public bool IsReloading;
        public bool IsEnabled;

        const float reloadingAnimationMax = 0.5f;
        float reloadingAnimationCurrent;

        IShape shape;

        Resolution currentResolution;

        public override Setup Start()
        {
            shape = ResourcesHandler.GetShape("battlezone_shapes", "weapon_scope");

            return new()
            {
                Name = "WeaponScope"
            };
        }

        public override void Update(float delta)
        {
            currentResolution = window.GetResolution();

            transform.Position = new Point(currentResolution.Width / 2, currentResolution.Height / 2);

            if (IsReloading)
            {
                reloadingAnimationCurrent += delta;

                if (reloadingAnimationCurrent > reloadingAnimationMax)
                    reloadingAnimationCurrent = 0;
            }
            else
                reloadingAnimationCurrent = 0;
        }

        public override bool OverrideRender(Canvas canvas)
        {
            if (!IsEnabled)
                return true;

            if (reloadingAnimationCurrent > 0.25f)
                return true;

            Point topOffset = new Point(shape.BottomRight.X / 2, shape.BottomRight.Y * 1.5f);
            Point bottomOffset = new Point(shape.BottomRight.X / 2, -shape.BottomRight.Y * 1.5f);

            //górna część
            for (int i = 0; i < shape.CompiledShape.Length - 1; i++)
            {
                var line = shape.CompiledShape[i];

                canvas.DrawLine(new Line(transform.Position + (line.StartPosition - topOffset) * 10,
                                         transform.Position + (line.EndPosition - topOffset) * 10, SKColors.Green));
            }

            //dolna część
            for (int i = 0; i < shape.CompiledShape.Length - 1; i++)
            {
                var line = shape.CompiledShape[i];

                var startPoint = new Point(line.StartPosition.X, line.StartPosition.Y * -1);
                var endPoint = new Point(line.EndPosition.X, line.EndPosition.Y * -1);

                canvas.DrawLine(new Line(transform.Position + (startPoint - bottomOffset) * 10,
                                         transform.Position + (endPoint - bottomOffset) * 10, SKColors.Green));
            }

            return true;
        }
    }
}
