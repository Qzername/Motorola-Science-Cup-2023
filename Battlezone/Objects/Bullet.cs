using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Battlezone.Objects
{
    public class Bullet : PhysicsObject
    {
        const float Speed = 50f;

        public override int PhysicsLayer => 0;

        Transform startTransform;

        public Bullet(Transform startTransform)
        {
            this.startTransform = startTransform;
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if(other.Name == "Cube")
            {
                window.Destroy(other);
                window.Destroy(this);
            }
        }

        public override Setup Start()
        {
            Point[] pointsDefinition =
            [
                new Point(1, 1, 2),
                new Point(-1, 1, 2),
                new Point(-1, -1, 2),
                new Point(1, -1, 2),
                new Point(1, 1, -2),
                new Point(-1, 1, -2),
                new Point(-1, -1, -2),
                new Point(1, -1, -2),
            ];

            Point[] linesDefinition =
            [
                //pierwszy kwadrat
                new Point(0, 1),
                new Point(1, 2),
                new Point(2, 3),
                new Point(3, 0),
                //drugi kwadrat
                new Point(4, 5),
                new Point(5, 6),
                new Point(6, 7),
                new Point(7, 4),
                //połaczenie między kwadratami
                new Point(0, 4),
                new Point(1, 5),
                new Point(2, 6),
                new Point(3, 7),
            ];

            Debug.WriteLine(startTransform.Rotation);

            return new Setup()
            {
                Name = "Bullet",
                Shape = new PredefinedShape(pointsDefinition, linesDefinition),
                Position = startTransform.Position,
                Rotation = startTransform.Rotation,
            };
        }

        public override void Update(float delta)
        {
            transform.Position = PointManipulationTools.MovePointForward(transform, Speed * delta);
        }
    }
}
