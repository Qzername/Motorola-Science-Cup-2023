using VGE.Audio.Default;
using VGE.Windows;

namespace Asteroids
{
    public class SoundRegistry
    {
        public static SoundRegistry Instance;

        public Dictionary<string, NASound> Database;

        public SoundRegistry()
        {
            Instance = this;
        }

        public void InitializeSounds(Window window)
        {
            Database = new Dictionary<string, NASound>();

            foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Resources/"))
                if (file.EndsWith(".wav"))
                {
                    string fileName = file.Split("Resources/").Last();

                    var fire = new NASound(fileName);
                    window.AudioEngine.RegisterSound(fire);

                    //z np. C://Asteroids/Resources/fire.wav zostanie samo fire
                    Database[fileName.Split(['/', '\\', '.'])[^2]] = fire;
                }

            window.AudioEngine.Initialize();
        }
    }
}
