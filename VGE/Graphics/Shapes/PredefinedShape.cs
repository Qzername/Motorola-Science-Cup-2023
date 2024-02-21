using SkiaSharp;

namespace VGE.Graphics.Shapes
{
	public class PredefinedShape : IShape
	{
		public Line[] compiledShape;
		public Line[] CompiledShape => compiledShape;

		Point center;
		public Point Center => center;

		public Point topLeft;
		public Point TopLeft => topLeft;

		public Point bottomRight;
		public Point BottomRight => bottomRight;

		public SKColor? customColor;
		public SKColor? CustomColor => customColor;

		Point[] points;
		Point[] connections;

		/// <summary>
		/// Kształt który z góry ma ustalone wszystkie linie
		/// </summary>
		/// <param name="points">Koordynaty punktów</param>
		/// <param name="connections">Oznaczenie który punkt łaczy się z którym, np. new Point(0,1) będzie oznaczało że punkt 0 łaczy się z punktem 1</param>
		public PredefinedShape(Point[] points, Point[] connections)
		{
			this.points = points;
			this.connections = connections;

			CompileShape();
		}

		/// <summary>
		/// Kształt który z góry ma ustalone wszystkie linie
		/// </summary>
		/// <param name="points">Koordynaty punktów</param>
		/// <param name="connections">Oznaczenie który punkt łaczy się z którym, np. new Point(0,1) będzie oznaczało że punkt 0 łaczy się z punktem 1</param>
		public PredefinedShape(Point[] points, Point[] connections, SKColor customColor)
		{
			this.points = points;
			this.connections = connections;
			this.customColor = customColor;

			CompileShape();
		}

		public void Rotate(Point rotation)
		{
			points = PointManipulationTools.Rotate(rotation, points);

			CompileShape();
		}

		/*
         * Kompilacja kształtu, nie chcemy za każdym razem gdy rysujemy shape
         * robić te same obliczenia, więc gdy zachodzi zmiana do kształtu
         * kompilujemy wygląd do liń
         */
		void CompileShape()
		{
			//wyliczenia elementów potrzebnych do poźniejszych kalkulacji
			float minX = points[0].X, maxX = points[0].X,
				  minY = points[0].Y, maxY = points[0].Y,
				  minZ = points[0].Y, maxZ = points[0].Z;

			foreach (var current in points)
			{
				if (current.X < minX) minX = current.X;
				else if (current.X > maxX) maxX = current.X;

				if (current.Y < minY) minY = current.Y;
				else if (current.Y > maxY) maxY = current.Y;

				if (current.Z < minZ) minZ = current.Z;
				else if (current.Z > maxZ) maxZ = current.Z;
			}

			topLeft = new Point(minX, minY, minZ);
			bottomRight = new Point(maxX, maxY, maxZ);

			center = new Point((bottomRight.X - topLeft.X) / 2 + topLeft.X, (bottomRight.Y - topLeft.Y) / 2 + topLeft.Y, (bottomRight.Z - topLeft.Z) / 2 + topLeft.Z);

			//kompilowanie kształtu
			compiledShape = new Line[connections.Length];

			for (int i = 0; i < connections.Length; i++)
			{
				var currentConnection = connections[i];

				//tak to może powodować błąd ale to najszybszy sposób aby to zrobić
				//dlatego wybrałem Point do zapisywania połączeń między punktami
				compiledShape[i] = new Line(points[Convert.ToInt32(currentConnection.X)],
											points[Convert.ToInt32(currentConnection.Y)], customColor);
			}
		}
	}
}
