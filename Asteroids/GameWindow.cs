using SkiaSharp;
using VGL;
using VGL.Graphics;

namespace Asteroids
{
    internal class GameWindow : Window
    {
        float x = 0;
        float speed = 120, rotationSpeed = 90;

        VectorObject player;

        public GameWindow() : base()
        {
            player = new VectorObject(
                new Shape(-90f, 
                        new SKPoint(0,0), 
                        new SKPoint(15,40), 
                        new SKPoint(30,0), 
                        new SKPoint(15,5)), 

                new SKPoint(150,150), 0f);
        }

        public override void Update(Canvas canvas)
        {
            float speedDelta = speed * time.DeltaTime;
            float rotationDelta = rotationSpeed * time.DeltaTime;

            if(KeyDown(Key.Left))
                player.Rotate(rotationDelta * -1f);
            else if(KeyDown(Key.Right))
                player.Rotate(rotationDelta);

            float sin = MathF.Sin(player.Transform.RotationRadians);
            float cos = MathF.Cos(player.Transform.RotationRadians);

            player.AddPosition(cos * speedDelta, sin * speedDelta);

            player.Draw(canvas);
        }
    }
}
