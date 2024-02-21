namespace VGE.Windows
{
	public struct Resolution(int width, int height)
	{
		public int Width { get; } = width;
		public int Height { get; } = height;
	}
}
