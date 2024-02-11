using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Asteroids.Objects
{
    public class Player : PhysicsObject
    {
        const float minSpeed = 0, maxSpeed = 400f;
        float speed, rotationSpeed = 90, prevZRotation, prevZRotationRadias;
    
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
				transform.Position = new Point(res.Width / 2f, res.Height / 2f);
                transform.Rotation = Point.Zero3D;
                prevZRotation = Transform.Rotation.Z;
                prevZRotationRadias = Transform.RotationRadians.Z;      
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
                Shape = new PointShape(new Point(-10, -10),
                                       new Point(0, 0),
                                       new Point(-10, 10),
                                       new Point(20, 0)),
                Position = new Point(res.Width / 2f, res.Height / 2f),
                Rotation = Point.Zero3D,
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
                bullet.Setup(transform.Position + Shape.CompiledShape[0].EndPosition, transform.Rotation.Z);

                lastSpaceState = true;
            }
            else if (!isSpacePressed && lastSpaceState)
                lastSpaceState = false;

            float sin = MathF.Sin(-prevZRotationRadias);
            float cos = MathF.Cos(-prevZRotationRadias);

            float speedDelta = speed * deltaTime;
            float rotationDelta = rotationSpeed * deltaTime;

            // Jesli shift (lewy lub prawy) jest wcisniety, przyspiesz rotacje
            if (window.KeyDown(Key.LeftShift) || window.KeyDown(Key.RightShift))
                rotationDelta *= 2;

            // Skrecanie w lewo/prawo
            if (window.KeyDown(Key.Left) || window.KeyDown(Key.A))
                Rotate(new Point(0,0,rotationDelta));
            else if (window.KeyDown(Key.Right) || window.KeyDown(Key.D))
                Rotate(new Point(0,0, rotationDelta * -1f));

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
						prevZRotation = transform.Rotation.Z;
						prevZRotationRadias = transform.RotationRadians.Z;
					}
                }
                else
                {
                    if (speed + maxSpeed / 100 < maxSpeed)
                        speed += maxSpeed / 100;
                    else
                        speed = maxSpeed;

					prevZRotation = transform.Rotation.Z;
					prevZRotationRadias = transform.RotationRadians.Z;
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
                transform.Position = new Point(res.Width, transform.Position.Y);
            else if (transform.Position.X > res.Width)
                transform.Position = new Point(0, transform.Position.Y);
            else if (transform.Position.Y < 0)
                transform.Position = new Point(transform.Position.X, res.Height);
            else if (transform.Position.Y > res.Height)
                transform.Position = new Point(transform.Position.X,0);
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
            float tPrevRotation = 360 + prevZRotation;
            float tRotation = 360 + transform.Rotation.Z;

            if (tRotation - 45 <= tPrevRotation && tPrevRotation <= tRotation + 45)
                return false;

            return true;
        }
    }
}
