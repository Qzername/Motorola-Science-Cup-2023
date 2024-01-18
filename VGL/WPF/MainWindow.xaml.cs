using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System.Diagnostics;
using VGL.Graphics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace VGL.WPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        Line[] linesToDraw;
        SKPaint paint;

        public MainWindow()
        {
            InitializeComponent();

            paint = new SKPaint()
            {
                StrokeWidth = 1,
                Color = SKColors.White,
            };

            linesToDraw = new Line[0];

            skElement.PaintSurface += OnPaintSurface;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.Black);

            foreach (var line in linesToDraw)
                canvas.DrawLine(line.StartPosition, line.EndPosition, paint);
        }

        public void RefreshCanvas() => Dispatcher.Invoke(skElement.InvalidateVisual);
        public void SetLines(Line[] lines) => linesToDraw = lines;
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
