using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGL.Graphics
{
    public struct Line
    {
        public SKPoint StartPosition;
        public SKPoint EndPosition;

        public Line(SKPoint startPosition, SKPoint endPoint)
        {
            StartPosition = startPosition;
            EndPosition = endPoint;
        }

        public Line(float x1, float y1, float x2, float y2)
        {
            StartPosition = new SKPoint(x1, y1);
            EndPosition = new SKPoint(x2, y2);
        }
    }
}
