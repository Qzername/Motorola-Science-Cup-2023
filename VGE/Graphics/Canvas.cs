namespace VGE.Graphics
{
	public class Canvas
	{
		List<Line> lines;

		public Canvas()
		{
			lines = new List<Line>();
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
			catch (Exception)
			{

			}
		}
		public void Clear()
		{
			lines.Clear();
		}

		public List<Line> GetLines() => lines;
	}
}
