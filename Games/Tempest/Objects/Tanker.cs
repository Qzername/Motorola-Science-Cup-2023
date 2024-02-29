using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
    public class Tanker : PhysicsObject
    {
        public override int PhysicsLayer => _mapPosition;
        private int _mapPosition = -1;
        private readonly Timer _bulletTimer = new();

        // Strzelaj co 1 do 2 sekund
        private const int MinShootDelay = 1000;
        private const int MaxShootDelay = 2001;

        private const float ZSpeed = 200f;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.PhysicsLayer != _mapPosition || GameManager.Instance.StopGame)
                return;

            if (other.Name == "Bullet")
                Split(true);
        }

        public override Setup Start()
        {
            if (_mapPosition == -1)
                _mapPosition = GameManager.Instance.Rand.Next(0, MapManager.Instance.Elements.Count);

            if (transform.Position.Z == 0)
                transform.Position.Z = GameManager.Instance.LevelConfig.Length;

            _bulletTimer.Elapsed += TimerShoot;
            _bulletTimer.Interval = GameManager.Instance.Rand.Next(MinShootDelay, MaxShootDelay);
            _bulletTimer.Enabled = true;

            return new Setup()
            {
                Name = "Tanker",
                Shape = new PointShape(GameManager.Instance.LevelConfig.Tanker,
                                new Point(0, 20, 0),
                                new Point(20, 0, 0),
                                new Point(0, -20, 0),
                                new Point(-20, 0, 0)),
                Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z),
                Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
            };
        }

        public void Setup(int mapPosition, float zPosition = 1600)
        {
            _mapPosition = mapPosition;
            transform.Position.Z = zPosition;
        }

        public override void Update(float delta)
        {
            if (GameManager.Instance.StopGame)
                return;

            if (transform.Position.Z > 400)
                transform.Position.Z -= ZSpeed * delta;
            else
                Split(false);
        }

        void TimerShoot(object? sender, ElapsedEventArgs e)
        {
            if (GameManager.Instance.StopGame)
                return;

            _bulletTimer.Interval = GameManager.Instance.Rand.Next(MinShootDelay, MaxShootDelay);

            var bullet = new Spike();
            bullet.Setup(_mapPosition, transform.Position.Z);
            window.Instantiate(bullet);
        }

        void Split(bool killedByPlayer)
        {
            EnemyManager.Instance.SplitTanker(this);
            Die(killedByPlayer);
        }

        void Die(bool killedByPlayer)
        {
            if (IsDead)
                return;

            if (killedByPlayer)
                GameManager.Instance.Score += 100;

            EnemyManager.Instance.EnemyDestroyed(this, killedByPlayer);
            IsDead = true;

            window.Destroy(this);
        }

        public override void OnDestroy()
        {
            _bulletTimer.Close();
            _bulletTimer.Enabled = false;
        }
    }
}
