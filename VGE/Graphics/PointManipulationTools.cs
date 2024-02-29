namespace VGE.Graphics
{
    public static class PointManipulationTools
    {
        //https://math.libretexts.org/Bookshelves/Applied_Mathematics/Mathematics_for_Game_Developers_(Burzynski)/04%3A_Matrices/4.06%3A_Rotation_Matrices_in_3-Dimensions
        /// <summary>
        /// Obrót punktów względem rotacji
        /// </summary>
        public static Point[] Rotate(Point rotation, Point[] points)
        {
            var sinX = MathF.Sin(rotation.X * MathTools.Deg2rad);
            var cosX = MathF.Cos(rotation.X * MathTools.Deg2rad);

            var sinY = MathF.Sin(rotation.Y * MathTools.Deg2rad);
            var cosY = MathF.Cos(rotation.Y * MathTools.Deg2rad);

            var sinZ = MathF.Sin(rotation.Z * MathTools.Deg2rad);
            var cosZ = MathF.Cos(rotation.Z * MathTools.Deg2rad);

            Point[] rotatedPoints = new Point[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                var currentPoint = points[i];
                var matrixPoint = new float[,] { { currentPoint.X, currentPoint.Y, currentPoint.Z } };

                float[,] rotatedPointMatrix = matrixPoint;

                if (rotation.X != 0f)
                    rotatedPointMatrix = MathTools.MultiplyMatrix(rotatedPointMatrix,
                                                        new float[,] {//x
                                                            {1,0,0 },//y
                                                            {0,cosX,-sinX },//y
                                                            {0,sinX,cosX },//y
                                                        });

                if (rotation.Y != 0f)
                    rotatedPointMatrix = MathTools.MultiplyMatrix(rotatedPointMatrix,
                                                        new float[,] {//x
                                                            {cosY, 0, sinY},//y
                                                            {0,1,0 },//y
                                                            {-sinY,0,cosY },//y
                                                        });

                if (rotation.Z != 0f)
                    rotatedPointMatrix = MathTools.MultiplyMatrix(rotatedPointMatrix,
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

        //https://stackoverflow.com/questions/45664697/calculating-forward-vector-given-rotation-in-3d
        /// <summary>
        /// Przeniesienie punktu o dystans do przodu gdzie jest skierowany
        /// </summary>
        public static Point MovePointForward(Transform originTransform, float distance)
        {
            float cosZ = MathF.Cos(originTransform.RotationRadians.Z);
            float cosY = MathF.Cos(originTransform.RotationRadians.Y);
            float sinY = MathF.Sin(originTransform.RotationRadians.Y);
            float sinZ = MathF.Sin(originTransform.RotationRadians.Z);

            var forwardVector = new Point(cosZ * cosY, sinZ, cosZ * sinY);

            return originTransform.Position + new Point(distance * forwardVector.Z, distance * forwardVector.Y, distance * forwardVector.X);
        }
    }
}
