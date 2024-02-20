using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE;
using VGE.Graphics.Shapes;

namespace ShapeRenderer
{
    public class ObjectToRender : VectorObject
    {
        public override Setup Start()
        {
            return new()
            {
                Name = nameof(ObjectToRender),
                Shape = new PointShape([Point.Zero]),
                Position = new Point(0,0,0),
                Rotation = Point.Zero,
            };
        }

        public void UpdateShape(PredefinedShape shape)
        {
            Shape = shape;
        }

        public override void Update(float delta)
        {
        }
    }
}
