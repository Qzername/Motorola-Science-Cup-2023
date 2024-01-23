using SkiaSharp;

namespace VGE.Physics
{
    public static class PhysicsTools
    {
        public static bool CheckCollisionAABB(SKPoint topLeft1, SKPoint bottomRight1, SKPoint topLeft2, SKPoint bottomRight2)
        {
            return 
                topLeft1.X <= bottomRight2.X && 
                bottomRight1.X >= topLeft2.X &&
                topLeft1.Y <= bottomRight2.Y && 
                bottomRight1.Y >= topLeft2.Y;
        }
    }
}
