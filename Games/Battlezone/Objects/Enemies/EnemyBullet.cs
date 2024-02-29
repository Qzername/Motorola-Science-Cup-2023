using VGE;

namespace Battlezone.Objects.Enemies
{
    public class EnemyBullet : Bullet
    {
        public override int PhysicsLayer => 1;

        public EnemyBullet(Transform startTransform) : base(startTransform)
        {
        }
    }
}
