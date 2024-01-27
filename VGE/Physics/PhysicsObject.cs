using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE.Physics
{
    public abstract class PhysicsObject : VectorObject
    {
        public abstract int PhysicsLayer { get; }

        public abstract void OnCollisionEnter(PhysicsObject other);
    }
}
