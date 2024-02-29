using Asteroids.Objects;
using Asteroids.Objects.UI;
using VGE;
using VGE.Audio.Default;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Physics;
using VGE.Windows;

namespace Asteroids
{
    internal class GameWindow : Window
    {
        SoundRegistry soundRegistry;

        static WindowConfiguration windowConfiguration = new WindowConfiguration()
        {
            Name = "Asteroids",
            Size = new Point(1280, 720)
        };

        public GameWindow() : base(windowConfiguration, new Scene2D())
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
            RegisterAudioEngine(new NAudioEngine());

            soundRegistry = new SoundRegistry();
            soundRegistry.InitializeSounds(this);

            Instantiate(new EnemySpawner());

            Instantiate(new GameManager());
            Instantiate(new UIManager());
        }

        public override void Update(Canvas canvas) { }

    }
}
