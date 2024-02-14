﻿using SkiaSharp;
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
        public Point Rotation;
        public Point RotationRadians
        {
            get => new Point(Rotation.X * MathTools.Deg2rad, 
                             Rotation.Y * MathTools.Deg2rad, 
                             Rotation.Z * MathTools.Deg2rad);
        }

        public Transform()
        {
            Position = new Point();
            Rotation = Point.Zero;
        }
    }
}
