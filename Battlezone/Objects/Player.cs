using Battlezone.Objects.Enemies;
using Battlezone.Objects.UI;
using VGE;
using VGE.Audio.Default;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Battlezone.Objects
{
    public class Player : PhysicsObject
    {
        PlayerCollider back, front;

        public override int PhysicsLayer => 0;

        public float Speed = 40;
        const float colliderDistance = 30f;

        public int MagazineMax = 3;
        int magazine = 3;

        public float ReloadingTime = 3f;
        public float ShootCooldownTime = 1f;
        float reloadingTimer = 0f, shootCooldownTimer = 1f;

        bool IsDead = false;

        NASound engineIdle, engineMove;

        public override Setup Start()
        {
            engineIdle = SoundRegistry.Instance.Database["engineidle"];
            engineIdle.IsLooped = true;

            engineMove = SoundRegistry.Instance.Database["engine"];
            engineMove.IsLooped = true;

            back = new PlayerCollider();
            window.Instantiate(back);

            front = new PlayerCollider();
            window.Instantiate(front);

            RefreshCollidersPosition();

            var shape = ObstacleShapeDefinitions.GetByIndex(0);

            return new Setup()
            {
                Name = "Player",
                Position = Point.Zero,
                Rotation = Point.Zero,
                Shape = new PredefinedShape(shape.PointsDefinition, shape.LinesDefinition),
            };
        }

        bool lastSpaceState;

        public override void Update(float delta)
        {
            if (IsDead)
                return;

            if (back.IsColliding && front.IsColliding)
            {
                back.IsColliding = false;
                front.IsColliding = false;
            }

            //movement
            if (window.KeyDown(Key.W) && !front.IsColliding)
            {
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, Speed * delta);
                RefreshCollidersPosition();

                if (back.IsColliding)
                    back.IsColliding = false;

                engineIdle.Pause();
                engineMove.Play();
            }
            else if (window.KeyDown(Key.S) && !back.IsColliding)
            {
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, -Speed * delta);
                RefreshCollidersPosition();

                if (front.IsColliding)
                    front.IsColliding = false;

                engineIdle.Pause();
                engineMove.Play();
            }
            else
            {
                engineIdle.Play();
                engineMove.Pause();
            }

            transform.Position = Scene3D.Camera.Position;

            //rotation
            if (window.KeyDown(Key.A))
                Scene3D.Camera.Rotation += new Point(0, -Speed * delta, 0);
            else if (window.KeyDown(Key.D))
                Scene3D.Camera.Rotation += new Point(0, Speed * delta, 0);

            //bullet
            if (magazine == 0)
            {
                GameManager.Instance.IsReloading = true;
                reloadingTimer += delta;

                if (ReloadingTime < reloadingTimer)
                {
                    magazine = MagazineMax;
                    reloadingTimer = 0;
                    shootCooldownTimer = 10f;
                    GameManager.Instance.IsReloading = false;
                }

                return;
            }

            if (shootCooldownTimer < ShootCooldownTime)
            {
                shootCooldownTimer += delta;
                return;
            }

            bool currentSpaceState = window.KeyDown(Key.Space);

            if (currentSpaceState && !lastSpaceState)
            {
                magazine--;

                window.Instantiate(new Bullet(Scene3D.Camera));
                lastSpaceState = true;
                shootCooldownTimer = 0f;
            }
            else if (!currentSpaceState && lastSpaceState)
                lastSpaceState = false;
        }

        public override bool OverrideRender(Canvas canvas)
        {
            //nie rysuj nic
            return true;
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name != "Enemy_MISSLE" && other.Name != "Bullet")
                return;

            window.Destroy(other);

            IsDead = true;

            //animacja śmierci
            window.Instantiate(new DeathAnimation(this));
        }

        /// <summary>
        /// Ustawienia pozwalające na respawn gracza
        /// </summary>
        public void Respawn()
        {
            EnemySpawner.Instance.SpawnEnemies = true;

            Scene3D.Camera = new Transform()
            {
                Position = Point.Zero,
                Rotation = Point.Zero
            };

            EnemySpawner.Instance.DestroyAllObjects();

            IsDead = false;

            front.IsColliding = false;
            back.IsColliding = false;

            //shooting
            GameManager.Instance.IsReloading = false;
            shootCooldownTimer = ShootCooldownTime;
            reloadingTimer = 0f;
            magazine = MagazineMax;
        }

        void RefreshCollidersPosition()
        {
            front.UpdatePosition(PointManipulationTools.MovePointForward(Scene3D.Camera, colliderDistance));
            back.UpdatePosition(PointManipulationTools.MovePointForward(Scene3D.Camera, -colliderDistance));
        }
    }
}
