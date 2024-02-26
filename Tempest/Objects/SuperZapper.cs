using System.Diagnostics;
using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace Tempest.Objects
{
	public class SuperZapper : VectorObject
	{
		private bool _isZPressed;
		public bool IsUsed;

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "SuperZapper",
				Shape = new PointShape(SKColors.Empty,
					new Point(0, 0, 0),
					new Point(0, 0, 0)),
				Position = Point.Zero,
				Rotation = Point.Zero
			};
		}

		public override void Update(float delta)
		{
			if (GameManager.StopGame || IsUsed)
				return;

			if (window.KeyDown(Key.Z) && !_isZPressed)
				_isZPressed = true;
			else if (!window.KeyDown(Key.Z) && _isZPressed)
			{
				IsUsed = true;
				_isZPressed = false;
				((GameWindow)window).DestroyEnemies();
			}
		}

		public override bool OverrideRender(Canvas canvas)
		{
			return true;
		}
	}
}
