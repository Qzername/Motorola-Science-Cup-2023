using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Tempest.Objects
{
    public class Player : PhysicsObject
    {
        public override int PhysicsLayer => mapPosition;
        int mapPosition = 0; //prostokąt na którym jest gracz

        const float zSpeed = 250f;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            Debug.WriteLine("PLAYER COLLISION: " + other.Name);
        }

        public override Setup Start()
        {
            MapManager.Instance.ResolutionChanged += (p) =>
            {
                transform.PerspectiveCenter = p;
            };

            return new Setup()
            {
                Name = "Player",
                Shape = new PointShape(SKColors.Red,
                                new Point(20, 20,0),
                                new Point(20, -20,0),
                                new Point(-20, -20,0),
                                new Point(-20, 20,0)),
                Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z) + new Point(0,0,400),
                PerspectiveCenter =  MapManager.Instance.PerspectivePoint,
                Rotation = Point.Zero3D
            };
        }

        bool wasApressed = false, wasDpressed = false;

        public override void Update(float delta)
        {
            //Tego W i S sterowania nie powinno być, robie to dla testów fizyki
            if (window.KeyDown(Key.W))
                transform.Position.Z += zSpeed * delta;

            if (window.KeyDown(Key.S))
                transform.Position.Z -= zSpeed * delta;

            if (window.KeyDown(Key.A) && !wasApressed)
                wasApressed = true;
            else if (!window.KeyDown(Key.A) && wasApressed)
            {
                wasApressed = false;

                if (mapPosition != 0)
                {
                    mapPosition--;
                    transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
                }
            }

            if (window.KeyDown(Key.D) && !wasDpressed)
                wasDpressed = true;
            else if (!window.KeyDown(Key.D) && wasDpressed )
            {
                wasDpressed = false;

                if(mapPosition != MapManager.Instance.Elements.Count - 1)
                {
                    mapPosition++;
                    transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
                }
            }
        }
    }
}
