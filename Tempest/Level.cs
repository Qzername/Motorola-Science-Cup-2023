using VGE;

namespace Tempest
{
	public class Level(Point[] layout, bool isClosed, int[] indexes)
	{
		public readonly Point[] Layout = layout;
		public readonly bool IsClosed = isClosed;
		public readonly int[] Indexes = indexes;
	}
}
