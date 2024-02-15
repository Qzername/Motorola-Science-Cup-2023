using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Battlezone.Objects
{
    public class Cube : PhysicsObject
    {
        Point startPosition;

        public Cube(Point startPosition)
        {
            this.startPosition = startPosition;
        }

        public override int PhysicsLayer => 1;

        public override void OnCollisionEnter(PhysicsObject other)
        {
        }

        public override Setup Start()
        {
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
                Position = startPosition,
                Rotation = Point.Zero,
                Shape = new PredefinedShape(pointsDefinition, linesDefinition),
            };
        }

        public override void Update(float delta)
        { 
        }
    }
}
