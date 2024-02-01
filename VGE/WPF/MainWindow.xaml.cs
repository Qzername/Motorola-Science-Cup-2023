using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System.Diagnostics;
using VGE.Graphics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace VGE.WPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        List<Line> linesToDraw;
        SKPaint paint;

        public MainWindow()
        {
            // Domyslna rozdzielczosc okna to 800x450
	        MinWidth = 800;
	        MinHeight = 450;
            
            InitializeComponent();

            paint = new SKPaint()
            {
                StrokeWidth = 1,
                Color = SKColors.White,
            };

            linesToDraw = new List<Line>();

            skElement.PaintSurface += OnPaintSurface;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.Black);

            for(int i = 0; i <linesToDraw.Count; i++)
            {
                var line = linesToDraw[i];
                canvas.DrawLine(line.StartPosition, line.EndPosition, paint);
            }
        }

        public void RefreshCanvas() => Dispatcher.Invoke(skElement.InvalidateVisual);
        public void SetLines(List<Line> lines) => linesToDraw = lines;
        public bool GetKey(Key key)
        {
            bool getKey = false;

            Dispatcher.Invoke(() =>
            {
                getKey = Keyboard.IsKeyDown((System.Windows.Input.Key)key);
            });

            return getKey;
        }
    }
}
