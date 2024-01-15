﻿using System.Timers;
using VGL.Graphics;
using VGL.WPF;

namespace VGL
{
    public abstract class Window
    {
        MainWindow mainWindow;
        System.Timers.Timer frameTimer;
        
        Canvas canvas;
        protected Time time;

        const int framerate = 60;

        public Window() 
        { 
            mainWindow = new MainWindow();

            canvas = new Canvas();
            time = new Time();

            frameTimer = new System.Timers.Timer(1000 / framerate);
            frameTimer.Elapsed += FrameUpdate;
        }

        private void FrameUpdate(object? sender, ElapsedEventArgs e)
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
    }
}