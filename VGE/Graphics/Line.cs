﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE.Graphics
{
    public struct Line
    {
        public Point StartPosition;
        public Point EndPosition;
        public SKColor? LineColor;

        public Line(Point startPosition, Point endPoint, SKColor? lineColor = null)
        {
            StartPosition = startPosition;
            EndPosition = endPoint;
            LineColor = lineColor;
        }
    }
}
