using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Battlezone.Objects
{
    /// <summary>
    /// Służy do wykrywania czy gracz może się poruszyć (do przodu, do tyłu) czy nie
    /// </summary>
    public class PlayerCollider : PhysicsObject
    {
        public bool IsColliding = false;

        public override int PhysicsLayer => 0;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            IsColliding = true;
        }

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Collider",
                Position = Point.Zero,
                Shape = new PointShape(new Point(20, 0, 20), new Point(-20, 0, 20), new Point(-20, 0, -20), new Point(20, 0, -20)),
            };
        }

        public void UpdatePosition(Point position)
        {
            transform.Position = position;
        }

        public override void Update(float delta)
        {
        }

        public override bool OverrideRender(Canvas canvas)
        {
            //nie rysuj nic
            return true;
        }
    }
}
