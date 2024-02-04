using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE;
using VGE.Graphics;

namespace Battlezone.Objects
{
    public class Cube : VectorObject
    {
        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Cube",
                Position = new SKPoint(0, 0),
                Rotation = 0f,
                Shape = new Shape(0f, new SKPoint(0,0), new SKPoint(10, 0)),
            };
        }

        public override void Update(float delta)
        {
        }
    }
}
