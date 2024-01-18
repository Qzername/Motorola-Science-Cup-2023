using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            //z jakiegoś powodu dzięki tym dwóm linijkom nie wyskakuje dziwny błąd z listom
            //jezeli go powtórzyć lub wywołać wiedz że nie zawsze on wyskakuje
            lines.Add(new Line(-10,-10,0,0));
            Clear();
        }

        /// <summary>
        /// Rysowanie linii na ekranie
        /// </summary>
        public void DrawLine(Line line) => lines.Add(line);
        public void Clear() => lines.Clear();
        public Line[] GetLines() => lines.ToArray(); 
    }
}
