using VGE;

namespace Tempest
{
	public class Level(Point[] layout, bool isClosed, int[] indexes)
	{
		public Point[] Layout = layout;
		public bool IsClosed = isClosed;
		public int[] Indexes = indexes;
	}
}
