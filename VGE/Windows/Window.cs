using System.Diagnostics;
using System.Timers;
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
		protected Time time;

		const int framerate = 60;

		List<VectorObject> objects;
		public List<VectorObject> Objects
		{
			get => objects;
			set => objects = value;
		}

		IScene scene;
		
		PhysicsEngine? physicsEngine;

		public Window(IScene scene)
		{
			this.scene = scene;

			mainWindow = new MainWindow();

			objects = new List<VectorObject>();

			canvas = new Canvas();
			time = new Time();

			frameTimer = new System.Timers.Timer(1000 / framerate);
			frameTimer.Elapsed += FrameUpdate;

			mainWindow.SetLines(canvas.GetLines());
		}

		//windows do swojego okna dodaje niewidzialne obiekty, tutaj jest kompensacja za nie
		int widthOffset = -16, heightOffset = -30;

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

			obj.OnDestroy();

			if (obj is PhysicsObject)
				physicsEngine?.UnregisterObject((PhysicsObject)obj);

			int index = objects.IndexOf(obj);

			GC.SuppressFinalize(objects[index]);

            objects.Remove(obj);
        }

        public void DestroyAll()
        {
	        if (objects.Count == 0)
		        return;

			VectorObject[] objectsCopy = objects.ToArray();

			foreach (VectorObject obj in objectsCopy)
				Destroy(obj);
		}
		#endregion

		/// <summary>
		/// Sprawdza czy dany przycisk jest wciśnięty.
		/// </summary>
		public bool KeyDown(Key key) => mainWindow.GetKey(key);
	}
}
