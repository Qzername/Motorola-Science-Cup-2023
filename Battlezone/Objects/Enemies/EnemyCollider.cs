using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battlezone.Objects.Enemies
{
    public class EnemyCollider : PlayerCollider
    {
        public override int PhysicsLayer => 1;
    }
}
