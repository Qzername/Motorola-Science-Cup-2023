using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VGE.Graphics;

namespace VGE.WPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Line> linesToDraw;

        SKPaint paint, customPaint;

        /// <summary>
        /// Skala okienek jaka jest ustawiona w windowsie
        /// </summary>
        public float Scale;

        public MainWindow(WindowConfiguration configuration)
        {
            Scale = 1f;

            InitializeComponent();

            //Ustawienia okna
            Title = configuration.Name;

            Width = configuration.Size.X;
            Height = configuration.Size.Y;


            paint = new SKPaint()
            {
                StrokeWidth = 1,
                Color = SKColors.White,
            };

            customPaint = new SKPaint()
            {
                StrokeWidth = 1,
                Color = SKColors.White
            };

            linesToDraw = new List<Line>();

            skElement.PaintSurface += OnPaintSurface;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.Black);

            //czasami linesToDraw może być zmodyfikowane w trakcie wykonywania fora
            //dlatego nic nie będzie szkodził jezeli jest tutaj try catch
            try
            {
                for (int i = 0; i < linesToDraw.Count; i++)
                {
                    var line = linesToDraw[i];

                    if (line.LineColor is null)
                        canvas.DrawLine(line.StartPosition, line.EndPosition, paint);
                    else
                    {
                        customPaint.Color = line.LineColor.Value;
                        canvas.DrawLine(line.StartPosition, line.EndPosition, customPaint);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Odświeżenie canvasa - renderowanie canvasa na nowo
        /// </summary>
        public void RefreshCanvas() => Dispatcher.Invoke(skElement.InvalidateVisual);
        /// <summary>
        /// Ustawienie oknu odwołanie do zbioru liń jakie ma narysować
        /// </summary>
        public void SetLines(List<Line> lines) => linesToDraw = lines;

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PresentationSource source = PresentationSource.FromVisual(this);
            Scale = Convert.ToSingle(source.CompositionTarget.TransformToDevice.M22);
        }

        /// <summary>
        /// Sprawdzenie czy dany przycisk jest wciśnięty
        /// </summary>
        public bool GetKey(Windows.Key key)
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
