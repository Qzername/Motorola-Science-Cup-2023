﻿using System;
using VGL;

namespace Sandbox
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GameWindow window = new GameWindow();

            window.Open();
        }
    }
}