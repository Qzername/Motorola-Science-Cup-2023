using SkiaSharp;

namespace VGE.Physics
{
    public static class PhysicsTools
    {
        public static bool CheckCollisionAABB(Point topLeft1, Point bottomRight1, Point topLeft2, Point bottomRight2)
        {
            return 
                topLeft1.X <= bottomRight2.X && 
                bottomRight1.X >= topLeft2.X &&
                topLeft1.Y <= bottomRight2.Y && 
                bottomRight1.Y >= topLeft2.Y &&
                topLeft1.Z <= bottomRight2.Z &&
                bottomRight1.Z >= topLeft2.Z;
        }
    }
}
