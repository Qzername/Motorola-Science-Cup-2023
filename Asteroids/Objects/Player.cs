using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Physics;
using VGE.Windows;

namespace Asteroids.Objects
{
    public class Player : PhysicsObject
    {
        const float minSpeed = 0, maxSpeed = 400f;
        float speed, rotationSpeed = 90, prevRotation, prevRotationRadias;
    
        bool lastSpaceState, respawnShield;

        public override int PhysicsLayer => (int)PhysicsLayers.Player;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            /*
				Jesli gracz jest w kolizji z Obstacle, UFO lub pociskiem UFO, traci jedno zycie
                Jesli gracz nie ma juz zyc, okno sie zamyka
            */

	        if (other.Name == "Obstacle" || other.Name == "UFO" || other.Name == "BulletUFO")
	        {
                // Jesli 5-sekundowa tarcza jest aktywna, zniszcz obiekt zamiast gracza
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
                
                // Zresetuj pozycje (itp. itd.) gracza
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

            Debug.WriteLine(res.Width);
            Debug.WriteLine(res.Height);

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

			/*
				Wystrzel pocisk jesli spacja jest wcisnieta (przytrzymanie spacji wystrzeli pocisk tylko raz) oraz jesli na ekranie jest mniej niz 4 pociskow
				https://www.classicgaming.cc/classics/asteroids/play-guide
            */
			if (isSpacePressed && !lastSpaceState && GameManager.BulletsOnScreen < 4)
            {
                GameManager.BulletsOnScreen++;
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

            // Jesli shift (lewy lub prawy) jest wcisniety, przyspiesz rotacje
            if (window.KeyDown(Key.LeftShift) || window.KeyDown(Key.RightShift))
                rotationDelta *= 2;

            // Skrecanie w lewo/prawo
            if (window.KeyDown(Key.Left) || window.KeyDown(Key.A))
                Rotate(rotationDelta * -1f);
            else if (window.KeyDown(Key.Right) || window.KeyDown(Key.D))
                Rotate(rotationDelta);

            // Poruszanie do przodu/Hamowanie (jesli gracz nie porusza sie do przodu ORAZ nie hamuje, zwolnij - 2x wolniej niz manualne hamowanie)
            if (window.KeyDown(Key.Up) || window.KeyDown(Key.W))
            {
                if (ShouldSlow())
                {
                    if (speed - maxSpeed / 100 > 35)
                        speed -= maxSpeed / 100;
                    else
                    {
                        speed = 35;
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
			// Jesli gracz wyleci poza ekran, przenies go na przeciwna krawedz
            
            GameManager.Player = this;
            // Zaktualizuj pozycje gracza, ktora jest globalnie dostepna
		}

        /*
			Jesli poprzednia zarejestrowana rotacja jest "daleko" od obecnej rotacji, zwroc true
			Jesli nie, zwroc false
			ShoudSlow dziala gdy np. Lecimy w lewą stronę, obracamy sie w prawą i chcemy przyspieszyc
			Wtedy statek zwalnia, bo nie jest skierowany w ta sama strone, w ktora przed chwila lecial 
        */
		bool ShouldSlow()
        {
            // "fix" dla negatywnych wartosci
            float tPrevRotation = 360 + prevRotation;
            float tRotation = 360 + transform.Rotation;

            if (tRotation - 45 <= tPrevRotation && tPrevRotation <= tRotation + 45)
                return false;

            return true;
        }
    }
}
