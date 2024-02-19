using SkiaSharp;
using System.ComponentModel;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Battlezone.Objects.Enemies
{
    public class Missle : Enemy
    {
        public override int PhysicsLayer => 1;

        const float speed = 80f;

        public override int Score => 2000;

        public Missle(Point startPosition) : base(startPosition)
        {
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
        }

        public override Setup Start()
        {
            var shape = ObstacleShapeDefinitions.GetByIndex(0);

            return new Setup()
            {
                Name = "Enemy_MISSLE",
                Position = startPosition,
                Rotation = Point.Zero,
                Shape = new PredefinedShape(shape.PointsDefinition, shape.LinesDefinition, SKColors.Yellow)
            };
        }

        public override void Update(float delta)
        {
            var pos = PositionCalculationTools.NextPositionTowardsPlayer(transform, speed, delta);

            transform.Position = pos.Item1;
            Rotate(new Point(0,pos.Item2,0));
        }
    }
}
