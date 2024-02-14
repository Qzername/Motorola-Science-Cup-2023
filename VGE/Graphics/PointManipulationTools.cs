using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE.Resources;

namespace VGE.Graphics
{
    public static class PointManipulationTools
    {
        //public static Point Rotate(float angle, Point point, Point rotationCenter) => RotateMultiple(angle, [point], rotationCenter)[0];

        public static Point[] RotateMultiple(float angle, Point[] points, Point rotationCenter)
        {
            //ten algorytm jest wzięty ze strony:
            //https://danceswithcode.net/engineeringnotes/rotations_in_2d/rotations_in_2d.html

            float angleRad = angle * (MathF.PI / 180);

            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            float cx = rotationCenter.X, cy = rotationCenter.Y;

            float temp;
            for (int n = 0; n < points.Length; n++)
            {
                var current = points[n];

                temp = (current.X - cx) * cos - (current.Y - cy) * sin + cx;
                points[n].Y = (current.X - cx) * sin + (current.Y - cy) * cos + cy;
                points[n].X = temp;
            }

            return points;
        }


        //https://math.libretexts.org/Bookshelves/Applied_Mathematics/Mathematics_for_Game_Developers_(Burzynski)/04%3A_Matrices/4.06%3A_Rotation_Matrices_in_3-Dimensions
        public static Point[] Rotate(Point rotation, Point[] points)
        {
            var sinX = MathF.Sin(rotation.X * MathTools.Deg2rad);
            var cosX = MathF.Cos(rotation.X * MathTools.Deg2rad);
            
            var sinY = MathF.Sin(rotation.Y * MathTools.Deg2rad);
            var cosY = MathF.Cos(rotation.Y * MathTools.Deg2rad);
            
            var sinZ = MathF.Sin(rotation.Z * MathTools.Deg2rad);
            var cosZ = MathF.Cos(rotation.Z * MathTools.Deg2rad);

            Point[] rotatedPoints = new Point[points.Length];

            for(int i = 0; i < points.Length; i++)
            {
                var currentPoint = points[i];
                var matrixPoint = new float[,] { { currentPoint.X, currentPoint.Y, currentPoint.Z } };

                float[,] rotatedPointMatrix = matrixPoint;

                if(rotation.X != 0f)
                    rotatedPointMatrix = MathTools.MultiplyMatrix(matrixPoint, 
                                                        new float[,] {//x
                                                            {1,0,0 },//y
                                                            {0,cosX,-sinX },//y
                                                            {0,sinX,cosX },//y
                                                        });

                if(rotation.Y != 0f)
                    rotatedPointMatrix = MathTools.MultiplyMatrix(matrixPoint,
                                                        new float[,] {//x
                                                            {cosY, 0, sinY},//y
                                                            {0,1,0 },//y
                                                            {-sinY,0,cosY },//y
                                                        });

                if(rotation.Z != 0f)
                    rotatedPointMatrix = MathTools.MultiplyMatrix(matrixPoint,
                                                        new float[,] {//x
                                                            {cosZ,-sinZ,0 },//y
                                                            {sinZ,cosZ,0 },//y
                                                            {0,0,1 },//y
                                                        });

                rotatedPoints[i] = new Point(rotatedPointMatrix[0, 0], 
                                             rotatedPointMatrix[0, 1], 
                                             rotatedPointMatrix[0, 2]);
            }

            return rotatedPoints;
        }
    }
}
