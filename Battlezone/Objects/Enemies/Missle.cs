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

        protected override int Score => 2000;

        public Missle(Point startPosition) : base(startPosition)
        {
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
        }

        public override Setup Start()
        {
            var shape = ObstacleShapeDefinitions.GetByIndex(1);

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
            var rotationTowardsPlayer = MathF.Atan2((transform.Position.Z - Scene3D.Camera.Position.Z) * MathTools.Deg2rad,
                                                    (transform.Position.X - Scene3D.Camera.Position.X) * MathTools.Deg2rad) * MathTools.Rad2deg;

            Rotate(new Point(0, rotationTowardsPlayer-transform.Rotation.Y+90, 0));

            transform.Position = PointManipulationTools.MovePointForward(transform, speed * delta);
        }
    }
}
