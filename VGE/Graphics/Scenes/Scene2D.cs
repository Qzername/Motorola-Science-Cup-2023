using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using VGE.Windows;

namespace VGE.Graphics.Scenes
{
    public class Scene2D : IScene
    {
        public void DrawObject(Canvas canvas, VectorObject vectorObject)
        {
            var shape = vectorObject.Shape;
            var transform = vectorObject.Transform;

            foreach (var l in shape.CompiledShape)
                canvas.DrawLine(new Line(l.StartPosition + transform.Position, l.EndPosition + transform.Position, shape.CustomColor));
        }

        public void UpdateResolution(Resolution resolution)
        {
            
        }
    }
}
