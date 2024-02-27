using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;

namespace Tempest.Objects.UI
{
	public class LifeCounter : VectorObject
	{
		Point startPosition;

		PointShape playerShape;

		public LifeCounter(Point startPosition)
		{
			this.startPosition = startPosition;

			playerShape = new PointShape(GameManager.Instance.LevelConfig.Player,
				new Point(0, 20),
				new Point(20, 0),
				new Point(-20, 0)
			);
			playerShape.Rotate(new Point(0, 0, 180));
		}

		public override Setup Start()
		{
			return new()
			{
				Position = startPosition,
				Name = "LiveCounter"
			};
		}

		public override void Update(float delta)
		{
		}

		public override bool OverrideRender(Canvas canvas)
		{
			if (GameManager.Instance is null)
				return true;

			Point offset = Point.Zero;

			for (int i = 0; i < GameManager.Instance.Lives; i++)
			{
				foreach (var line in playerShape.CompiledShape)
					canvas.DrawLine(new Line(transform.Position + offset + line.StartPosition, transform.Position + offset + line.EndPosition));

				offset.X += playerShape.BottomRight.X * 2 + 10;
			}

			return true;
		}
	}
}
