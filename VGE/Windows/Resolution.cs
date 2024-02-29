namespace VGE.Windows
{
    public struct Resolution(int width, int height)
    {
        public int Width { get; } = width;
        public int Height { get; } = height;


        public static bool operator ==(Resolution res1, Resolution res2)
        {
            return res1.Width == res2.Width && res1.Height == res2.Height;
        }

        public static bool operator !=(Resolution res1, Resolution res2)
        {
            return !(res1 == res2);
        }
    }
}
