using Battlezone.Objects;
using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Physics;
using VGE.Windows;

namespace Battlezone
{
    internal class GameWindow : Window
    {
        public GameWindow() : base(new Scene3D())
        {
            PhysicsConfiguration configuration = new PhysicsConfiguration()
            {
                LayerConfiguration = new Dictionary<int, int[]>()
                {
                    { 0, new int[] {1} }, //0 -> player
                    { 1, new int[] {0} }, //1 -> bullets && obstacles
                }
            };
            
            RegisterPhysicsEngine(new PhysicsEngine(configuration));

            Instantiate(new Background());
            
            Instantiate(new Player());

            Instantiate(new Cube(new Point(0,0,100)));
            Instantiate(new Cube(new Point(0, 0, -100)));
            Instantiate(new Cube(new Point(100, 0, 0)));
            Instantiate(new Cube(new Point(-100, 0, 0)));
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}