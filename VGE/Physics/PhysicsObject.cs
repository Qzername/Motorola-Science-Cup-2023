namespace VGE.Physics
{
    /// <summary>
    /// Rozszerzenie VectorObject umożliwiające mu kolizje z innymi obiektami fizycznymi
    /// </summary>
    public abstract class PhysicsObject : VectorObject
    {
        public bool IsColliding;
        public abstract int PhysicsLayer { get; }

        public abstract void OnCollisionEnter(PhysicsObject other);

        public bool IsDead;
    }
}
