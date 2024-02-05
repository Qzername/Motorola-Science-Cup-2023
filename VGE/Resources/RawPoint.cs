using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE.Resources
{
    public struct RawPoint(float x, float y)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;
    }
}
