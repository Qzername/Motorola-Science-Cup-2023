using VGE.Graphics;

namespace VGE.Resources
{
    public struct ShapeSet(string name)
    {
        public string Name { get; set; } = name;
        public Dictionary<string, Shape[]> Set { get; set; } = new Dictionary<string, Shape[]>();
    }
}
