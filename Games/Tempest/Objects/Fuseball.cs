﻿using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
    public class Fuseball : PhysicsObject
    {
        public override int PhysicsLayer => _mapPosition;
        private int _mapPosition = -1;

        private const float ZSpeed = 400f;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.PhysicsLayer != _mapPosition || GameManager.Instance.StopGame)
                return;

            if (other.Name == "Bullet")
                Die(true);
        }

        public override Setup Start()
        {
            if (_mapPosition == -1)
                _mapPosition = GameManager.Instance.Rand.Next(0, MapManager.Instance.Elements.Count);

            if (transform.Position.Z == 0)
                transform.Position.Z = GameManager.Instance.LevelConfig.Length;

            return new Setup()
            {
                Name = "Fuseball",
                Shape = new PointShape(GameManager.Instance.LevelConfig.Fuseball,
                    new Point(-5, -10),
                    new Point(-20, -20),
                    new Point(-5, -10),
                    new Point(0, 0),
                    new Point(5, -10),
                    new Point(20, -20),
                    new Point(5, -10),
                    new Point(0, 0),
                    new Point(5, 5),
                    new Point(20, 15),
                    new Point(5, 5),
                    new Point(0, 0),
                    new Point(-5, 10),
                    new Point(0, 20),
                    new Point(-5, 10),
                    new Point(0, 0),
                    new Point(-10, 5),
                    new Point(-25, 10),
                    new Point(-10, 5),
                    new Point(0, 0)
                ),
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
                Die(false);
        }

        void Die(bool killedByPlayer)
        {
            if (IsDead)
                return;

            if (killedByPlayer)
                GameManager.Instance.Score += 500;

            EnemyManager.Instance.EnemyDestroyed(this, killedByPlayer);
            IsDead = true;

            window.Destroy(this);
        }

        public override void OnDestroy()
        {
        }
    }
}
