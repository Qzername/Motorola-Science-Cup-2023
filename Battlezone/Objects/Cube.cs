using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Diagnostics;
using System.Net;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace Battlezone.Objects
{
    public class Cube : VectorObject
    {
        const float speed = 40, rotationSpeed = 5f;

        Transform camera = new Transform()
        {
            Position = new Point(0, 0, 0),
            Rotation = new Point(0,0,0)
        };

        Point[] linesDefinition;

        public override Setup Start()
        {
            Resolution currentResolution = window.GetResolution();
            Point centerOfScreen = new Point(currentResolution.Width / 2, currentResolution.Height / 2);

            var pointsDefinition = new Point[]
            {
                new Point(10,10,10),
                new Point(-10,10,10),
                new Point(-10,-10,10),
                new Point(10,-10,10),
                new Point(10,10,-10),
                new Point(-10,10,-10),
                new Point(-10,-10,-10),
                new Point(10,-10,-10),
            };

            linesDefinition = new Point[]
            {
                //pierwszy kwadrat
                new Point(0,1),
                new Point(1,2),
                new Point(2,3),
                new Point(3,0),
                //drugi kwadrat
                new Point(4,5),
                new Point(5,6),
                new Point(6,7),
                new Point(7,4),
                //połaczenie między kwadratami
                new Point(0,4),
                new Point(1,5),
                new Point(2,6),
                new Point(3,7),
            };

            return new Setup()
            {
                Name = "Cube",
                Position = new Point(0, 0, 100),
                Rotation = Point.Zero3D,
                Shape = new PredefinedShape(pointsDefinition, linesDefinition),
                PerspectiveCenter = new Point(centerOfScreen.X, centerOfScreen.Y, 500f)
            };
        }

        public override void Update(float delta)
        { 
            //axis position
            if (window.KeyDown(Key.Left))
                transform.Position.X -= speed* delta;
            else if(window.KeyDown(Key.Right))
                transform.Position.X += speed* delta;  

            if (window.KeyDown(Key.Up))
                transform.Position.Y -= speed* delta;
            else if(window.KeyDown(Key.Down))
                transform.Position.Y += speed* delta;  

            if (window.KeyDown(Key.U))
                transform.Position.Z += speed * delta;
            else if(window.KeyDown(Key.J))
                transform.Position.Z -= speed * delta;

            //axis rotation
            if (window.KeyDown(Key.A))
                camera.Rotation += new Point(0, -speed * delta, 0);
            else if (window.KeyDown(Key.D))
                camera.Rotation += new Point(0, speed * delta, 0);

        }

        public override void RefreshGraphics(Canvas canvas)
        {
            var resolution = window.GetResolution();
            Point centerOfScreen = new Point(resolution.Width / 2, resolution.Height / 2);

            var pointsRaw = ((PredefinedShape)Shape).Points;
            Point[] points = new Point[pointsRaw.Length];

            for (int i = 0; i < pointsRaw.Length; i++)
                points[i] = transform.Position + pointsRaw[i] - camera.Position;

            points = PointManipulationTools.Rotate(camera.Rotation, points);

            for (int i = 0; i < pointsRaw.Length; i++)
            {
                var curr = points[i];
                points[i] = new Point(400 * curr.X / curr.Z, 400 * curr.Y / curr.Z) + centerOfScreen;
            }

            foreach (var ld in linesDefinition)
                canvas.DrawLine(new Line(points[Convert.ToInt32(ld.X)], points[Convert.ToInt32(ld.Y)]));

            /*var shape = new PredefinedShape(points, linesDefinition);

            foreach (var line in shape.compiledShape)
            {
                var rawStartPoint = line.StartPosition + transform.Position;
                var rawEndPoint = line.EndPosition + transform.Position;

                canvas.DrawLine(new Line(startPoint, endPoint));
            }*/
        }
    }
}
