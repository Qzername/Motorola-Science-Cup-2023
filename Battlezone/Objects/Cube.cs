using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Diagnostics;
using System.Net;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace Battlezone.Objects
{
    public class Cube : VectorObject
    {
        const float speed = 40, rotationSpeed = 5f;

        public override Setup Start()
        {
            Resolution currentResolution = window.GetResolution();
            Point centerOfScreen = new Point(currentResolution.Width / 2, currentResolution.Height / 2);

            Point[] pointsDefinition = 
            [
                new Point(10,10,10),
                new Point(-10,10,10),
                new Point(-10,-10,10),
                new Point(10,-10,10),
                new Point(10,10,-10),
                new Point(-10,10,-10),
                new Point(-10,-10,-10),
                new Point(10,-10,-10),
            ];

            Point[] linesDefinition =
            [
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
            ];

            return new Setup()
            {
                Name = "Cube",
                Position = new Point(0, 0, 100),
                Rotation = Point.Zero,
                Shape = new PredefinedShape(pointsDefinition, linesDefinition),
            };
        }
        //hubert gej
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
                Scene3D.Camera.Rotation += new Point(0, -speed * delta, 0);
            else if (window.KeyDown(Key.D))
                Scene3D.Camera.Rotation += new Point(0, speed * delta, 0);

        }
    }
}
