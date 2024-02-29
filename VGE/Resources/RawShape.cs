namespace VGE.Resources
{
    public struct RawShape(RawPoint[] points)
    {
        public RawPoint[] Points { get; set; } = points;
    }
}
