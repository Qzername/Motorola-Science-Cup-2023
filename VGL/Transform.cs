using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGL
{
    public struct Transform
    {
        public SKPoint Position;
        public float Rotation;

        public Transform()
        {
            Position = new SKPoint();
            Rotation = 0f;
        }
    }
}
