using SkiaSharp;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Resources;

namespace Battlezone.Objects.Enemies
{
	public class Missle : Enemy
	{
		public override int PhysicsLayer => 1;

		const float speed = 20f;

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
				Position = startPosition + new Point(0,-10,0),
				Rotation = Point.Zero,
				Shape = (PredefinedShape)ResourcesHandler.Get3DShape("missle")
        };
		}

		public override void Update(float delta)
		{
			var pos = PositionCalculationTools.NextPositionTowardsPlayer(transform, speed, delta);

			transform.Position = pos.Item1;
			Rotate(new Point(0, pos.Item2, 0));
		}
	}
}
