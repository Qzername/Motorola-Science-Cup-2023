using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Objects;
using VGE.Windows;

namespace ShapeRenderer
{
	public class ProgramWindow : Window
	{
		ObjectToRender otr;

		Text errorText;

		List<Point> pointDefinitions;
		List<Point> lineDefinitions;

		public ProgramWindow() : base(new Scene3D(100f))
		{
			otr = new ObjectToRender();
			Instantiate(otr);

			Instantiate(new CameraController());

			errorText = new Text("ERROR", 2f, new Point(5, 5));
			Instantiate(errorText);
			errorText.IsEnabled = false;

			ReloadShape();
		}

		public override void Update(Canvas canvas)
		{
			if (KeyDown(Key.R))
				ReloadShape();
		}

		void ReloadShape()
		{
			errorText.IsEnabled = false;

			try
			{
				pointDefinitions = [];
				lineDefinitions = [];

				int mode = 0;

				var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.3Dshape");

				foreach (var line in File.ReadAllLines(files[0]))
				{
					if (string.IsNullOrEmpty(line) || line.StartsWith('#'))
						continue;

					if (line[0] == '[')
					{
						mode++;
						continue;
					}

					var coordinates = line.Split([",", ", "], StringSplitOptions.RemoveEmptyEntries);

					Point point;

					if (coordinates.Length == 2)
						point = new Point(Convert.ToSingle(coordinates[0].Replace('.', ',')), Convert.ToSingle(coordinates[1].Replace('.', ',')), 0);
					else
						point = new Point(Convert.ToSingle(coordinates[0].Replace('.', ',')), Convert.ToSingle(coordinates[1].Replace('.', ',')), Convert.ToSingle(coordinates[2].Replace('.', ',')));

					if (mode == 1)
						pointDefinitions.Add(point);
					else
						lineDefinitions.Add(point);
				}

				otr.UpdateShape(new PredefinedShape(pointDefinitions.ToArray(), lineDefinitions.ToArray(), SKColors.Green));
			}
			catch (Exception)
			{
				errorText.IsEnabled = true;
			}
		}
	}
}
