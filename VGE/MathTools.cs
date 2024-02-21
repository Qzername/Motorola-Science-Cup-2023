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
			int rB = B.GetLength(0);
			int cB = B.GetLength(1);

			float temp = 0;
			float[,] kHasil = new float[rA, cB];

			for (int i = 0; i < rA; i++)
			{
				for (int j = 0; j < cB; j++)
				{
					temp = 0;
					for (int k = 0; k < cA; k++)
					{
						temp += A[i, k] * B[k, j];
					}
					kHasil[i, j] = temp;
				}
			}

			return kHasil;
		}
	}
}
