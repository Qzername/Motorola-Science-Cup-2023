namespace VGE.Physics
{
	public abstract class PhysicsObject : VectorObject
	{
		public abstract int PhysicsLayer { get; }

		public abstract void OnCollisionEnter(PhysicsObject other);

		private bool _isDead;
		public bool IsDead
		{
			get => _isDead;
			set => _isDead = value;
		}
	}
}
