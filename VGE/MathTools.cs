namespace VGE
{
    public static class MathTools
    {
        public static float Rad2deg = 180 / MathF.PI;
        public static float Deg2rad = MathF.PI / 180;

        //https://stackoverflow.com/questions/6311309/how-can-i-multiply-two-matrices-in-c
        public static float[,] MultiplyMatrix(float[,] A, float[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int cB = B.GetLength(1);
            float[,] kHasil = new float[rA, cB];

            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    float temp = 0;
                    for (int k = 0; k < cA; k++)
                    {
                        temp += A[i, k] * B[k, j];
                    }
                    kHasil[i, j] = temp;
                }
            }

            return kHasil;
        }

        /// <summary>
        /// Kalkulacja dystansu od siebie dwóch punktów
        /// </summary>
        //Pitagoras
        public static float CalculateDistance(Point p1, Point p2) => MathF.Sqrt(MathF.Pow(p1.X - p2.X, 2) + MathF.Pow(p1.Y - p2.Y, 2) + MathF.Pow(p1.Z - p2.Z, 2));
    }
}
