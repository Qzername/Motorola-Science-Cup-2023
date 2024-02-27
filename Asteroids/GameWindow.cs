using Asteroids.Objects;
using SkiaSharp;
using System.Diagnostics;
using VGE.Graphics;
using VGE.Physics;
using System.Timers;
using VGE.Objects;
using VGE.Windows;
using VGE.Graphics.Scenes;
using Asteroids.Objects.UI;

namespace Asteroids
{
    internal class GameWindow : Window
    {
        SoundRegistry soundRegistry;

		public GameWindow() : base(new Scene2D())
        {
            var physicsEngine = new PhysicsEngine(new PhysicsConfiguration()
            {
                LayerConfiguration = new Dictionary<int, int[]>()
                {
                    { 0, [1] }, //0 -> player
                    { 1, [0] }, //1 -> bullets && obstacles
                }
            });
            RegisterPhysicsEngine(physicsEngine);

            soundRegistry = new SoundRegistry();
            soundRegistry.InitializeSounds(this);

            Instantiate(new EnemySpawner());

            Instantiate(new GameManager());
            Instantiate(new UIManager());
        }

        public override void Update(Canvas canvas) { }

    }
}
