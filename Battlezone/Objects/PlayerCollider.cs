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
		public override int PhysicsLayer => 0;

		public override void OnCollisionEnter(PhysicsObject other)
		{
		}

		public override Setup Start()
		{
			var shape = ObstacleShapeDefinitions.GetByIndex(0);

			return new Setup()
			{
				Name = "Collider",
				Position = Point.Zero,
				Shape = new PredefinedShape(shape.PointsDefinition, shape.LinesDefinition),
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
			return true;
		}
	}
}
