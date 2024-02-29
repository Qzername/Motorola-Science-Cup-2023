using VGE;
using VGE.Graphics.Shapes;
using VGE.Resources;

namespace Battlezone.Objects.Enemies
{
    public class FastTank : Tank
    {
        protected override Point defaultRotation => new Point(0, 180, 0);
        protected override PredefinedShape shape => (PredefinedShape)ResourcesHandler.Get3DShape("superTank");
        protected override float Speed => 40f;
        public override int Score => 3000;

        public FastTank(Point startPosition) : base(startPosition)
        {
        }
    }
}
