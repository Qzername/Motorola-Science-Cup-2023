using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE
{
    public struct Transform
    {
        public SKPoint Position;
        public float Rotation;
        public float RotationRadians
        {
            get => Rotation * (MathF.PI / 180);
        }

        public Transform()
        {
            Position = new SKPoint();
            Rotation = 0f;
        }
    }
}
