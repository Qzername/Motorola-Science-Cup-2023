using SkiaSharp;
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

        public Line(Point startPosition, Point endPoint)
        {
            StartPosition = startPosition;
            EndPosition = endPoint;
        }

        public Line(float x1, float y1, float x2, float y2)
        {
            StartPosition = new Point(x1, y1);
            EndPosition = new Point(x2, y2);
        }
    }
}
