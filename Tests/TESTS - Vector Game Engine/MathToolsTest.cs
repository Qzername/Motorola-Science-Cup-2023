using VGE;

namespace UnitTestVGE
{
    [TestClass]
    public class MathToolsTest
    {

        [TestMethod]
        public void CheckAnglesCalculation()
        {
            //rad2deg
            Assert.AreEqual(180, MathF.PI * MathTools.Rad2deg);

            //deg2rad
            Assert.AreEqual(MathF.PI, 180 * MathTools.Deg2rad, 1);
        }

        [TestMethod]
        public void CheckMultiplyMatrix()
        {
            float[,] firstMatrix = new float[,]
            {
                {1,2,3},
                {4,5,6}
            };

            float[,] secondMatrix = new float[,]
            {
                {2,3},
                {2,1},
                {1,2}
            };

            float[,] correctResult = new float[,]
            {
                {9,11 },
                {24,29}
            };

            var result = MathTools.MultiplyMatrix(firstMatrix, secondMatrix);

            Assert.IsTrue(result[0, 0] == correctResult[0, 0]);
            Assert.IsTrue(result[0, 1] == correctResult[0, 1]);
            Assert.IsTrue(result[1, 0] == correctResult[1, 0]);
            Assert.IsTrue(result[1, 1] == correctResult[1, 1]);
        }
    }
}
