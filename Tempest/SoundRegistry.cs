using System.Diagnostics;
using VGE;
using VGE.Audio;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest
{
    public class SoundRegistry
    {
        public static SoundRegistry Instance;

        public Dictionary<string, Sound> Database;

        public SoundRegistry()
        {
            Instance = this;
        }

        public void InitializeSounds(Window window)
        {
            Database = new Dictionary<string, Sound>();

            foreach(var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Resources/"))
                if (file.EndsWith(".wav"))
                {
                    string fileName = file.Split("Resources/").Last();

                    var fire = new Sound(fileName);
                    window.AudioEngine.RegisterSound(fire);

                    //z np. C://Asteroids/Resources/fire.wav zostanie samo fire
                    Database[fileName.Split(['/', '\\', '.'])[^2]] = fire;
                }

            window.AudioEngine.InitializeMixer();
        }
    }
}
