using Battlezone.Objects.Enemies;
using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;

namespace Battlezone.Objects.UI
{
	public class Radar : VectorObject
	{
		const float radarDistance = 400f;
		const float radarHeight = 40f;
		const float scannerSpeed = 10f;

		float rotationOfScanner = 0f;

		Point[] scanner;
		PointShape enemyOnRadar;

		public override Setup Start()
		{
			scanner = [new(0, radarHeight, 0), new(0, 0, 0)];
			enemyOnRadar = new PointShape([new(-1, -1), new(-1, 1), new(1, 1), new(1, -1)]);

			return new()
			{
				Name = "Radar",
				Position = new Point(400, 50)
			};
		}

		public override void Update(float delta)
		{
			transform.Position.X = window.GetResolution().Width / 2;

			rotationOfScanner += scannerSpeed * delta;
			rotationOfScanner %= 360;
		}

		public override bool OverrideRender(Canvas canvas)
		{
			var finalScanner = PointManipulationTools.Rotate(new Point(0, 0, rotationOfScanner), scanner);

			canvas.DrawLine(new Line(finalScanner[0] + transform.Position, finalScanner[1] + transform.Position, SKColors.Aqua));

			foreach (var enemy in EnemySpawner.Instance.Enemies)
			{
				if (enemy.IsDead)
					continue;

				if (CalculateDistanceToCamera(enemy.Transform.Position) >= radarDistance)
					continue;

				if (enemy.Name == "Enemy_UFO")
					continue;

				var offset = enemy.Transform.Position - Scene3D.Camera.Position;
				var pos = new Point(radarHeight * (offset.X / radarDistance), -radarHeight * (offset.Z / radarDistance), 0);

				foreach (var line in enemyOnRadar.CompiledShape)
				{
					Point[] linePoints = [line.StartPosition + pos, line.EndPosition + pos];

					linePoints = PointManipulationTools.Rotate(new Point(0, 0, Scene3D.Camera.Rotation.Y), linePoints);

					canvas.DrawLine(new Line(linePoints[0] + transform.Position, linePoints[1] + transform.Position, SKColors.Aqua));
				}
			}

			return true;
		}

		float CalculateDistanceToCamera(Point position) => MathF.Sqrt(MathF.Pow(position.X - Scene3D.Camera.Position.X, 2) + MathF.Pow(position.Z - Scene3D.Camera.Position.Z, 2));
	}
}
