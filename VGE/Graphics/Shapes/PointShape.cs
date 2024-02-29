using SkiaSharp;

namespace VGE.Graphics.Shapes
{
    /// <summary>
    /// Kszałt któremu podajemy kolejne koordynaty punktów
    /// </summary>
    public class PointShape : IShape
    {
        Line[] compiledShape;
        public Line[] CompiledShape => compiledShape;

        Point center;
        public Point Center => center;

        Point topLeft, bottomRight;
        public Point TopLeft => topLeft;
        public Point BottomRight => bottomRight;

        SKColor? customColor;
        public SKColor? CustomColor => customColor;

        Point[] rawShape;

        /// <param name="defaultRotation">Obrócenie kształtu na start, dzięki temu można poprawić kszałt tak aby był skierowny na wprost</param>
        public PointShape(params Point[] shape)
        {
            customColor = null;
            ConfigureShape(shape);
        }

        /// <param name="defaultRotation">Obrócenie kształtu na start, dzięki temu można poprawić kszałt tak aby był skierowny na wprost</param>
        public PointShape(SKColor customColor, params Point[] shape)
        {
            this.customColor = customColor;
            ConfigureShape(shape);
        }

        void ConfigureShape(params Point[] shape)
        {
            rawShape = shape;
            compiledShape = new Line[rawShape.Length];

            CompileShape();

            //ustawiamy środek kształtu ze względu na to że według środka będziemy mogli obracać kształt
            center = new Point((bottomRight.X - topLeft.X) / 2 + topLeft.X, (bottomRight.Y - topLeft.Y) / 2 + topLeft.Y);
        }

        /// <summary>
        /// Obrót kształtu 
        /// </summary>
        public void Rotate(Point rotation)
        {
            rawShape = PointManipulationTools.Rotate(rotation, rawShape);

            CompileShape();
        }

        /*
         * Kompilacja kształtu, nie chcemy za każdym razem gdy rysujemy shape
         * robić te same obliczenia, więc gdy zachodzi zmiana do kształtu
         * kompilujemy wygląd do liń
         */
        void CompileShape()
        {
            float minX = rawShape[0].X, maxX = rawShape[0].X,
                  minY = rawShape[0].Y, maxY = rawShape[0].Y,
                  minZ = rawShape[0].Y, maxZ = rawShape[0].Z;


            for (int i = 1; i < rawShape.Length; i++)
            {
                var current = rawShape[i];

                if (current.X < minX) minX = current.X;
                else if (current.X > maxX) maxX = current.X;

                if (current.Y < minY) minY = current.Y;
                else if (current.Y > maxY) maxY = current.Y;

                if (current.Z < minZ) minZ = current.Z;
                else if (current.Z > maxZ) maxZ = current.Z;

                compiledShape[i - 1] = new Line(rawShape[i - 1], current);
            }

            for (int i = 1; i < rawShape.Length; i++)
                compiledShape[i - 1] = new Line(rawShape[i - 1], rawShape[i]);

            if (minX == maxX) //0 == 0
                maxX = 1;

            if (minY == maxY)//0 == 0
                maxY = 1;

            topLeft = new Point(minX, minY);
            bottomRight = new Point(maxX, maxY);

            compiledShape[compiledShape.Length - 1] = new Line(rawShape[compiledShape.Length - 1], rawShape[0]);
        }
    }
}
