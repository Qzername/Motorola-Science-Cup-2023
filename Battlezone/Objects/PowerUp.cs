using Battlezone.Objects.UI;
using System.Security.Cryptography;
using VGE;
using VGE.Physics;
using VGE.Resources;

namespace Battlezone.Objects
{
    public class PowerUp : PhysicsObject
    {
        public override int PhysicsLayer => 2;

        Point startPosition;

        float animationCycle = 0, animationSpeed = 20f;

        public PowerUp(Point startPosition)
        {
            this.startPosition = startPosition; 
        }

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other is not Bullet)
                return;

            int type = new Random().Next(0, 4);

            switch(type)
            { //potężne te powerupy wiem, ale będą też rzadkie
                case 0: GameManager.Instance.Player.MagazineMax *= 2; break;
                case 1: GameManager.Instance.Player.ReloadingTime /= 2; break;
                case 2: GameManager.Instance.Player.Speed += 20; break;
                case 3: GameManager.Instance.Player.ShootCooldownTime /= 2; break;
            }

            window.Instantiate(new PowerUpAnimation());

            window.Destroy(this);
        }

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "PowerUp",
                Shape = ResourcesHandler.Get3DShape("Obstacles/rhombus"),
                Position = startPosition - new Point(0,5,0)
            };
        }

        public override void Update(float delta)
        {
            animationCycle += delta * animationSpeed;

            transform.Position.Y += MathF.Sin(animationCycle);

            Rotate(new(0, animationSpeed * delta, 0));
        }
    }
}
