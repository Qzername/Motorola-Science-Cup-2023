using VGE.Graphics.Shapes;

namespace VGE.Resources
{
	public struct ShapeSet(string name)
	{
		public string Name { get; set; } = name;
		public Dictionary<string, IShape[]> Set { get; set; } = new Dictionary<string, IShape[]>();
	}
}
