using SkiaSharp;
using System.Diagnostics;
using VGL;
using VGL.Graphics;
using VGL.Physics;

namespace Asteroids
{
    internal class GameWindow : Window
    {
        float speed = 120, rotationSpeed = 90, bulletSpeed = 10;

        PhysicsEngine physicsEngine;

        VectorObject player, obstacle;
        Shape bullet;

        List<VectorObject> bullets;

        bool lastSpaceState = false;

        int score = 0;

        public GameWindow() : base()
        {
            player = new VectorObject("Player",
                new Shape(-90f, 
                        new SKPoint(0,0), 
                        new SKPoint(15,40), 
                        new SKPoint(30,0), 
                        new SKPoint(15,5)), 
                new SKPoint(150,150), 0f);

            obstacle = new VectorObject("Obstacle",
                new Shape(0f,
                    new SKPoint(0, 0),
                    new SKPoint(0, 30),
                    new SKPoint(30, 30),
                    new SKPoint(30, 0)),
                new SKPoint(30,30), 0f);

            bullets = new List<VectorObject>();

            physicsEngine = new PhysicsEngine(new PhysicsConfiguration()
            {
                LayerConfiguration =  new Dictionary<int, int[]>()
                {
                    { 0, new int[] {1} }, //0 -> player
                    { 1, new int[] {0} }, //1 -> bullets && obstacles
                }
            });

            physicsEngine.RegisterObject(0, obstacle);
            physicsEngine.RegisterObject(1, player);

            physicsEngine.CollisionDetected += PhysicsEngine_CollisionDetected;
        }

        private void PhysicsEngine_CollisionDetected(VectorObject arg1, VectorObject arg2)
        {
            if (arg1.Name == "Obstacle" && arg2.Name == "Bullet")
                score++;
        }

        public override void Update(Canvas canvas)
        {
            //Ten kod jest testowy jkbc więc pozmieniaj wszystko według swoich upodobań

            //player
            float speedDelta = speed * time.DeltaTime;
            float rotationDelta = rotationSpeed * time.DeltaTime;

            if(KeyDown(Key.Left) || KeyDown(Key.A))
                player.Rotate(rotationDelta * -1f);
            else if(KeyDown(Key.Right) || KeyDown(Key.D))
                player.Rotate(rotationDelta);

            bool isSpacePressed = KeyDown(Key.Space);

            if (isSpacePressed && !lastSpaceState)
            {
                var bullet = new VectorObject("Bullet", new Shape(0f, new SKPoint(0, 0), new SKPoint(10, 0)),
                            player.Transform.Position + player.Shape.CompiledShape[0].EndPosition, //wykorzystuje tutaj shape gracza aby respic w drugim punkcie pocisk (zobacz shape gracza)
                            player.Transform.Rotation);

                bullets.Add(bullet);
                physicsEngine.RegisterObject(1, bullet);

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

            Debug.WriteLine(score);
        }
    }
}
