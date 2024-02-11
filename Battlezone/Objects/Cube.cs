using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace Battlezone.Objects
{
    public class Cube : VectorObject
    {
        const float speed = 40;

        public override Setup Start()
        {
            Resolution currentResolution = window.GetResolution();
            Point centerOfScreen = new Point(currentResolution.Width/2 , currentResolution.Height/2);

            return new Setup()
            {
                Name = "Cube",
                Position = new Point(0, 0, 100),
                Rotation = 0f,
                Shape = new PointShape(0f, new Point(-10,10,0), 
                                      new Point(10, 10,0),
                                      new Point(10, -10,0),
                                      new Point(-10, -10,0)),
                PerspectiveCenter = new Point(centerOfScreen.X, centerOfScreen.Y, 500f)
            };
        }

        public override void Update(float delta)
        { 
            if (window.KeyDown(Key.Left))
                transform.Position.X += speed* delta;
            else if(window.KeyDown(Key.Right))
                transform.Position.X -= speed* delta;  

            if (window.KeyDown(Key.Up))
                transform.Position.Y -= speed* delta;
            else if(window.KeyDown(Key.Down))
                transform.Position.Y += speed* delta;  

            if (window.KeyDown(Key.W))
                transform.Position.Z -= speed * delta;
            else if(window.KeyDown(Key.S))
                transform.Position.Z += speed * delta;  
        }
    }
}
