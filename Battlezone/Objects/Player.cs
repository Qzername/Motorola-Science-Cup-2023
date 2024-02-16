using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Battlezone.Objects
{
    public class Player : PhysicsObject
    {
        public override int PhysicsLayer => 0;

        const float speed = 40;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Player",
                Position = Point.Zero,
                Rotation = Point.Zero,
                Shape = new PointShape(new Point(20, 0,20), new Point(-20, 0, 20), new Point(-20,0,-20), new Point(20,0,-20)),
            };
        }

        bool lastSpaceState;

        public override void Update(float delta)
        {
            Debug.WriteLine(Scene3D.Camera.Rotation);
            //movement
            if (window.KeyDown(Key.W))
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, speed * delta);
            else if(window.KeyDown(Key.S))
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, -speed * delta);

            //rotation
            if (window.KeyDown(Key.A))
                Scene3D.Camera.Rotation += new Point(0, -speed * delta, 0);
            else if (window.KeyDown(Key.D))
                Scene3D.Camera.Rotation += new Point(0, speed * delta, 0);


            //bullet
            bool currentSpaceState = window.KeyDown(Key.Space);

            if (currentSpaceState && !lastSpaceState)
            {
                window.Instantiate(new Bullet(Scene3D.Camera));
                lastSpaceState = true;
            }
            else if (!currentSpaceState && lastSpaceState)
                lastSpaceState = false;
        }

        public override bool OverrideRender(Canvas canvas)
        {
            //nie rysuj nic
            return true;
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
        }
    }
}
