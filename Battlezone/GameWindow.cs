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
        public GameWindow() : base(new Scene3D(Settings.RenderDistance))
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
            Instantiate(new ObstacleGenerator());
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}