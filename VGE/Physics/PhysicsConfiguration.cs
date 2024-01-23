using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE.Physics
{
    public struct PhysicsConfiguration
    {
        public Dictionary<int, int[]> LayerConfiguration;
        public SKPoint CollisonTopLeftOffset;
        public SKPoint CollisonBottomRightOffset;
    }
}
