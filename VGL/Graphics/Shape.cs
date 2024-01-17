﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace VGL.Graphics
{
    public class Shape
    {
        public Line[] CompiledShape;
        public SKPoint Center;

        SKPoint[] rawShape;

        /// <param name="defaultRotation">Obrócenie kształtu na start, dzięki temu można poprawić kszałt tak aby był skierowny na wprost</param>
        public Shape(float defaultRotation, params SKPoint[] shape)
        {
            rawShape = shape;
            CompiledShape = new Line[rawShape.Length];

            if(defaultRotation != 0)
               Rotate(defaultRotation);

            float minX = rawShape[0].X, maxX = rawShape[0].X, 
                  minY = rawShape[0].Y, maxY = rawShape[0].Y;

            for(int i = 1; i < rawShape.Length; i++)
            {
                var current = rawShape[i];

                if (current.X < minX) minX = current.X;
                else if (current.X > maxX) maxX = current.X;

                if(current.Y < minY) minY = current.Y; 
                else if (current.Y > maxY) maxY = current.Y;

                CompiledShape[i - 1] = new Line(rawShape[i - 1], current);
            }
            
            //ustawiamy środek kształtu ze względu na to że według środka będziemy mogli obracać kształt
            Center = new SKPoint((maxX - minX) / 2 + minX, (maxY - minY) / 2 + minY);

            CompiledShape[CompiledShape.Length - 1] = new Line(rawShape[CompiledShape.Length - 1], rawShape[0]);
        }

        /// <summary>
        /// Obrót kształtu 
        /// </summary>
        public void Rotate(float angle)
        {
            //ten algorytm jest wzięty ze strony:
            //https://danceswithcode.net/engineeringnotes/rotations_in_2d/rotations_in_2d.html

            float angleRad = angle * (MathF.PI / 180);

            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            float cx = Center.X, cy = Center.Y;

            float temp;
            for (int n = 0; n < rawShape.Length; n++)
            {
                var current = rawShape[n];

                temp = ((current.X - cx) * cos - (current.Y - cy) * sin) + cx;
                rawShape[n].Y = ((current.X - cx) * sin + (current.Y - cy) * cos) + cy;
                rawShape[n].X = temp;
            }

            CompileShape();
        }

        /*
         * Kompilacja kształtu, nie chcemy za każdym razem gdy rysujemy shape
         * robić te same obliczenia, więc gdy zachodzi zmiana do kształtu
         * kompilujemy wygląd do liń
         */
        void CompileShape()
        {
            for (int i = 1; i < rawShape.Length; i++)
                CompiledShape[i - 1] = new Line(rawShape[i - 1], rawShape[i]);

            CompiledShape[CompiledShape.Length - 1] = new Line(rawShape[CompiledShape.Length - 1], rawShape[0]);
        }
    }
}
