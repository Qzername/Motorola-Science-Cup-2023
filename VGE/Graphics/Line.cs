using SkiaSharp;

namespace VGE.Graphics
{
	public struct Line
	{
		public Point StartPosition;
		public Point EndPosition;
		public SKColor? LineColor;

		public Line(Point startPosition, Point endPoint, SKColor? lineColor = null)
		{
			StartPosition = startPosition;
			EndPosition = endPoint;
			LineColor = lineColor;
		}
	}
}
