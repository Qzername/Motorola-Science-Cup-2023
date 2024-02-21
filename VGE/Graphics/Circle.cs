using SkiaSharp;

namespace VGE.Graphics
{
	public struct Circle(Point position, float radius, SKColor? circleColor = null)
	{
		public Point Position = position;
		public float Radius = radius;
		public SKColor? CircleColor = circleColor;
	}
}
