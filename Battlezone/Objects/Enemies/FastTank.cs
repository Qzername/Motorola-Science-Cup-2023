using VGE;
using VGE.Graphics.Shapes;
using VGE.Resources;

namespace Battlezone.Objects.Enemies
{
	public class FastTank : Tank
	{
        protected override PredefinedShape shape => (PredefinedShape)ResourcesHandler.Get3DShape("superTank");
        protected override float Speed => 30f;
		public override int Score => 3000;

		public FastTank(Point startPosition) : base(startPosition)
		{
		}
	}
}
