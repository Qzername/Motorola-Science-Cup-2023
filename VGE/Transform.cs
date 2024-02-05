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
        public Point Position;
        public float Rotation;
        public float RotationRadians
        {
            get => Rotation * (MathF.PI / 180);
        }

        //3D options
        public bool Is3D => Position.Is3D;
        public Point? PerspectiveCenter;

        public Transform()
        {
            Position = new Point();
            Rotation = 0f;
        }
    }
}
