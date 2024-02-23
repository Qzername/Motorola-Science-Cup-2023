using VGE;
using VGE.Graphics.Shapes;

namespace Tempest.Objects
{
	public class MapElement : VectorObject
	{
		private float _length, _rotation;
		public float Length => _length;

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "MapElement",
				Position = transform.Position,
				Shape = new PointShape(GameManager.LevelConfig.Tunnel,
									new Point(0, 0, 950),
									new Point(_length, 0, 950),
									new Point(_length, 0, -350),
									new Point(0, 0, -350)),
				Rotation = new Point(0, 0, _rotation),
			};
		}

		public override void Update(float delta)
		{
		}

		public void Setup(Point position, float length, float rotation)
		{
			transform.Position = position;
			_length = length;
			_rotation = rotation;
		}

		public Point GetCenterPosition()
		{
			var firstLine = Shape.CompiledShape[0];
			return transform.Position +
					new Point((firstLine.StartPosition.X + firstLine.EndPosition.X) / 2,
							  (firstLine.EndPosition.Y + firstLine.StartPosition.Y) / 2);
		}
	}
}
