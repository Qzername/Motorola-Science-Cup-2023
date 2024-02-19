using SkiaSharp;
using System.ComponentModel;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Battlezone.Objects.Enemies
{
    public class Tank : Enemy
    {
        EnemyCollider front;

        const float colliderDistance = 5f;

        protected virtual float Speed => 10f;
        const float bulletFrequency = 4f;
        float currentTimer = 0f;

        public Tank(Point startPosition) : base(startPosition)
        {
        }

        public override int PhysicsLayer => 1;

        protected override int Score => 1000;

        public override void OnCollisionEnter(PhysicsObject other)
        {
        }

        public override Setup Start()
        {
            front = new EnemyCollider();
            window.Instantiate(front);

            front.UpdatePosition(PointManipulationTools.MovePointForward(RecalculateRotation(), colliderDistance));
            
            var shape = ObstacleShapeDefinitions.GetByIndex(0);

            return new Setup()
            {
                Name = "Enemy_TANK",
                Position = startPosition,
                Rotation = Point.Zero,
                Shape = new PredefinedShape(shape.PointsDefinition, shape.LinesDefinition, SKColors.Red)
            };
        }

        public override void Update(float delta)
        {
            front.UpdatePosition(PointManipulationTools.MovePointForward(RecalculateRotation(), colliderDistance));

            if (!front.IsColliding)
            {
                //position
                var pos = PositionCalculationTools.NextPositionTowardsPlayer(transform, Speed, delta);

                transform.Position = pos.Item1;
                Rotate(new Point(0, pos.Item2, 0));

                //shooting
                currentTimer += delta;
            }

            if (currentTimer < bulletFrequency)
                return;

            window.Instantiate(new EnemyBullet(RecalculateRotation()));

            currentTimer = 0f;
        }

        Transform RecalculateRotation()
        {
            var offset = Scene3D.Camera.Position - transform.Position;

            return new Transform()
            {
                Position = transform.Position,
                Rotation = new Point(0, 180 - transform.Rotation.Y - 90 + (offset.X < 0 && offset.Z < 0 ? 180 : 0), 0)
            };
        }
    }
}
