using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace VGE.Graphics.Shapes
{
    public interface IShape
    {
        abstract Line[] CompiledShape { get; }
        public abstract Point Center { get; }

        public abstract Point TopLeft { get; }
        public abstract Point BottomRight { get; }

        public abstract SKColor? CustomColor { get; }

        public abstract void Rotate(Point rotation);
    }
}
