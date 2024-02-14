using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE
{
    public struct Point
    {
        public float X;
        public float Y;
        public float Z;

        public static Point Zero = new Point(0, 0, 0);

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public static implicit operator SKPoint(Point p)
        {
            return new SKPoint(p.X, p.Y);
        }

        public static implicit operator Point(SKPoint p)
        {
            return new Point(p.X, p.Y);
        }

        public static Point operator +(Point p1, Point p2) => new Point(p1.X+p2.X, p1.Y+p2.Y, p1.Z + p2.Z);

        public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

        public static implicit operator string(Point p) => $"X: {p.X}, Y: {p.Y}, Z: {p.Z}";
    }
}
