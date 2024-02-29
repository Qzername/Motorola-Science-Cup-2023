using VGE;
using VGE.Graphics;

namespace UnitTestVGE
{
    [TestClass]
    public class PointManipulationToolsTest
    {
        [TestMethod]
        public void RotateTest()
        {
            Assert.AreEqual(new Point(0, 0, 1).Z, PointManipulationTools.Rotate(new Point(0, 90, 0), [new(1, 0, 0)])[0].Z);
        }

        [TestMethod]
        public void MoveForwardTest()
        {
            Point expected = new(0, 0, 1);

            Point result = PointManipulationTools.MovePointForward(new Transform()
            {
                Position = new(0, 0, 0),
                Rotation = new(0, 0, 0)
            }, 1f);

            Assert.AreEqual(expected, result);
        }
    }
}
