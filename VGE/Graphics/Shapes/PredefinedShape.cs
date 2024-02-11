using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE.Resources;

namespace VGE.Graphics.Shapes
{
    public class PredefinedShape : IShape
    {
        public Line[] CompiledShape => throw new NotImplementedException();

        Point center;
        public Point Center => throw new NotImplementedException();

        public Point TopLeft => throw new NotImplementedException();

        public Point BottomRight => throw new NotImplementedException();

        public SKColor? customColor;
        public SKColor? CustomColor => customColor;

        Point[] points;
        Point[] connections;

        /// <summary>
        /// Kształt który z góry ma ustalone wszystkie linie
        /// </summary>
        /// <param name="points">Koordynaty punktów</param>
        /// <param name="connections">Oznaczenie który punkt łaczy się z którym, np. new Point(0,1) będzie oznaczało że punkt 0 łaczy się z punktem 1</param>
        public PredefinedShape(Point[] points, Point[] connections)
        {
            this.points = points;
            this.connections = connections;

            CompileShape();
        }

        /// <summary>
        /// Kształt który z góry ma ustalone wszystkie linie
        /// </summary>
        /// <param name="points">Koordynaty punktów</param>
        /// <param name="connections">Oznaczenie który punkt łaczy się z którym, np. new Point(0,1) będzie oznaczało że punkt 0 łaczy się z punktem 1</param>
        public PredefinedShape(Point[] points, Point[] connections, SKColor customColor)
        {
            this.points = points;
            this.connections = connections;
            this.customColor = customColor;

            CompileShape();
        }

        public void Rotate(float angle)
        {
            //ten algorytm jest wzięty ze strony:
            //https://danceswithcode.net/engineeringnotes/rotations_in_2d/rotations_in_2d.html

            float angleRad = angle * (MathF.PI / 180);

            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            float cx = center.X, cy = center.Y;

            float temp;
            for (int n = 0; n < rawShape.Length; n++)
            {
                var current = rawShape[n];

                temp = (current.X - cx) * cos - (current.Y - cy) * sin + cx;
                rawShape[n].Y = (current.X - cx) * sin + (current.Y - cy) * cos + cy;
                rawShape[n].X = temp;
            }

            CompileShape();
        }

        void CompileShape()
        {

        }
    }
}
