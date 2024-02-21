using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Battlezone.Objects
{
	public class Obstacle : PhysicsObject
	{
		Point startPosition;

		public Obstacle(Point startPosition)
		{
			this.startPosition = startPosition;
		}

		public override int PhysicsLayer => 2;

		public override void OnCollisionEnter(PhysicsObject other)
		{
		}

		public override Setup Start()
		{
			var definition = ObstacleShapeDefinitions.GetRandom();

			return new Setup()
			{
				Name = "Cube",
				Position = startPosition,
				Rotation = Point.Zero,
				Shape = new PredefinedShape(definition.PointsDefinition, definition.LinesDefinition),
			};
		}

		public override void Update(float delta)
		{
		}
	}
}
