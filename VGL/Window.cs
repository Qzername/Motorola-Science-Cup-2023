using VGL.WPF;

namespace VGL
{
    public abstract class Window
    {
        MainWindow mainWindow;
        Timer frameTimer;

        const int framerate = 60;

        public Window() 
        { 
            mainWindow = new MainWindow();

            frameTimer = new Timer((e) => FrameUpdate(), null,Timeout.Infinite,1000/framerate);
        }

        void FrameUpdate()
        {
            Update();
        }

        public virtual void Update()
        {

        }

        public void Open()
        {
            mainWindow.ShowDialog();
            frameTimer.Change(0, 1000 / framerate);
        }

        public void Close()
        {
            mainWindow.Close();
            frameTimer.Change(Timeout.Infinite, 1000 / framerate);
        }
    }
}
