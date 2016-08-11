using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backgammon;
using System.Collections.Generic;

namespace BackgammonTests
{
    [TestClass]
    public class UnitTestBoard
    {
        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Exception in method Board.Board: nonpositive number.")]
        public void TestConstructorBoardZeroTriangles()
        {
            Board board = new Board(0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Exception in method Board.Board: nonpositive number.")]
        public void TestConstructorBoardNegativeTrianglesNum()
        {
            Board board = new Board(-24);
        }

        [TestMethod]
        public void TestConstructorBoardPositiveTrianglesNum()
        {
            Board board = new Board(24);
            Assert.AreEqual(board.TrianglesNumber, 24);
        }

        [TestMethod]
        public void TestSetTriangle()
        {
            Board board = new Board(24);
            board.SetTriangle(0, 1, 10);
            Triangle triangle = board[0];
            Assert.AreEqual(triangle.PlayerNumber, 1);
            Assert.AreEqual(triangle.CheckersNumber, 10);
        }

        [TestMethod]
        public void TestAddCheckerToEmptyTriangle()
        {
            Board board = new Board(24);
            Assert.AreEqual(board[1].CheckersNumber, 0);
            Assert.AreEqual(board[1].PlayerNumber, -1);
            board.AddCheckerToTriangle(1, 0);
            Assert.AreEqual(board[1].CheckersNumber, 1);
            Assert.AreEqual(board[1].PlayerNumber, 0);
        }

        [TestMethod]
        public void TestAddCheckerToOwnedTriangle()
        {
            Board board = new Board(24);
            Assert.AreEqual(board[0].CheckersNumber, 2);
            Assert.AreEqual(board[0].PlayerNumber, 0);
            board.AddCheckerToTriangle(0, board[0].PlayerNumber);
            Assert.AreEqual(board[0].CheckersNumber, 3);
        }

        [TestMethod]
        public void TestAddCheckerToBlotTriangle()
        {
            Board board = new Board(24);
            board[0].SetTriangle(0, 1);
            Assert.AreEqual(board[0].CheckersNumber, 1);
            Assert.AreEqual(board[0].PlayerNumber, 0);
            board.AddCheckerToTriangle(0, 1); // HIT
            Assert.AreEqual(board[0].CheckersNumber, 1);
            Assert.AreEqual(board[0].PlayerNumber, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Triangle.AddChecker: triangle is ruled by the rival player.")]
        public void TestAddCheckerToRuledTriangle()
        {
            Board board = new Board(24);
            Assert.AreEqual(board[0].CheckersNumber, 2);
            Assert.AreEqual(board[0].PlayerNumber, 0);
            board.AddCheckerToTriangle(0, 1); // Exception
        }

        [TestMethod]
        public void TestRemoveCheckerFromBlotTriangle()
        {
            Board board = new Board(24);
            board[0].SetTriangle(0, 1);
            Assert.AreEqual(board[0].CheckersNumber, 1);
            Assert.AreEqual(board[0].PlayerNumber, 0);
            board.RemoveCheckerFromTriangle(0);
            Assert.AreEqual(board[0].CheckersNumber, 0);
            Assert.AreEqual(board[0].PlayerNumber, -1);
        }

        [TestMethod]
        public void TestRemoveCheckerFromFullTriangle()
        {
            Board board = new Board(24);
            board[0].SetTriangle(0, 5);
            Assert.AreEqual(board[0].CheckersNumber, 5);
            Assert.AreEqual(board[0].PlayerNumber, 0);
            board.RemoveCheckerFromTriangle(0);
            Assert.AreEqual(board[0].CheckersNumber, 4);
            Assert.AreEqual(board[0].PlayerNumber, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Triangle.RemoveChecker: triangle is already empty.")]
        public void TestRemoveCheckerFromEmptyTriangle()
        {
            Board board = new Board(24);
            Assert.AreEqual(board[1].CheckersNumber, 0);
            Assert.AreEqual(board[1].PlayerNumber, -1);
            board.RemoveCheckerFromTriangle(1); // execption
        }

        [TestMethod]
        public void TestIsBaseTriangle()
        {
            Board board = new Board(24);
            for (int i = 0; i < 6; i++)
            {
                Assert.IsTrue(board.IsBaseTriangle(0, i));
            }
            for (int i = 18; i < 24; i++)
            {
                Assert.IsTrue(board.IsBaseTriangle(1, i));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Board.IsBaseTriangle: Illegal parameters.")]
        public void TestIsBaseTriangleOutOfBounds1()
        {
            Board board = new Board(24);
            Assert.IsTrue(board.IsBaseTriangle(0, -1));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Board.IsBaseTriangle: Illegal parameters.")]
        public void TestIsBaseTriangleOutOfBounds2()
        {
            Board board = new Board(24);
            Assert.IsTrue(board.IsBaseTriangle(0, 25));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
   "Execption in method Board.CheckersNumberInPlayerBase: Illegal parameters.")]
        public void TestCheckersNumberInPlayerBaseIllegalPlayerNum1()
        {
            Board board = new Board(24);
            board.CheckersNumberInPlayerBase(-2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
  "Execption in method Board.CheckersNumberInPlayerBase: Illegal parameters.")]
        public void TestCheckersNumberInPlayerBaseIllegalPlayerNum2()
        {
            Board board = new Board(24);
            board.CheckersNumberInPlayerBase(2);
        }

        [TestMethod]
        public void TestCheckersNumberInPlayer0Base()
        {
            Board board = new Board(24);
            Assert.AreEqual(2, board.CheckersNumberInPlayerBase(0));
        }

        [TestMethod]
        public void TestCheckersNumberInPlayer1Base()
        {
            Board board = new Board(24);
            Assert.AreEqual(2, board.CheckersNumberInPlayerBase(1));
        }

        [TestMethod]
        public void ClosestToBaseFilledTriangleIndex1()
        {
            Board board = new Board(24);
            Assert.AreEqual(0, board.ClosestToBaseFilledTriangleIndex(0));
        }

        [TestMethod]
        public void ClosestToBaseFilledTriangleIndex2()
        {
            Board board = new Board(24);
            board.ClearTriangles();
            List<Triangle> trianglesList = new List<Triangle>();
            trianglesList.Add(new Triangle(19, 5, 0));
            trianglesList.Add(new Triangle(20, 5, 0));
            trianglesList.Add(new Triangle(21, 5, 0));
            trianglesList.Add(new Triangle(22, 5, 0));
            trianglesList.Add(new Triangle(23, 5, 0));
            board.Setup(trianglesList);
            Assert.AreEqual(19, board.ClosestToBaseFilledTriangleIndex(0));
        }

        [TestMethod]
        public void ClosestToBaseFilledTriangleIndex3()
        {
            Board board = new Board(24);
            board.ClearTriangles();
            List<Triangle> trianglesList = new List<Triangle>();
            trianglesList.Add(new Triangle(13, 1, 1));
            trianglesList.Add(new Triangle(14, 1, 0));
            trianglesList.Add(new Triangle(16, 1, 0));
            trianglesList.Add(new Triangle(18, 5, 0));
            trianglesList.Add(new Triangle(19, 5, 0));
            trianglesList.Add(new Triangle(20, 5, 0));
            trianglesList.Add(new Triangle(21, 5, 0));
            trianglesList.Add(new Triangle(22, 5, 0));
            trianglesList.Add(new Triangle(23, 5, 0));
            board.Setup(trianglesList);
            Assert.AreEqual(14, board.ClosestToBaseFilledTriangleIndex(0));
        }

        [TestMethod]
        public void ClosestToBaseFilledTriangleIndex4()
        {
            Board board = new Board(24);
            board.ClearTriangles();
            List<Triangle> trianglesList = new List<Triangle>();
            trianglesList.Add(new Triangle(0, 5, 1));
            trianglesList.Add(new Triangle(1, 5, 1));
            trianglesList.Add(new Triangle(2, 5, 1));
            trianglesList.Add(new Triangle(3, 5, 1));
            trianglesList.Add(new Triangle(4, 5, 1));
            trianglesList.Add(new Triangle(5, 5, 1));
            board.Setup(trianglesList);
            Assert.AreEqual(5, board.ClosestToBaseFilledTriangleIndex(1));
        }

        [TestMethod]
        public void ClosestToBaseFilledTriangleIndex5()
        {
            Board board = new Board(24);
            board.ClearTriangles();
            List<Triangle> trianglesList = new List<Triangle>();
            trianglesList.Add(new Triangle(0, 5, 1));
            trianglesList.Add(new Triangle(1, 5, 1));
            trianglesList.Add(new Triangle(2, 5, 1));
            trianglesList.Add(new Triangle(3, 5, 1));
            trianglesList.Add(new Triangle(4, 5, 1));
            trianglesList.Add(new Triangle(5, 5, 1));
            trianglesList.Add(new Triangle(6, 1, 1));
            trianglesList.Add(new Triangle(8, 1, 1));
            trianglesList.Add(new Triangle(10, 1, 0));
            board.Setup(trianglesList);
            Assert.AreEqual(8, board.ClosestToBaseFilledTriangleIndex(1));
        }
    }
}
