using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Battlezone.Objects
{
    public class Bullet : PhysicsObject
    {
        const float speed = 150f;
        const float maxDistance = 1000f;

        float distance;

        public override int PhysicsLayer => 0;

        Transform startTransform;

        public Bullet(Transform startTransform)
        {
            distance = 0f;
            this.startTransform = startTransform;
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name != "Cube")
                window.Destroy(other);

            window.Destroy(this);
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

            return new Setup()
            {
                Name = "Bullet",
                Shape = new PredefinedShape(pointsDefinition, linesDefinition),
                Position = startTransform.Position,
                Rotation = startTransform.Rotation * -1f,
            };
        }

        public override void Update(float delta)
        {
            distance += speed * delta;

            if (distance >= maxDistance)
                window.Destroy(this);

            transform.Position = PointManipulationTools.MovePointForward(new Transform()
            {
                Position =transform.Position,
                Rotation = startTransform.Rotation
            }, speed * delta);
        }
    }
}
