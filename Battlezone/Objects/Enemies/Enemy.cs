using VGE;
using VGE.Physics;

namespace Battlezone.Objects.Enemies
{
    public abstract class Enemy : PhysicsObject
    {
        //Punktacja jest zawsze wzięta stąd:
        //https://www.arcade-history.com/?n=battlezone-upright-model&page=detail&id=210
        public abstract int Score { get; }
        protected Point startPosition;

        public bool IsDead;

        public Enemy(Point startPosition)
        {
            this.startPosition = startPosition;
        }

        public override void OnDestroy()
        {
            IsDead = true;
        }
    }
}
