using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;

namespace Asteroids.Objects
{
    public class Player : PhysicsObject
    {
        const float minSpeed = 0, maxSpeed = 500;
        float speed, rotationSpeed = 90, prevRotation, prevRotationRadias;
    
        bool lastSpaceState, respawnShield;

        public override int PhysicsLayer => (int)PhysicsLayers.Player;

        public override void OnCollisionEnter(PhysicsObject other)
        {
	        if (other.Name == "Obstacle")
	        {
		        if (respawnShield)
		        {
                    window.Destroy(other);
					return;                    
		        }
                
		        window.Destroy(this);
                GameManager.Lives--;
                
                if (GameManager.Lives == 0)
                {
					window.Close();
					return;
                }

				respawnShield = true;
                
				Thread.Sleep(2000);
                
                Resolution res = window.GetResolution();
				transform.Position = new SKPoint(res.Width / 2f, res.Height / 2f);
                transform.Rotation = 0f;
                prevRotation = Transform.Rotation;
                prevRotationRadias = Transform.RotationRadians;      
                speed = 0;
                
                window.Instantiate(this);
                
                Thread.Sleep(5000);
                
                respawnShield = false;
			}
		}

        public override Setup Start()
        {
	        Resolution res = window.GetResolution();
            
            return new Setup()
            {
                Name = "Player",
                Shape = new Shape(-90f,
                        new SKPoint(0, 0),
                        new SKPoint(15, 40),
                        new SKPoint(30, 0),
                        new SKPoint(15, 5)),
                Position = new SKPoint(res.Width / 2f, res.Height / 2f),
                Rotation = 0f,
            };
        }

        public override void Update(float deltaTime)
        {
            bool isSpacePressed = window.KeyDown(Key.Space);

            if (isSpacePressed && !lastSpaceState)
            {
                var bullet = new Bullet();
                window.Instantiate(bullet);
                bullet.Setup(transform.Position + Shape.CompiledShape[0].EndPosition, transform.Rotation);

                lastSpaceState = true;
            }
            else if (!isSpacePressed && lastSpaceState)
                lastSpaceState = false;

            float sin = MathF.Sin(prevRotationRadias);
            float cos = MathF.Cos(prevRotationRadias);

            float speedDelta = speed * deltaTime;
            float rotationDelta = rotationSpeed * deltaTime;

            if (window.KeyDown(Key.LeftShift) || window.KeyDown(Key.RightShift))
                rotationDelta *= 2;

            if (window.KeyDown(Key.Left) || window.KeyDown(Key.A))
                Rotate(rotationDelta * -1f);
            else if (window.KeyDown(Key.Right) || window.KeyDown(Key.D))
                Rotate(rotationDelta);

            if (window.KeyDown(Key.Up) || window.KeyDown(Key.W))
            {
                if (ShouldSlow())
                {
                    if (speed - maxSpeed / 100 > 20)
                        speed -= maxSpeed / 100;
                    else
                    {
                        speed = 20;
						prevRotation = transform.Rotation;
						prevRotationRadias = transform.RotationRadians;
					}
                }
                else
                {
                    if (speed + maxSpeed / 100 < maxSpeed)
                        speed += maxSpeed / 100;
                    else
                        speed = maxSpeed;

					prevRotation = transform.Rotation;
					prevRotationRadias = transform.RotationRadians;
				}

				transform.Position.X += cos * speedDelta;
                transform.Position.Y += sin * speedDelta;
            }
            else if (window.KeyDown(Key.Down) || window.KeyDown(Key.S))
            {
                transform.Position.X += cos * speedDelta;
                transform.Position.Y += sin * speedDelta;

                if (speed - maxSpeed / 500 > minSpeed)
                    speed -= maxSpeed / 500;
                else
                    speed = minSpeed;
            }
            else
            {
                transform.Position.X += cos * speedDelta;
                transform.Position.Y += sin * speedDelta;

                if (speed - maxSpeed / 1000 > minSpeed)
                    speed -= maxSpeed / 1000;
                else
                    speed = minSpeed;
            }

            Resolution res = window.GetResolution();

			if (transform.Position.X < 0)
                transform.Position = new SKPoint(res.Width, transform.Position.Y);
            else if (transform.Position.X > res.Width)
                transform.Position = new SKPoint(0, transform.Position.Y);
            else if (transform.Position.Y < 0)
                transform.Position = new SKPoint(transform.Position.X, res.Height);
            else if (transform.Position.Y > res.Height)
                transform.Position = new SKPoint(transform.Position.X,0);
        }

        bool ShouldSlow()
        {
            float tPrevRotation = 360 + prevRotation;
            float tRotation = 360 + transform.Rotation;

            if (tRotation - 45 <= tPrevRotation && tPrevRotation <= tRotation + 45)
                return false;

            return true;
        }
    }
}
