using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace VGE.Graphics.Scenes
{
    public class Scene3D : IScene
    {
        public static Transform Camera;
        Resolution resolution = new Resolution(800,450);
        Point centerOfScreen => new Point(resolution.Width / 2, resolution.Height / 2);

        Point viewport = new Point(400, 400);

        public void ChangeResolution(Resolution resolution)
        {
            this.resolution = resolution;
        }

        public Scene3D()
        {
            Camera = new Transform();
        }

        public void DrawObject(Canvas canvas, VectorObject vectorObject)
        {
            var shape = vectorObject.Shape;
            var transform = vectorObject.Transform;

            foreach(var line in shape.CompiledShape)
            {
                Point[] points = [transform.Position + line.StartPosition - Camera.Position,
                                  transform.Position + line.EndPosition - Camera.Position];

                points = PointManipulationTools.Rotate(Camera.Rotation, points);

                for (int i = 0; i < points.Length; i++)
                {
                    var curr = points[i];
                    points[i] = new Point(400 * curr.X / curr.Z, 400 * curr.Y / curr.Z) + centerOfScreen;
                }

                canvas.DrawLine(new Line(points[0], points[1], line.LineColor));
            }
        }
    }
}
