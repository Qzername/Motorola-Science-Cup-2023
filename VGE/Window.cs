using System.Timers;
using System.Windows.Automation;
using VGE.Graphics;
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

        public Window() 
        { 
            mainWindow = new MainWindow();

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

		void FrameUpdate(object? sender, ElapsedEventArgs e)
        {
            time.NextFrame();
            canvas.Clear();

            Update(canvas);

            mainWindow.SetLines(canvas.GetLines());
            mainWindow.RefreshCanvas();
        }

        public abstract void Update(Canvas canvas);

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

        /// <summary>
        /// Sprawdza czy dany przycisk jest wciśnięty.
        /// </summary>
        protected bool KeyDown(Key key) => mainWindow.GetKey(key);
    }
}
