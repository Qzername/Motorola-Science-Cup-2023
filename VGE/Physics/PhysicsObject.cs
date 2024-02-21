namespace VGE.Physics
{
	public abstract class PhysicsObject : VectorObject
	{
		public abstract int PhysicsLayer { get; }

		public abstract void OnCollisionEnter(PhysicsObject other);
	}
}
