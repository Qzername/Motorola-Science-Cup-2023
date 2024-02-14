using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Windows;

namespace Battlezone.Objects
{
    public class Player : VectorObject
    {
        const float speed = 40;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Player",
                Position = Point.Zero,
                Rotation = Point.Zero,
                Shape = null
            };
        }

        public override void Update(float delta)
        {
            //movement
            if(window.KeyDown(Key.W))
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, speed * delta);
            else if(window.KeyDown(Key.S))
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, -speed * delta);

            //rotation
            if (window.KeyDown(Key.A))
                Scene3D.Camera.Rotation += new Point(0, -speed * delta, 0);
            else if (window.KeyDown(Key.D))
                Scene3D.Camera.Rotation += new Point(0, speed * delta, 0);        }

        public override bool OverrideRender(Canvas canvas)
        {
            //nie rysuj nic
            return true;
        }
    }
}
