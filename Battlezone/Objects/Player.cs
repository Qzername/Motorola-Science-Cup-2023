using Battlezone.Objects.Enemies;
using System.Diagnostics;
using VGE;
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

        const float speed = 40;
        const float colliderDistance = 10f;

        int magazine = 3;

        const float reloadingTime = 3f;
        const float shootCooldownTime = 1f;
        float reloadingTimer = 0f, shootCooldownTimer = 1f;

        public override Setup Start()
        {
            back = new PlayerCollider();
            window.Instantiate(back);

            front =new PlayerCollider();
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
            //movement
            if (window.KeyDown(Key.W) && !front.IsColliding)
            {
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, speed * delta);
                RefreshCollidersPosition();

                if (back.IsColliding)
                    back.IsColliding = false;
            }
            else if(window.KeyDown(Key.S) && !back.IsColliding)
            {
                Scene3D.Camera.Position = PointManipulationTools.MovePointForward(Scene3D.Camera, -speed * delta);
                RefreshCollidersPosition();

                if (front.IsColliding)
                    front.IsColliding = false;
            }

            transform.Position = Scene3D.Camera.Position;

            //rotation
            if (window.KeyDown(Key.A))
                Scene3D.Camera.Rotation += new Point(0, -speed * delta, 0);
            else if (window.KeyDown(Key.D))
                Scene3D.Camera.Rotation += new Point(0, speed * delta, 0);

            //bullet
            if (magazine == 0)
            {
                GameManager.IsReloading = true;
                reloadingTimer += delta;

                if (reloadingTime < reloadingTimer)
                {
                    magazine = 3;
                    reloadingTimer = 0;
                    shootCooldownTimer = 10f;
                    GameManager.IsReloading = false;
                }

                return;
            }

            if(shootCooldownTimer < shootCooldownTime)
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

            GameManager.Lives--;

            if (GameManager.Lives == 0)
                window.Close();

            EnemySpawner.Instance.DestroyAllObjects();
            Scene3D.Camera = new Transform()
            {
                Position = Point.Zero,
                Rotation = Point.Zero
            };

            front.IsColliding = false;
            back.IsColliding = false;

            //shooting
            GameManager.IsReloading = false;
            shootCooldownTimer = shootCooldownTime;
            reloadingTimer = 0f;
            magazine = 3;
        }

        void RefreshCollidersPosition()
        {
            front.UpdatePosition(PointManipulationTools.MovePointForward(Scene3D.Camera, colliderDistance));
            back.UpdatePosition(PointManipulationTools.MovePointForward(Scene3D.Camera, -colliderDistance));
        }
    }
}
