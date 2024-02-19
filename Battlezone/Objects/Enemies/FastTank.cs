using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE;

namespace Battlezone.Objects.Enemies
{
    public class FastTank : Tank
    {
        protected override float Speed => 30f;
        protected override int Score => 3000;

        public FastTank(Point startPosition) : base(startPosition)
        {
        }
    }
}
