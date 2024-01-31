namespace VGE.Resources
{
    public struct RawShape(RawSKPoint[] points)
    {
        public RawSKPoint[] Points { get; set; } = points;
    }
}
