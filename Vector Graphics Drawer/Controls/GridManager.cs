using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VGE.Resources;

namespace VectorGraphicsDrawer.Controls
{
	public class GridManager : Control
	{
		public static GridManager Instance;

		bool isDrawingDisabled, hideLastLine;

		Point GridCellSize;
		Point GridOffest;

		Pen linePen, circlePen;
		FormattedText disabledText;

		List<List<Point>> shapes;

		public GridManager()
		{
			Instance = this;

			isDrawingDisabled = true;

			GridCellSize = new Point(25, 25);
			GridOffest = new Point(0, 0); //debug

			linePen = new Pen(Brushes.White);
			circlePen = new Pen(Brushes.Red, 2);

			disabledText = new FormattedText("Drawing is disabled", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 36, Brushes.SteelBlue);

			Clear();

			PointerPressed += GridManager_PointerPressed;
		}

		public override void Render(DrawingContext context)
		{
			//if that line wouldnt exist, avalonia would not detect pointer click
			context.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Bounds.Width, Bounds.Height));

			for (int x = 1; x < Bounds.Width / GridCellSize.X - 1; x++)
				for (int y = 1; y < Bounds.Height / GridCellSize.Y; y++)
				{
					if ((x - 1) % 5 == 0 && (y - 1) % 5 == 0)
						context.DrawEllipse(Brushes.Orange, null, new Point(x * GridCellSize.X, y * GridCellSize.Y), 3f, 3f);
					else
						context.DrawEllipse(Brushes.Orange, null, new Point(x * GridCellSize.X, y * GridCellSize.Y), 1f, 1f);
				}

			foreach (var shape in shapes)
			{
				for (int i = 1; i < shape.Count; i++)
					context.DrawLine(linePen, GetRealPosition(shape[i - 1]), GetRealPosition(shape[i]));

				if (shape.Count > 2 && !hideLastLine)
					context.DrawLine(linePen, GetRealPosition(shape[0]), GetRealPosition(shape[^1]));
			}

			var lastShape = shapes[^1];

			if (lastShape.Count > 2)
			{
				var lastPoint = GetRealPosition(lastShape[^1]);
				context.DrawEllipse(null, circlePen, new Rect(lastPoint.X - 5, lastPoint.Y - 5, 10, 10));
			}

			if (isDrawingDisabled)
				context.DrawText(disabledText, new Point(20, 20));
		}

		public void Clear()
		{
			shapes = new();
			AddShape();
			InvalidateVisual();
		}

		public void AddShape()
		{
			shapes.Add(new List<Point>());
		}

		public void SetShape(RawShape[] rawShapes)
		{
			Clear();

			foreach (var shape in rawShapes)
			{
				Point[] points = new Point[shape.Points.Length];

				for (int i = 0; i < shape.Points.Length; i++)
					points[i] = new Point(shape.Points[i].X, shape.Points[i].Y);

				shapes.Add(points.ToList());
			}
		}

		public void SwitchDrawing(bool isDrawingDisabled)
		{
			this.isDrawingDisabled = isDrawingDisabled;
			InvalidateVisual();
		}

		public void SwitchHideLastLine(bool hideLastLine)
		{
			this.hideLastLine = hideLastLine;
			InvalidateVisual();
		}

		public void Undo()
		{
			if (shapes.Count == 0)
				return;

			if (shapes.Count > 1 && shapes[^1].Count == 0)
			{
				shapes.RemoveAt(shapes.Count - 1);
				return;
			}

			shapes[^1].RemoveAt(shapes[^1].Count - 1);
			InvalidateVisual();
		}

		public RawShape[] Build()
		{
			List<RawShape> final = new List<RawShape>();

			foreach (var shape in shapes)
			{
				RawPoint[] skpoints = new RawPoint[shape.Count];

				for (int i = 0; i < skpoints.Length; i++)
					skpoints[i] = new RawPoint(
						Convert.ToSingle(shape[i].X),
						Convert.ToSingle(shape[i].Y));

				final.Add(new RawShape(skpoints));
			}

			return final.ToArray();
		}

		void GridManager_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
		{
			if (isDrawingDisabled)
				return;

			Point mousePosition = e.GetPosition(this);

			shapes[^1].Add(GetGridPosition(mousePosition));

			InvalidateVisual();
		}

		Point GetRealPosition(Point gridPosition) => new Point((gridPosition.X + GridOffest.X) * GridCellSize.X, (gridPosition.Y + GridOffest.Y) * GridCellSize.Y);
		Point GetGridPosition(Point realPosition) => new Point(Math.Round(realPosition.X / GridCellSize.X), Math.Round(realPosition.Y / GridCellSize.Y));
	}
}
