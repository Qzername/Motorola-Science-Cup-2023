using SkiaSharp.Views.Desktop;
using SkiaSharp;

namespace VGL.WPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            // the the canvas and properties
            var canvas = e.Surface.Canvas;

            // make sure the canvas is blank
            canvas.Clear(SKColors.Black);

            // draw some text
            var paint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                TextSize = 24,
                StrokeWidth = 10f,
            };
            var coord = new SKPoint(e.Info.Width / 2, (e.Info.Height + paint.TextSize) / 2);
            canvas.DrawText("SkiaSharp", coord, paint);
            canvas.DrawLine(new SKPoint(20, 20), new SKPoint(100, 100), paint);
        }
    }
}
