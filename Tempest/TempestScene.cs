using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Windows;

namespace Tempest
{
    internal class TempestScene : IScene
    {
        public static TempestScene Instance;

        public TempestScene()
        {
            Instance = this;
        }

        Point perspectivePoint = new Point(400, 225);

        public void ChangePerspectivePoint(Point perspectivePoint)
        {
            this.perspectivePoint = perspectivePoint;
        }

        public void UpdateResolution(Resolution resolution)
        {
            
        }

		public void DrawObject(Canvas canvas, VectorObject vectorObject)
        {
            var shape = vectorObject.Shape;
            var transform = vectorObject.Transform;

            foreach (var l in shape.CompiledShape)
            {
                //Długość Z jest inna dla każdego punktu, tutaj obliczenia na nowy:
                var startPosition = transform.Position + l.StartPosition;
                var endPosition = transform.Position + l.EndPosition;

                float deltaSP = perspectivePoint.Z / startPosition.Z;
                float deltaEP = perspectivePoint.Z / endPosition.Z;

                Line finalLine = new Line()
                {
                    StartPosition = new Point(startPosition.X * deltaSP, startPosition.Y * deltaSP) + perspectivePoint,
                    EndPosition = new Point(endPosition.X * deltaEP, endPosition.Y * deltaEP) + perspectivePoint,
                    LineColor = shape.CustomColor
                };

                canvas.DrawLine(finalLine);
            }
        }
    }
}
