using Emotion.Common;
using System;

namespace Sandbox
{
    class Program
    {
        public static void Main(string[] args)
        {
            var config = new Configurator
            {
                HostTitle = "TEST"
            };

            Engine.Setup(config);
            Engine.SceneManager.SetScene(new GameScene());
            Engine.Run();
        }
    }
}