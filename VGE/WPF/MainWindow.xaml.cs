using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VGE.Graphics;

namespace VGE.WPF
{
	/// <summary>
	/// Logika interakcji dla klasy MainWindow.xaml
	/// </summary>
	public partial class MainWindow : System.Windows.Window
	{
		string exePath = string.Empty;

		List<Line> linesToDraw;
		List<Circle> circlesToDraw;

		SKPaint paint, customPaint;
		SKPaint circlePaint, customCirclePaint;

		/// <summary>
		/// Skala okienek jaka jest ustawiona w windowsie
		/// </summary>
		public float Scale;

		public MainWindow()
		{
			Scale = 1f;

			//branie obecnego folderu z exe
			exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
			var files = exePath.Split('\\');
			exePath = exePath.Replace(files[^1], string.Empty);

			// Domyslna rozdzielczosc okna to 800x450
			MinWidth = 800;
			MinHeight = 450;

			InitializeComponent();

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

			circlePaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 1,
				Color = SKColors.White,
			};

			customCirclePaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 1,
				Color = SKColors.White
			};

			linesToDraw = new List<Line>();
			circlesToDraw = new List<Circle>();

			skElement.PaintSurface += OnPaintSurface;
		}

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var canvas = e.Surface.Canvas;

			canvas.Clear(SKColors.Black);

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

				for (int i = 0; i < circlesToDraw.Count; i++)
				{
					var circle = circlesToDraw[i];

					if (circle.CircleColor is null)
						canvas.DrawCircle(circle.Position, circle.Radius, circlePaint);
					else
					{
						customCirclePaint.Color = circle.CircleColor.Value;
						canvas.DrawCircle(circle.Position, circle.Radius, customCirclePaint);
					}
				}
			}
			catch (Exception)
			{

			}
		}

		public void PlaySound(string path)
		{
            var mediaplayer = new MediaPlayer();
            mediaplayer.Open(new Uri(exePath + path));
            mediaplayer.Play();
        }

		public void RefreshCanvas() => Dispatcher.Invoke(skElement.InvalidateVisual);
		public void SetLines(List<Line> lines) => linesToDraw = lines;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PresentationSource source = PresentationSource.FromVisual(this);
            Scale = Convert.ToSingle(source.CompositionTarget.TransformToDevice.M22);
        }

        public void SetCircles(List<Circle> circles) => circlesToDraw = circles;
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
