using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backgammon;
using System.Collections.Generic;

namespace BackgammonTests
{
    [TestClass]
    public class UnitTestBackgammonGame
    {
        static BackgammonGame InitializeGame()
        {
            int diceFaces = Constants.DiceFaces;
            int triangles = Constants.TrianglesNumber;
            int checkerNumber = Constants.CheckersNumber;
            Player player1 = new HumanPlayer("Human 1", 0, CheckerColor.White, checkerNumber, 0);
            Player player2 = new HumanPlayer("Human 2", 1, CheckerColor.Green, checkerNumber, 0);
            return CreateBackgammonGame(player1, player2, triangles, diceFaces);
        }

        static BackgammonGame CreateBackgammonGame(Player player1, Player player2, int trianglesNumber, int diceFaces)
        {
            BackgammonGame game = new BackgammonGame(player1, player2, trianglesNumber, diceFaces);
            game.SetCurrentPlayer(0);
            return game;
        }

        [TestMethod]
        public void TestIsLegalRegularTransferStepIllegal1()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(5);
            game.AddDiceToList(6);
            Assert.IsFalse(game.IsLegalRegularTransferStep(0, 5));
        }

        [TestMethod]
        public void TestIsLegalRegularTransferStepLegal1()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(5);
            game.AddDiceToList(6);
            Assert.IsTrue(game.IsLegalRegularTransferStep(0, 6));
        }

        [TestMethod]
        public void TestIsLegalHitStepIllegal1()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(5);
            game.AddDiceToList(6);
            Assert.IsFalse(game.IsLegalHitStep(0, 5));
        }

        [TestMethod]
        public void TestIsLegalHitStepLegal1()
        {
            BackgammonGame game = InitializeGame();
            game.Board.SetTriangle(5, 1, 1);
            game.AddDiceToList(5);
            game.AddDiceToList(6);
            Assert.IsTrue(game.IsLegalHitStep(0, 5));
        }

        [TestMethod]
        public void TestIsDestinationTriangleIsHitYes()
        {
            BackgammonGame game = InitializeGame();
            game.Board.SetTriangle(5, 1, 1);
            game.AddDiceToList(5);
            game.AddDiceToList(6);
            Assert.IsTrue(game.IsDestinationTriangleIsHit(5));
        }

        [TestMethod]
        public void TestIsDestinationTriangleIsHitNo()
        {
            BackgammonGame game = InitializeGame();
            game.Board.SetTriangle(5, 1, 2);
            game.AddDiceToList(5);
            game.AddDiceToList(6);
            Assert.IsFalse(game.IsDestinationTriangleIsHit(5));
        }

        [TestMethod]
        public void TestIsLegalRetrievalStepIllegalNoHitCheckers()
        {
            BackgammonGame game = InitializeGame();
            game.Board.SetTriangle(5, 1, 2);
            game.AddDiceToList(2);
            game.AddDiceToList(2);
            Assert.IsFalse(game.IsLegalRetrievalStep(1));
        }

        [TestMethod]
        public void TestIsLegalRetrievalStepIllegalTriangleIsRuled()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(1);
            game.AddDiceToList(1);
            game.PlayersArray[1].SetCheckerHit();
            Assert.IsFalse(game.IsLegalRetrievalStep(0));
        }

        [TestMethod]
        public void TestIsLegalRetrievalStepIllegalAllBaseTrianglesAreRuled()
        {
            BackgammonGame game = InitializeGame();
            for (int dice = 1; dice <= 6; dice++)
            {
                game.AddDiceToList(dice);
            }

            for (int tri = 18; tri < 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 2);

            }
            game.PlayersArray[1].SetCheckerHit();
            for (int destTri = 18; destTri < 23; destTri++)
            {
                Assert.IsFalse(game.IsLegalRetrievalStep(destTri));
            }
        }

        [TestMethod]
        public void TestIsLegalRetrievalStepSuccess()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(1);
            game.AddDiceToList(1);
            game.AddDiceToList(1);
            // all besides triangle 5
            for (int tri = 18; tri < 22; tri++)
            {
                game.Board.SetTriangle(tri, 0, 2);
            }
            game.Board[23].Clear();
            game.PlayersArray[1].SetCheckerHit();
            Assert.IsTrue(game.IsLegalRetrievalStep(23));
        }

        [TestMethod]
        public void TestAreAllCheckersInRivalBaseFail1()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            Assert.IsFalse(game.AreAllCheckersInRivalBase());
        }

        [TestMethod]
        public void TestAreAllCheckersInRivalBaseFail2()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            game.Board.ClearTriangles();
            for (int tri = 17; tri < 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 1);
            }
            Assert.IsFalse(game.AreAllCheckersInRivalBase());
        }

        [TestMethod]
        public void TestAreAllCheckersInRivalBaseFail3()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(1);
            game.Board.ClearTriangles();
            for (int tri = 0; tri <= 6; tri++)
            {
                game.Board.SetTriangle(tri, 1, 1);
            }
            Assert.IsFalse(game.AreAllCheckersInRivalBase());
        }

        [TestMethod]
        public void TestAreAllCheckersInRivalBaseSuccess()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            game.Board.ClearTriangles();
            for (int tri = 18; tri < 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 1);
            }
            Assert.IsTrue(game.AreAllCheckersInRivalBase());
        }

        [TestMethod]
        public void TestIsLegalRegularRemoveByDiceFailNotAllCheckersInRivalBase()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            Assert.IsFalse(game.IsLegalRegularRemoveByDice(19, 6));
        }

        [TestMethod]
        public void TestIsLegalRegularRemoveByDiceFailSourceTriangleIsEmpty()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            game.Board.ClearTriangles();
            for (int tri = 18; tri < 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 3);
            }
            game.SetCurrentPlayer(0);
            Assert.IsFalse(game.IsLegalRegularRemoveByDice(23, 1));
        }

        [TestMethod]
        public void TestIsLegalRegularRemoveByDice5Success()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            game.Board.ClearTriangles();
            for (int tri = 18; tri < 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 3);
            }
            game.SetCurrentPlayer(0);
            Assert.IsTrue(game.IsLegalRegularRemoveByDice(19, 5));
        }

        [TestMethod]
        public void TestIsLegalSpecialRemoveByDiceFailNotAllCheckersInRivalBase()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            Assert.IsFalse(game.IsLegalSpecialRemoveByDice(19, 6));
        }

        [TestMethod]
        public void TestIsLegalSpecialRemoveByDice6FailSourceTriangleNotTheClosestToBase()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            game.Board.ClearTriangles();
            for (int tri = 19; tri <= 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 3);
            }
            Assert.IsFalse(game.IsLegalSpecialRemoveByDice(20, 6));
        }

        [TestMethod]
        public void TestIsLegalSpecialRemoveByDice5Success()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            game.Board.ClearTriangles();
            for (int tri = 20; tri <= 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 3);
            }
            Assert.IsTrue(game.AreAllCheckersInRivalBase());
            Assert.AreEqual(20, game.Board.ClosestToBaseFilledTriangleIndex(0));
            Assert.IsTrue(game.IsLegalSpecialRemoveByDice(20, 5));
        }

        [TestMethod]
        public void TestIsLegalRemovalStepFailSourceTriangleIsEmpty()
        {
            BackgammonGame game = InitializeGame();
            game.Board.ClearTriangles();
            for (int tri = 18; tri < 23; tri++)
            {
                game.Board.SetTriangle(tri, 0, 3);
            }
            game.SetCurrentPlayer(0);
            game.AddDiceToList(1);
            game.AddDiceToList(1);
            Assert.IsFalse(game.IsLegalRemovalStep(23));
        }

        [TestMethod]
        public void TestIsLegalRemovalStepSuccessRegularRemove()
        {
            BackgammonGame game = InitializeGame();
            game.Board.ClearTriangles();
            for (int tri = 19; tri < 24; tri++)
            {
                game.Board.SetTriangle(tri, 0, 3);
            }
            for (int tri = 0; tri < 5; tri++)
            {
                game.Board.SetTriangle(tri, 1, 3);
            }
            game.SetCurrentPlayer(0);
            game.AddDiceToList(1);
            game.AddDiceToList(1);
            Assert.IsTrue(game.IsLegalRemovalStep(23));
        }

        [TestMethod]
        public void TestIsLegalRemovalStepSuccessSpecialRemove()
        {
            BackgammonGame game = InitializeGame();
            game.Board.ClearTriangles();
            game.SetCurrentPlayer(0);
            for (int tri = 19; tri < 24; tri++)
            {
                game.Board.SetTriangle(tri, 0, 2);
            }
            game.AddDiceToList(6);
            game.AddDiceToList(6);
            Assert.IsTrue(game.IsLegalRemovalStep(19));
        }

        [TestMethod]
        public void TestUpdateDiceListAfterGeneralTransferStep()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(5);
            Assert.IsTrue(game.DicesList.Contains(5));
            game.UpdateDiceListAfterGeneralTransferStep(15, 20);
            Assert.IsFalse(game.DicesList.Contains(5));
        }

        [TestMethod]
        public void TestUpdateDiceListAfterRetreivalStepPlayer0RetreiveToTri3DiceIs4()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(4);
            Assert.IsTrue(game.DicesList.Contains(4));
            game.SetCurrentPlayer(0);
            game.UpdateDiceListAfterRetreivalStep(3);
            Assert.IsFalse(game.DicesList.Contains(4));
        }

        [TestMethod]
        public void TestUpdateDiceListAfterRetreivalStepPlayer1RetreiveToTri21DiceIs3()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(3);
            Assert.IsTrue(game.DicesList.Contains(3));
            game.SetCurrentPlayer(1);
            game.UpdateDiceListAfterRetreivalStep(21);
            Assert.IsFalse(game.DicesList.Contains(3));
        }

        [TestMethod]
        public void TestUpdateDiceListAfterRemovalStepPlayer0RemovesFromTri18()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(6);
            Assert.IsTrue(game.DicesList.Contains(6));
            game.SetCurrentPlayer(0);
            game.UpdateDiceListAfterRegularRemovalStep(18);
            Assert.IsFalse(game.DicesList.Contains(6));
        }

        [TestMethod]
        public void TestUpdateDiceListAfterRemovalStepPlayer1RemovesFromTri1()
        {
            BackgammonGame game = InitializeGame();
            game.AddDiceToList(2);
            Assert.IsTrue(game.DicesList.Contains(2));
            game.SetCurrentPlayer(1);
            game.UpdateDiceListAfterRegularRemovalStep(1);
            Assert.IsFalse(game.DicesList.Contains(2));
        }

        [TestMethod]
        public void TesGetPossibleDestTrianglePlayer0FromTriangle11Dices245()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(0);
            game.AddDiceToList(2);
            game.AddDiceToList(4);
            game.AddDiceToList(5);
            List<int> expectedList = new List<int>() { 15, 13, 16 };
            List<int> actualList = game.GetPossibleDestTriangles(11);
            CollectionAssert.AreEquivalent(expectedList, actualList);
        }

        [TestMethod]
        public void TestGetPossibleDestTrianglePlayer1FromTriangle7Dices246()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(1);
            game.AddDiceToList(2);
            game.AddDiceToList(4);
            game.AddDiceToList(6);
            List<int> expectedList = new List<int>() { 5, 3, 1 };
            List<int> actualList = game.GetPossibleDestTriangles(7);
            CollectionAssert.AreEquivalent(expectedList, actualList);
        }

        [TestMethod]
        public void TestGameWinnerPlayer1Wins()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(1);
            game.Board.ClearTriangles();
            game.PlayersArray[1].SetCheckersOnBoard(0);
            Assert.AreEqual(0, game.PlayersArray[1].CheckersOnBoard);
            Assert.AreEqual(15, game.PlayersArray[0].CheckersOnBoard);
            Assert.AreEqual(1, game.GameWinner());
        }

        [TestMethod]
        public void TestGameLooserPlayer0Loose()
        {
            BackgammonGame game = InitializeGame();
            game.SetCurrentPlayer(1);
            game.Board.ClearTriangles();
            game.PlayersArray[1].SetCheckersOnBoard(0);
            Assert.AreEqual(0, game.PlayersArray[1].CheckersOnBoard);
            Assert.AreEqual(15, game.PlayersArray[0].CheckersOnBoard);
            Assert.AreEqual(0, game.GameLooser());
        }

        [TestMethod]
        public void TestIsRegularWinPlayer1WinPlayer0Loose()
        {
            BackgammonGame game = InitializeGame();
            game.PlayersArray[1].SetCheckersOnBoard(0);
            game.PlayersArray[0].SetCheckersOnBoard(14);
            Assert.IsTrue(game.IsRegularWin());
        }

        [TestMethod]
        public void TestIsMarsWinPlayer1WinPlayer0Loose()
        {
            BackgammonGame game = InitializeGame();
            game.PlayersArray[1].SetCheckersOnBoard(0);
            Assert.IsTrue(game.IsMarsWin());
        }

        [TestMethod]
        public void TestIsTurkishMarsWinPlayer1WinPlayer0Loose()
        {
            BackgammonGame game = InitializeGame();
            game.PlayersArray[1].SetCheckersOnBoard(0);
            Assert.AreEqual(2, game.Board.CheckersNumberInPlayerBase(0));
            Assert.IsTrue(game.IsTurkishMarsWin());
        }

        [TestMethod]
        public void TestIsStarsMarsWinPlayer1WinPlayer0Loose()
        {
            BackgammonGame game = InitializeGame();
            game.PlayersArray[1].SetCheckersOnBoard(0);
            game.PlayersArray[0].SetCheckerHit();
            Assert.AreEqual(1, game.GameWinner());
            Assert.AreEqual(0, game.GameLooser());
            Assert.AreEqual(1, game.PlayersArray[0].CheckersHit);
            Assert.IsTrue(game.IsStarsMarsWin());
        }
    }
}
