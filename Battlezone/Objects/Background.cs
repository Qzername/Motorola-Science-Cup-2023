using SkiaSharp;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;

namespace Battlezone.Objects
{
    public class Background : VectorObject
    {
        const int maxX = 700; //ostatni punkt z shape

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Background",
                Position = Point.Zero,
                Rotation = Point.Zero,
                Shape = new PointShape(SKColors.Gray, 
                    new Point(0,0), 
                    new Point(50,-50), 
                    new Point(100,-25), 
                    new Point(200,-12,5), 
                    new Point(250,-35), 
                    new Point(300,0), 
                    new Point(400,0), 
                    new Point(450,-20),
                    new Point(500,-20),
                    new Point(600,-70),
                    new Point(700,0))
            };
        }

        public override void Update(float delta)
        {
        }

        //tło jest tak na prawdę 2D
        public override bool OverrideRender(Canvas canvas)
        {
            var res = window.GetResolution();

            canvas.DrawLine(new Line(new Point(0, res.Height / 2, 0), new(1200, res.Height / 2, 0), SKColors.Gray));

            float backgroundRotation = Scene3D.Camera.Rotation.Y*MathF.PI;

            if(backgroundRotation < 0)
                backgroundRotation = 360+backgroundRotation + (Convert.ToInt32(backgroundRotation/360) * -360);

            float offsetRatio = backgroundRotation / 360;
            float offsetWidth = Convert.ToInt32(offsetRatio * maxX);

            int amountToRender = (res.Width + Convert.ToInt32(offsetWidth)) / maxX;

            for (int i = 0; i < amountToRender+1; i++)
                foreach (var l in Shape.CompiledShape)
                    canvas.DrawLine(new Line()
                    {
                        StartPosition = new Point(l.StartPosition.X - offsetWidth + i*maxX, l.StartPosition.Y + res.Height/2),
                        EndPosition = new Point(l.EndPosition.X - offsetWidth + i*maxX, l.EndPosition.Y+ res.Height/2),
                        LineColor = SKColors.Gray
                    });

            return true;
        }
    }
}
