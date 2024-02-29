using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
    public class Bullet : PhysicsObject
    {
        public override int PhysicsLayer => _mapPosition;
        private int _mapPosition;

        private const float ZSpeed = 900f;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.PhysicsLayer != _mapPosition || GameManager.Instance.StopGame)
                return;

            string[] names = { "BulletTanker", "Flipper", "Tanker", "Spiker", "SpikerLine", "Fuseball" };

            foreach (string name in names)
                if (name == other.Name)
                    window.Destroy(this);
        }

        public override Setup Start()
        {
            SoundRegistry.Instance.Database["fire"].PlayFromStart();

            return new Setup()
            {
                Name = "Bullet",
                Shape = new PointShape(GameManager.Instance.LevelConfig.Player,
                                new Point(5, 5, 0),
                                new Point(5, -5, 0),
                                new Point(-5, -5, 0),
                                new Point(-5, 5, 0)),
                Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z),
                Rotation = Point.Zero
            };
        }

        public void Setup(int mapPosition, float zPosition)
        {
            _mapPosition = mapPosition;
            transform.Position.Z = zPosition;
            transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z - 10);
        }

        public override void Update(float delta)
        {
            if (GameManager.Instance.StopGame)
                return;

            transform.Position.Z += ZSpeed * delta;

            if (transform.Position.Z > GameManager.Instance.LevelConfig.Length)
                window.Destroy(this);
        }
    }
}
