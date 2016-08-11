using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backgammon;

namespace BackgammonTests
{
    [TestClass]
    public class UnitTestHumanPlayer
    {
        [TestMethod]
        public void TestFullCtorPlayer()
        {
            Player player = new HumanPlayer("name", 0, CheckerColor.Red, 15, 0);
            Assert.AreEqual(player.PlayerName, "name");
            Assert.AreEqual(player.PlayerNumber, 0);
            Assert.AreEqual(player.CheckerColor, CheckerColor.Red);
            Assert.AreEqual(player.CheckersOnBoard, 15);
            Assert.AreEqual(player.CheckersHit, 0);
        }

        [TestMethod]
        public void TestEmptyCtorPlayer()
        {
            Player player = new HumanPlayer();
            Assert.AreEqual(player.PlayerName, "Player");
            Assert.AreEqual(player.PlayerNumber, 0);
            Assert.AreEqual(player.CheckerColor, CheckerColor.White);
            Assert.AreEqual(player.CheckersOnBoard, 15);
            Assert.AreEqual(player.CheckersHit, 0);
        }

        [TestMethod]
        public void TestSetProperties()
        {
            Player player = new HumanPlayer();
            player.SetProperties("abc", 1, CheckerColor.Blue, 15, 0);
            Assert.AreEqual(player.PlayerName, "abc");
            Assert.AreEqual(player.PlayerNumber, 1);
            Assert.AreEqual(player.CheckerColor, CheckerColor.Blue);
            Assert.AreEqual(player.CheckersOnBoard, 15);
            Assert.AreEqual(player.CheckersHit, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Player.SetCheckerHit(): CheckersOnBoard is already zero.")]
        public void TestSetCheckerHitWhenNoCheckersOnBoard()
        {
            Player player = new HumanPlayer("name", 0, CheckerColor.Red, 0, 0);
            player.SetCheckerHit();
        }

        [TestMethod]
        public void TestSetCheckerHitWhenThereAreCheckersOnBoard()
        {
            Player player = new HumanPlayer("name", 0, CheckerColor.Red, 15, 0);
            player.SetCheckerHit();
            Assert.IsTrue(player.CheckersHit == 1);
            Assert.IsTrue(player.CheckersOnBoard == 14);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
    "Execption in method Player.SetCheckerBack(): CheckersHit is already zero.")]
        public void TestSetCheckerBackWhenNoCheckerHit()
        {
            Player player = new HumanPlayer("name", 0, CheckerColor.Red, 15, 0);
            player.SetCheckerBack();
        }

        [TestMethod]
        public void TestSetCheckerBackWhenThereIsCheckerHit()
        {
            Player player = new HumanPlayer("name", 0, CheckerColor.Red, 15, 0);
            player.SetCheckerHit();
            Assert.IsTrue(player.CheckersHit == 1);
            Assert.IsTrue(player.CheckersOnBoard == 14);
            player.SetCheckerBack();
            Assert.IsTrue(player.CheckersHit == 0);
            Assert.IsTrue(player.CheckersOnBoard == 15);
        }

        [TestMethod]
        public void TestRivalPlayer()
        {
            Assert.IsTrue(Player.RivalPlayer(0) == 1);
            Assert.IsTrue(Player.RivalPlayer(1) == 0);
        }
    }
}
