namespace VGE.Resources
{
    public struct RawShapeSet(string name)
    {
        public string Name { get; set; } = name;
        public Dictionary<string, RawShape[]> Set { get; set; } = new Dictionary<string, RawShape[]>();
    }
}
