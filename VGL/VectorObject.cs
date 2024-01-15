using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGL.Graphics;

namespace VGL
{
    public class VectorObject
    {
        Line[] shape;
        Transform transform;

        public VectorObject(Line[] shape)
        {
            this.shape = shape;
            transform = new Transform();
        }
        public VectorObject(Line[] shape, Transform transform)
        {
            this.shape = shape;
            this.transform = transform;
        }
        public VectorObject(Line[] shape, SKPoint position, float rotation)
        {
            this.shape = shape;
            this.transform = new Transform()
            {
                Position = position,
                Rotation = rotation
            };
        }

        public void Draw(Canvas canvas)
        {
            foreach (var l in shape)
                canvas.DrawLine(new Line(l.StartPosition + transform.Position, l.EndPosition + transform.Position));
        }
    }
}
