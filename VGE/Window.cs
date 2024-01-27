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
        
        public Resolution GetResolution()
        {
			int width = (int)mainWindow.Dispatcher.Invoke(() => mainWindow.Width);
			int height = (int)mainWindow.Dispatcher.Invoke(() => mainWindow.Height);
            
			return new Resolution(width, height);
        }

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
            mainWindow.Dispatcher.Invoke(() => mainWindow.Close());
        }
        #endregion

        #region Zarządzanie obiektami
        public void Instantiate(VectorObject prefab)
        {
            prefab.Initialize(this);

            objects.Add(prefab);

            if (prefab is PhysicsObject)
                physicsEngine?.RegisterObject((PhysicsObject)prefab);
        }

        public void Destroy(VectorObject objToDestroy)
        {
            VectorObject? obj = objects.SingleOrDefault(x => x.Guid == objToDestroy.Guid);

            if (obj == null)
	            return;

            if (obj is PhysicsObject)
                physicsEngine?.UnregisterObject((PhysicsObject)obj);

            objects.Remove(obj);
        }
        #endregion

        /// <summary>
        /// Sprawdza czy dany przycisk jest wciśnięty.
        /// </summary>
        public bool KeyDown(Key key) => mainWindow.GetKey(key);
    }
}
