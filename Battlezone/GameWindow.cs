using Battlezone.Objects;
using Battlezone.Objects.Enemies;
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
                    { 0, [1,2] }, //0 -> player
                    { 1, [0,2] }, //1 -> enemies 
                    { 2, [1,2] }, //2 -> obstacles
                }
            };
            
            RegisterPhysicsEngine(new PhysicsEngine(configuration));

            Instantiate(new Background());
            Instantiate(new Player());
            Instantiate(new ObstacleGenerator());
            Instantiate(new EnemySpawner());
        }

        public override void Update(Canvas canvas)
        {
        }
    }
}