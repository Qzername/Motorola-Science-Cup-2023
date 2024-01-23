using System;
using System.Diagnostics;
using VGE;

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