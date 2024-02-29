using System.Timers;
using VGE;
using VGE.Physics;

namespace Tempest
{
    public class TempestPhysicsEngine : PhysicsEngine
    {
        List<PhysicsObject> physicsObjects;

        public TempestPhysicsEngine() : base(new PhysicsConfiguration())
        {
            physicsObjects = new List<PhysicsObject>();
        }

        public override void RegisterObject(PhysicsObject obj)
        {
            physicsObjects.Add(obj);
        }

        public override void UnregisterObject(PhysicsObject obj)
        {
            physicsObjects.Remove(obj);
        }

        protected override void PhysicsUpdate(object? sender, ElapsedEventArgs e)
        {
            /*
             * Zrobiłem obejście do własnego systemu (co w sumie jest troche ironiczne)
             * Layer oznacza na którym MapElement znajduje się obiekt
             */
            for (int i = 0; i < physicsObjects.Count; i++)
            {
                var obj1 = physicsObjects[i];

                var obj1TopLeft = new Point(-10, obj1.Transform.Position.Z - 10);
                var obj1BottomRight = new Point(10, obj1.Transform.Position.Z + 10);

                for (int j = i + 1; j < physicsObjects.Count; j++)
                {
                    var obj2 = physicsObjects[j];

                    if (physicsObjects[i].PhysicsLayer == physicsObjects[j].PhysicsLayer)
                        if (PhysicsTools.CheckCollisionAABB(obj1TopLeft, obj1BottomRight,
                                new Point(-10, obj2.Transform.Position.Z - 10),
                                 new Point(10, obj2.Transform.Position.Z + 10)))
                        {
                            obj1.OnCollisionEnter(obj2);
                            obj2.OnCollisionEnter(obj1);
                        }
                }
            }

        }
    }
}
