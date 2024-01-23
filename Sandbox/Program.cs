using System;
using System.Diagnostics;
using VGE;

namespace Sandbox
{
    /*
     * Ten projekt służy jedynie do testów biblioteki
     * Nie chcemy zaśmiecać naszych gier testowym kodem
     */

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