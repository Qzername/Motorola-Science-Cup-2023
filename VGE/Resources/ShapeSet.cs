using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE.Graphics;

namespace VGE.Resources
{
    public struct ShapeSet(string name)
    {
        public string Name { get; set; } = name;
        public Dictionary<string, RawShape[]> Set { get; set; } = new Dictionary<string, RawShape[]>();
    }
}
