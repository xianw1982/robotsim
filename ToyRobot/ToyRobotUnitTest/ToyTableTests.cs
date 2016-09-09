namespace ToyRobot.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ToyTableTests
    {

        /// <summary>
        /// Test success creation of ToyTables
        /// </summary>
        [TestMethod]
        public void ToyTableCreate_Success()
        {
            ToyTableCreate_successtest(5, 5);
            ToyTableCreate_successtest(13, 25);
            ToyTableCreate_successtest(1, 1);
            ToyTableCreate_successtest(1, 100);
            ToyTableCreate_successtest(int.MaxValue, 2555);
            ToyTableCreate_successtest(int.MaxValue, int.MaxValue);
        }

        /// <summary>
        /// test that invalid argument causes toytable to throw
        /// </summary>
        [TestMethod]
        public void ToyTableCreate_Throw()
        {
            ToyTableCreate_failtest(0, 0);
            ToyTableCreate_failtest(0, 110);
            ToyTableCreate_failtest(42, -11);
            ToyTableCreate_failtest(-111, -223);
            ToyTableCreate_failtest(int.MinValue, 42);
            ToyTableCreate_failtest(4545, int.MinValue);
            ToyTableCreate_failtest(int.MinValue, int.MinValue);
        }

        [TestMethod]
        public void IsValidPlacementTest()
        {
            ToyTable tb = ToyTableCreate_successtest(59, 33);

            int validMinRangeX = 0;
            int validMinRangeY = 0;
            int validMaxRangeX = tb.TableBoundary.X;
            int validMaxRangeY = tb.TableBoundary.Y;
            
            // iterate through the combinations and check that the coordinate check works
            for (int x = -100; x < 100; x ++ )
            {
                for(int y = -100; y < 100; y++)
                {
                    if (x < validMinRangeX || y < validMinRangeY ||
                        x > validMaxRangeX || y > validMaxRangeY)
                    {
                        Assert.IsFalse(tb.IsValidPlacement(new CoordinateXY(x, y)));
                    }else
                    {
                        Assert.IsTrue(tb.IsValidPlacement(new CoordinateXY(x, y)));
                    }
                }
            }

            Assert.IsFalse(tb.IsValidPlacement(new CoordinateXY(int.MinValue, int.MinValue)));
            Assert.IsFalse(tb.IsValidPlacement(new CoordinateXY(int.MinValue, int.MaxValue)));
            Assert.IsFalse(tb.IsValidPlacement(new CoordinateXY(int.MaxValue, int.MinValue)));
            Assert.IsFalse(tb.IsValidPlacement(new CoordinateXY(int.MaxValue, int.MaxValue)));
        }

        internal static ToyTable ToyTableCreate_successtest(int x, int y)
        {
            ToyTable tb = new ToyTable(x, y);
            Assert.IsNotNull(tb);
            Assert.IsNotNull(tb.TableBoundary);
            Assert.AreEqual(x-1, tb.TableBoundary.X);
            Assert.AreEqual(y-1, tb.TableBoundary.Y);
            return tb;
        }

        private static void ToyTableCreate_failtest(int x, int y)
        {
            try
            {
                ToyTable tb = new ToyTable(x, y);
                Assert.Fail($"invalid input parameters {x},{y}");
            }
            catch (ArgumentException) { }
        }
    }
}
