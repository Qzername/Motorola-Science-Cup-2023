using System.Text.Json.Serialization.Metadata;
using System.Timers;
using System.Windows.Automation;
using VGE.Graphics;
using VGE.Physics;
using VGE.WPF;

namespace VGE
{
    public abstract class Window
    {
        MainWindow mainWindow;
        System.Timers.Timer frameTimer;
        
        Canvas canvas;
        protected Time time;

        const int framerate = 120;

        List<VectorObject> objects;
        PhysicsEngine? physicsEngine;

        public Window() 
        { 
            mainWindow = new MainWindow();

            objects = new List<VectorObject>();

            canvas = new Canvas();
            time = new Time();

            frameTimer = new System.Timers.Timer(1000 / framerate);
            frameTimer.Elapsed += FrameUpdate;
        }
        
        // Uzywaj preLaunchHeight i preLaunchWidth zamiast Height i Width TYLKO w konstruktorze
        public int preLaunchHeight => (int)mainWindow.Height;
        public int preLaunchWidth => (int)mainWindow.Width;

		public int Height => (int)mainWindow.ActualHeight;
        public int Width => (int)mainWindow.ActualWidth;

        #region Zarządzanie klatkami
        void FrameUpdate(object? sender, ElapsedEventArgs e)
        {
            time.NextFrame();
            canvas.Clear();

            Update(canvas);

            for(int i = 0; i < objects.Count; i++)
                objects[i].Update(time.DeltaTime);

            for (int i = 0; i < objects.Count; i++)
                objects[i].RefreshGraphics(canvas);

            mainWindow.SetLines(canvas.GetLines());
            mainWindow.RefreshCanvas();
        }

        public abstract void Update(Canvas canvas);
        #endregion

        #region Zarządzanie oknem
        public void RegisterPhysicsEngine(PhysicsEngine physicsEngine)
        {
            this.physicsEngine = physicsEngine;
        }

        public void Open()
        {
            time.StartCounting();
            frameTimer.Enabled = true;
            mainWindow.ShowDialog();
        }
        public void Close()
        {
            frameTimer.Enabled = false;
            mainWindow.Close();
        }
        #endregion

        #region Zarządzanie obiektami
        public void Instantiate(VectorObject prefab, int physicsLayer = -1)
        {
            prefab.Initialize(this);

            objects.Add(prefab);

            if (physicsEngine is not null && physicsLayer != -1)
                physicsEngine.RegisterObject(physicsLayer, prefab);
        }

        public void Destroy(VectorObject objToDestroy)
        {
            VectorObject? obj = objects.Where(x => x.Guid == objToDestroy.Guid).FirstOrDefault();

            objects.Remove(obj);
            physicsEngine?.UnRegisterObject(0, obj);
        }
        #endregion

        /// <summary>
        /// Sprawdza czy dany przycisk jest wciśnięty.
        /// </summary>
        public bool KeyDown(Key key) => mainWindow.GetKey(key);
    }
}
