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

        const float speed = 20f;//80f;

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
                Rotation = new Point(0,110,0),
                Shape = new PredefinedShape(shape.PointsDefinition, shape.LinesDefinition, SKColors.Yellow)
            };
        }

        public override void Update(float delta)
        {
            //angle
            Point offset = transform.Position - Scene3D.Camera.Position;

            float c = MathF.Sqrt(MathF.Pow(offset.X, 2) + MathF.Pow(offset.Z, 2));

            float sinAngleToPlayer = offset.Z / c;
            float angleToPlayer = (MathF.Asin(sinAngleToPlayer) * MathTools.Rad2deg);

            Debug.WriteLine(offset);

            if(offset.X  >0)
                Rotate(new Point(0, angleToPlayer - transform.Rotation.Y, 0));
            else
                Rotate(new Point(0, ((angleToPlayer * -1f )- transform.Rotation.Y) , 0));

            //next position
            float cNext = (delta * speed);

            float z = sinAngleToPlayer * cNext;
            float x = (offset.X / c) * cNext;

            transform.Position.X -= x;
            transform.Position.Z -= z;
        }
    }
}
