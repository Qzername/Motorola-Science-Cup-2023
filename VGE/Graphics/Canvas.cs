namespace VGE.Graphics
{
    public class Canvas
    {
        List<Line> lines;
        List<Circle> circles;
        public Canvas() 
        { 
            lines = new List<Line>();
            circles = new List<Circle>();
        }

        /// <summary>
        /// Rysowanie linii na ekranie
        /// </summary>
        public void DrawLine(Line line)
        {
            try
            {
                lines.Add(line);
            }
            catch(Exception)
            {

            }
        }

        public void DrawCircle(Circle circle)
        {
            try
            {
                circles.Add(circle);
            }
            catch(Exception)
            {

            }
        }

        public void Clear() 
        {
            circles.Clear();
            lines.Clear(); 
        }
        public List<Line> GetLines() => lines;
        public List<Circle> GetCircles() => circles;
    }
}
