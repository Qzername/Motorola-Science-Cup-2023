using SkiaSharp;
using System.Diagnostics;
using System.Timers;

namespace VGL.Physics
{
    public class PhysicsEngine
    {
        System.Timers.Timer physicsTimer;

        const int physicsFramerate = 50;

        PhysicsConfiguration physicsConfiguration;

        public event Action<VectorObject, VectorObject> CollisionDetected;

        /*
         * JAK TO DZIAŁA:
         * w celu optymalizacji zrobiłem fizyke opartom na warstwach
         * wykorzystujemy to w np. asteroids, w którym asteroidy przelatują przez siebie, ale nie przez gracza
         */
        Dictionary<int, List<VectorObject>> objects;

        public PhysicsEngine(PhysicsConfiguration configuration)
        {
            objects = new Dictionary<int, List<VectorObject>>();

            physicsConfiguration = configuration;

            //timer
            physicsTimer = new System.Timers.Timer(1000 / physicsFramerate);
            physicsTimer.Elapsed += PhysicsUpdate;
            physicsTimer.Enabled = true;
        }

        public void RegisterObjects(int layer, List<VectorObject> objs)
        {
	        if (!objects.ContainsKey(layer))
		        objects[layer] = new List<VectorObject>();

	        foreach (VectorObject obj in objs)
	        {
	            var foundObject = objects[layer].Where(x => x.Guid == obj.Guid).FirstOrDefault();
	            if (foundObject == null)
		            objects[layer].Add(obj);
	        }
		}

		public void RegisterObject(int layer, VectorObject obj)
        {
            if (!objects.ContainsKey(layer))
                objects[layer] = new List<VectorObject>();

            var foundObject = objects[layer].Where(x => x.Guid == obj.Guid).FirstOrDefault();
            if (foundObject == null)
	            objects[layer].Add(obj);
		}

        public void UnRegisterObject(int layer, VectorObject obj)
        {
	        if (!objects.ContainsKey(layer))
		        objects[layer] = new List<VectorObject>();

	        var foundObject = objects[layer].Where(x => x.Guid == obj.Guid).FirstOrDefault();
            if (foundObject != null)
                objects[layer].RemoveAt(objects[layer].IndexOf(foundObject));
        }

		void PhysicsUpdate(object? sender, ElapsedEventArgs e)
        {
            List<int> checkedLayer = new List<int>();

            /*
             * niestety cztery zagnieżdzone foreache
             * ale ta metoda pozwoli na pewną optymalizacje tak czy inaczej
             */

            foreach (var layer in physicsConfiguration.LayerConfiguration)
            {
                checkedLayer.Add(layer.Key);

                var objectsInCurrentLayer = objects[layer.Key].ToArray(); 

                foreach (var collidingLayer in layer.Value)
                {
                    if (checkedLayer.Contains(collidingLayer))
                        continue;

                    var objectsInCollidingLayer = objects[collidingLayer].ToArray();

                    //faktycznie sprawdzanie kolizji
                    foreach(var obj1 in objectsInCurrentLayer)
                        foreach(var obj2 in objectsInCollidingLayer)
                            if (PhysicsTools.CheckCollisionAABB(
                                obj1.Transform.Position + obj1.Shape.TopLeft,
                                obj1.Transform.Position + obj1.Shape.BottomRight,
                                obj2.Transform.Position + obj2.Shape.TopLeft,
                                obj2.Transform.Position + obj2.Shape.BottomRight))
                                CollisionDetected?.Invoke(obj1, obj2);

                }
            }

        }
    }
}
