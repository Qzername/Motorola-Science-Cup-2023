using System;
using System.Diagnostics;
using VGL;

namespace Asteroids
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            GameWindow window = new GameWindow();
            window.Open();
        }
    }
}