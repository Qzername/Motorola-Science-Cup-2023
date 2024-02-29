using System.Timers;
using VGE.Audio;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Physics;
using VGE.WPF;

namespace VGE.Windows
{
    public abstract class Window
    {
        MainWindow mainWindow;
        System.Timers.Timer frameTimer;

        Canvas canvas;
        Time time;

        int framerate = 60;

        List<VectorObject> objects;
        IScene scene;

        PhysicsEngine? physicsEngine;
        public AudioEngine AudioEngine;

        /// <param name="windowConfiguration">Ustawienia okna</param>
        /// <param name="scene">Rodzaj sceny</param>
        public Window(WindowConfiguration windowConfiguration, IScene scene)
        {
            this.scene = scene;

            mainWindow = new MainWindow(windowConfiguration);

            objects = new List<VectorObject>();

            canvas = new Canvas();
            time = new Time();

            frameTimer = new System.Timers.Timer(1000 / framerate);
            frameTimer.Elapsed += FrameUpdate;

            mainWindow.SetLines(canvas.GetLines());
        }

        //windows do swojego okna dodaje niewidzialne obiekty, tutaj jest kompensacja za nie
        int widthOffset = -16, heightOffset = -30;

        /// <summary>
        /// Obecna rozdzielczość okna
        /// </summary>
        public Resolution GetResolution()
        {
            int actualWidth = mainWindow.Dispatcher.Invoke(() => (int)mainWindow.ActualWidth);
            int actualHeight = mainWindow.Dispatcher.Invoke(() => (int)mainWindow.ActualHeight);

            int width = (int)(actualWidth == 0 ? mainWindow.Width : actualWidth);
            int height = (int)(actualHeight == 0 ? mainWindow.Height : actualHeight);

            return new Resolution(Convert.ToInt32((width + widthOffset) * mainWindow.Scale),
                                  Convert.ToInt32((height + heightOffset) * mainWindow.Scale));
        }

        #region Zarządzanie klatkami
        void FrameUpdate(object? sender, ElapsedEventArgs e)
        {
            time.NextFrame();
            canvas.Clear();

            Update(canvas);

            for (int i = 0; i < objects.Count; i++)
                objects[i].Update(time.DeltaTime);

            scene.UpdateResolution(GetResolution());

            for (int i = 0; i < objects.Count; i++)
                if (!objects[i].OverrideRender(canvas))
                    scene.DrawObject(canvas, objects[i]);

            mainWindow.RefreshCanvas();
        }

        public abstract void Update(Canvas canvas);
        #endregion

        #region Zarządzanie oknem
        /// <summary>
        /// Rejestracja silnika fizycznego - po zarejestrowaniu będzie sprawdzana fizyka 50 klatek na sekunde
        /// </summary>
        /// <param name="physicsEngine"></param>
        public void RegisterPhysicsEngine(PhysicsEngine physicsEngine)
        {
            this.physicsEngine = physicsEngine;
        }

        public void RegisterAudioEngine(AudioEngine audioEngine)
        {
            AudioEngine = audioEngine;
        }

        /// <summary>
        /// Otwarcie okna
        /// </summary>
        public void Open()
        {
            time.StartCounting();
            frameTimer.Enabled = true;
            mainWindow.ShowDialog();
        }

        /// <summary>
        /// Zamknięcie okna
        /// </summary>
        public void Close()
        {
            frameTimer.Enabled = false;
            mainWindow.Dispatcher.Invoke(() => mainWindow.Close());
        }
        #endregion

        #region Zarządzanie obiektami
        /// <summary>
        /// Inicjalizacja obiektu, po inicjalizacji obiekt będzie prawidłowo działał
        /// </summary>
        public void Instantiate(VectorObject prefab)
        {
            prefab.Initialize(this);

            objects.Add(prefab);

            if (prefab is PhysicsObject)
                physicsEngine?.RegisterObject((PhysicsObject)prefab);
        }

        /// <summary>
        /// Usuwanie obiektu, nie będzie już się renderował, ani nie będzie się aktulizował
        /// </summary>
        public void Destroy(VectorObject objToDestroy)
        {
            try
            {
                if (objToDestroy == null)
                    return;

                VectorObject? obj = objects.SingleOrDefault(x => x.Guid == objToDestroy.Guid);

                if (obj == null)
                    return;

                obj.OnDestroy();

                if (obj is PhysicsObject)
                    physicsEngine?.UnregisterObject((PhysicsObject)obj);

                int index = objects.IndexOf(obj);

                GC.SuppressFinalize(objects[index]);

                objects.Remove(obj);
            }
            catch (Exception)
            {

            }
        }
        #endregion

        /// <summary>
        /// Sprawdza czy dany przycisk jest wciśnięty
        /// </summary>
        public bool KeyDown(Key key) => mainWindow.GetKey(key);
    }
}
