using Emotion.Graphics;
using Emotion.Primitives;
using Emotion.Scenography;

namespace Sandbox
{
    internal class GameScene : Scene
    {
        public override void Draw(RenderComposer composer)
        {
            composer.RenderLine(new System.Numerics.Vector3(10, 100, 0), new System.Numerics.Vector3(20, 10, 50), Color.Green, 10f);

            composer.RenderLine(new System.Numerics.Vector3(10, 100, 50), new System.Numerics.Vector3(10, 10, 0), Color.Red, 10f);
        }

        public override Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public override void Update()
        {
        }
    }
}
