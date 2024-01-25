using SkiaSharp;
using VGE;
using VGE.Graphics;

namespace Asteroids.Objects
{
    public class Player : VectorObject
    {
        const float minSpeed = 0, maxSpeed = 300;
        float speed = 0, rotationSpeed = 90, prevRotation, prevRotationRadias;
    
        bool lastSpaceState;

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Player",
                Shape = new Shape(-90f,
                        new SKPoint(0, 0),
                        new SKPoint(15, 40),
                        new SKPoint(30, 0),
                        new SKPoint(15, 5)),
                Position = new SKPoint(window.preLaunchWidth / 2, window.preLaunchHeight / 2),
                Rotation = 0f,
            };
        }

        public override void Update(float deltaTime)
        {
            bool isSpacePressed = window.KeyDown(Key.Space);

            if (isSpacePressed && !lastSpaceState)
            {
                var bullet = new Bullet();
                window.Instantiate(bullet,(int)PhysicsLayers.Player);
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
                // jezeli poprzednia zarejestrowana rotacja jest w tym samym kierunku co aktualna rotacja, to zwiekszaj predkosc
                if (ShouldSlow())
                {
                    if (speed - maxSpeed / 100 > minSpeed)
                        speed -= maxSpeed / 100;
                    else
                    {
                        prevRotation = transform.Rotation;
                        prevRotationRadias = transform.RotationRadians;
                        speed = minSpeed;
                    }
                }
                else
                {
                    prevRotation = transform.Rotation;
                    prevRotationRadias = transform.RotationRadians;
                    if (speed + maxSpeed / 100 < maxSpeed)
                        speed += maxSpeed / 100;
                    else
                        speed = maxSpeed;
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

            if (transform.Position.X < 0)
                transform.Position = new SKPoint(window.Width, transform.Position.Y);
            else if (transform.Position.X > window.Width)
                transform.Position = new SKPoint(0, transform.Position.Y);
            else if (transform.Position.Y < 0)
                transform.Position = new SKPoint(transform.Position.X, window.Height);
            else if (transform.Position.Y > window.Height)
                transform.Position = new SKPoint(transform.Position.X,0);
        }

        bool ShouldSlow()
        {
            float tPrevRotation = prevRotation;
            float tRotation = transform.Rotation;

            if (tPrevRotation < 0)
                tPrevRotation = 360 + tPrevRotation;
            if (tRotation < 0)
                tRotation = 360 + tRotation;

            if (tRotation - 45 <= tPrevRotation && tPrevRotation <= tRotation + 45)
                return false;

            return true;
        }
    }
}
