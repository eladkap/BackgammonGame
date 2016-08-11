using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backgammon;

namespace BackgammonTests
{
    [TestClass]
    public class UnitTestTriangle
    {
        [TestMethod]
        public void TestFullCtorTriangle()
        {
            Triangle triangle = new Triangle(4, 5, 1);
            Assert.AreEqual(triangle.Index, triangle.CheckersNumber, 4);
            Assert.AreEqual(triangle.CheckersNumber, 5);
            Assert.AreEqual(triangle.PlayerNumber, 1);
        }

        [TestMethod]
        public void TestEmptyCtorTriangle()
        {
            Triangle triangle = new Triangle();
            Assert.AreEqual(triangle.Index, triangle.CheckersNumber, 0);
            Assert.AreEqual(triangle.CheckersNumber, 0);
            Assert.AreEqual(triangle.PlayerNumber, -1);
        }

        [TestMethod]
        public void TestSetTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 7);
            Assert.AreEqual(triangle.PlayerNumber, 1);
            Assert.AreEqual(triangle.CheckersNumber, 7);
        }

        [TestMethod]
        public void TestIsEmpty()
        {
            Triangle triangle = new Triangle();
            Assert.IsTrue(triangle.IsEmpty());
            triangle.SetTriangle(1, 7);
            Assert.AreEqual(triangle.IsEmpty(), false);
        }

        [TestMethod]
        public void TestIsBlot()
        {
            Triangle triangle = new Triangle();
            Assert.AreEqual(triangle.IsBlot(), false);
            triangle.SetTriangle(1, 7);
            Assert.AreEqual(triangle.IsBlot(), false);
            triangle.SetTriangle(1, 1);
            Assert.AreEqual(triangle.IsBlot(), true);
        }

        [TestMethod]
        public void TestIsOwnedByPlayer()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 4);
            Assert.AreEqual(triangle.IsOwnedByPlayer(1), true);
            Assert.AreEqual(triangle.IsOwnedByPlayer(0), false);
        }

        [TestMethod]
        public void TestIsRuledByPlayerTriangleWithTwoCheckers()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 2);
            Assert.AreEqual(triangle.IsRuledByPlayer(1), true);
            Assert.AreEqual(triangle.IsOwnedByPlayer(0), false);
        }

        [TestMethod]
        public void TestIsRuledByPlayerTriangleWithOneChecker()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 1);
            Assert.AreEqual(triangle.IsRuledByPlayer(1), false);
            Assert.AreEqual(triangle.IsOwnedByPlayer(0), false);
        }

        [TestMethod]
        public void TestIsRuledByPlayerEmptyTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 0);
            Assert.AreEqual(triangle.IsRuledByPlayer(1), false);
            Assert.AreEqual(triangle.IsOwnedByPlayer(0), false);
        }

        [TestMethod]
        public void TestAddCheckerIntoEmptyTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 0);
            Assert.IsTrue(triangle.CheckersNumber == 0);
            triangle.AddChecker(1);
            Assert.IsTrue(triangle.CheckersNumber == 1);
        }

        [TestMethod]
        public void TestAddCheckerIntoOwnedTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 1);
            Assert.IsTrue(triangle.CheckersNumber == 1);
            triangle.AddChecker(1);
            Assert.IsTrue(triangle.CheckersNumber == 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Triangle.AddChecker: triangle is ruled by the rival player.")]
        public void TestAddCheckerIntoRuledTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 2);
            Assert.IsTrue(triangle.CheckersNumber == 2);
            triangle.AddChecker(0);
        }

        [TestMethod]
        public void TestRemoveCheckerFromNotEmptyTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 2);
            Assert.IsTrue(triangle.CheckersNumber == 2);
            triangle.RemoveChecker();
            Assert.IsTrue(triangle.CheckersNumber == 1);
        }

        [TestMethod]
        public void TestRemoveLastCheckerFromNotEmptyTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 1);
            Assert.IsTrue(triangle.CheckersNumber == 1);
            triangle.RemoveChecker();
            Assert.IsTrue(triangle.CheckersNumber == 0);
            Assert.IsTrue(triangle.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Triangle.RemoveChecker: triangle is already empty.")]
        public void TestRemoveCheckerFromEmptyTriangle()
        {
            Triangle triangle = new Triangle();
            triangle.SetTriangle(1, 0);
            Assert.IsTrue(triangle.IsEmpty());
            triangle.RemoveChecker();
        }
    }
}
