﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace VGE.Graphics.Scenes
{
    public class Scene3D : IScene
    {
        public static Transform Camera;
        Resolution resolution = new Resolution(800,450);
        Point centerOfScreen => new Point(resolution.Width / 2, resolution.Height / 2);

        Point viewport = new Point(400, 400);

        public void ChangeResolution(Resolution resolution)
        {
            this.resolution = resolution;
        }

        public Scene3D()
        {
            Camera = new Transform();
        }

        //https://en.wikipedia.org/wiki/3D_projection
        public void DrawObject(Canvas canvas, VectorObject vectorObject)
        {
            var shape = vectorObject.Shape;
            var transform = vectorObject.Transform;

            // --- Sprawdzanie czy punkt powinien byc renderowany ---
            //sprawdzanie czy punkt jest za drugim punktem
            //jeżeli tak jest, jeżeli dodamy punkt oddalony o 1 przed kamerą, to dystans do punktu sprawdzanego powinien się zwiększyć
            Point distanceOld = vectorObject.Transform.Position - Camera.Position;
            Point distanceNew = vectorObject.Transform.Position - PointManipulationTools.MovePointForward(Camera, 1);

            if (MathF.Pow(distanceNew.X, 2) + MathF.Pow(distanceNew.Y, 2) + MathF.Pow(distanceNew.Z, 2) >
                MathF.Pow(distanceOld.X, 2) + MathF.Pow(distanceOld.Y, 2) + MathF.Pow(distanceOld.Z, 2))
                return;

            foreach (var line in shape.CompiledShape)
            {
                Point[] points = [transform.Position + line.StartPosition - Camera.Position,
                                  transform.Position + line.EndPosition - Camera.Position];

                points = PointManipulationTools.Rotate(Camera.Rotation, points);

                for (int i = 0; i < points.Length; i++)
                {
                    var curr = points[i];
                    points[i] = new Point(400 * curr.X / curr.Z, 400 * curr.Y / curr.Z) + centerOfScreen;
                }

                //jeżeli punkt nie jest na ekranie, nie rysuj go
                if (points[0].X < -300 || points[0].X > resolution.Width+300)
                    continue;

                canvas.DrawLine(new Line(points[0], points[1], line.LineColor));
            }
        }
    }
}