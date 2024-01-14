using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGL.Graphics
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
        public void DrawLine(Line line) => lines.Add(line);
        public void Clear() => lines.Clear();
        public Line[] GetLines() => lines.ToArray(); 
    }
}
