using VGE;

namespace Battlezone.Objects.Enemies
{
	public class FastTank : Tank
	{
		protected override float Speed => 30f;
		public override int Score => 3000;

		public FastTank(Point startPosition) : base(startPosition)
		{
		}
	}
}
