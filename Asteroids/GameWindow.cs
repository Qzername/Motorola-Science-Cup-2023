using SkiaSharp;
using System.Diagnostics;
using VGL;
using VGL.Graphics;

namespace Asteroids
{
    internal class GameWindow : Window
    {
        float x = 0;
        float speed = 120, rotationSpeed = 90, bulletSpeed = 10;

        VectorObject player, obstacle;
        Shape bullet;

        List<VectorObject> bullets;

        bool lastSpaceState = false;

        public GameWindow() : base()
        {
            player = new VectorObject(
                new Shape(-90f, 
                        new SKPoint(0,0), 
                        new SKPoint(15,40), 
                        new SKPoint(30,0), 
                        new SKPoint(15,5)), 
                new SKPoint(150,150), 0f);

            obstacle = new VectorObject(
                new Shape(0f,
                    new SKPoint(0, 0),
                    new SKPoint(0, 30),
                    new SKPoint(30, 30),
                    new SKPoint(30, 0)),
                new SKPoint(30,30), 0f);

            bullets = new List<VectorObject>();
        }

        public override void Update(Canvas canvas)
        {
            //Ten kod jest testowy jkbc więc pozmieniaj wszystko według swoich upodobań

            //player
            float speedDelta = speed * time.DeltaTime;
            float rotationDelta = rotationSpeed * time.DeltaTime;

            if(KeyDown(Key.Left))
                player.Rotate(rotationDelta * -1f);
            else if(KeyDown(Key.Right))
                player.Rotate(rotationDelta);

            bool isSpacePressed = KeyDown(Key.Space);

            if (isSpacePressed && !lastSpaceState)
            {
                bullets.Add(new VectorObject(new Shape(0f, new SKPoint(0, 0), new SKPoint(10, 0)),
                            player.Transform.Position+player.Shape.CompiledShape[0].EndPosition, //wykorzystuje tutaj shape gracza aby respic w drugim punkcie pocisk (zobacz shape gracza)
                            player.Transform.Rotation));

                lastSpaceState = true;
            }
            else if (!isSpacePressed && lastSpaceState)
                lastSpaceState = false;

            float sin = MathF.Sin(player.Transform.RotationRadians);
            float cos = MathF.Cos(player.Transform.RotationRadians);

            player.AddPosition(cos * speedDelta, sin * speedDelta);

            player.Draw(canvas);

            //obstacle
            obstacle.Draw(canvas);
        
            //bullets
            for(int i = 0; i < bullets.Count; i++)  
            {
                sin = MathF.Sin(bullets[i].Transform.RotationRadians);
                cos = MathF.Cos(bullets[i].Transform.RotationRadians);

                bullets[i].AddPosition(cos*bulletSpeed, sin*bulletSpeed);

                bullets[i].Draw(canvas);
            }
        }
    }
}
