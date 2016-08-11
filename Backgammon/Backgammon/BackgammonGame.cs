using System;
using System.Collections.Generic;

namespace Backgammon
{
    public enum CheckerColor
    {
        White = 15,
        Red = 12,
        Blue = 9,
        Green = 10
    };

    public class BackgammonGame : IBackgammonGame
    {
        /// <summary>
        /// Number of initial checkers number for each player (15).
        /// </summary>
        private readonly int _initialCheckersNumber;

        /// <summary>
        /// Dice.
        /// </summary>
        private Dice _dice;

        /// <summary>
        /// List of dice Results in the last roll.
        /// </summary>
        private List<int> _dicesList;

        /// <summary>
        /// Players array (2 players).
        /// </summary>
        private Player[] _playersArray;

        /// <summary>
        /// Game board.
        /// </summary>
        private Board _board;

        /// <summary>
        /// Current player number.
        /// </summary>
        private int _currentTurn;

        public BackgammonGame(Player player1, Player player2, int trianglesNumber, int diceFaces)
        {
            _initialCheckersNumber = player1.CheckersOnBoard;
            _currentTurn = 0;
            _playersArray = new Player[2];
            _dicesList = new List<int>();
            _dice = new Dice(diceFaces);
            InitializePlayers(player1, player2);
            InitializeClassicBoard(trianglesNumber);
        }

        public Player this[int playerIndex]
        {
            get
            {
                if (!Player.IsLegalPlayerNumber(playerIndex))
                {
                    throw new Exception("Exception in method BackgammonGame.operator[]: Out of bounds.");
                }
                return PlayersArray[playerIndex];
            }
        }

        public int TrianglesNumber
        {
            get { return _board.TrianglesNumber; }
        }

        public Player[] PlayersArray { get { return _playersArray; } }

        public int PlayersNum { get { return _playersArray.Length; } }

        public Board Board { get { return _board; } }

        public int CurrentTurn { get { return _currentTurn; } }

        public List<int> DicesList { get { return _dicesList; } }

        public int DiceFaces { get { return _dice.Faces; } }

        public void InitializePlayers(Player player1, Player player2)
        {
            for (int i = 0; i < PlayersNum; i++)
            {
                _playersArray[i] = new HumanPlayer();
            }
            player1._game = this;
            player2._game = this;
            _playersArray[0] = player1;
            _playersArray[1] = player2;
        }

        public void InitializeClassicBoard(int trianglesNumber)
        {
            _board = new Board(trianglesNumber);
            _board.SetupClassic();
        }

        public int RollDice(bool gameRoutine)
        {
            int result = _dice.Roll();
            if (gameRoutine)
            {
                _dicesList.Add(result);
            }
            return result;
        }

        public bool DicesAreDouble()
        {
            return _dicesList.Count == 4;
        }

        public void SwitchTurn()
        {
            _currentTurn = Player.RivalPlayer(CurrentTurn);
            _dicesList.Clear();
        }

        public void SetCurrentPlayer(int currentPlayer)
        {
            _currentTurn = currentPlayer;
        }

        public bool IsLegalStepByDiceResult(int srcTri, int destTri, int dice)
        {
            if (CurrentTurn == 0)
            {
                return destTri - srcTri == dice;
            }
            else if (CurrentTurn == 1)
            {
                return srcTri - destTri == dice;
            }
            return false;
        }

        public bool IsLegalGeneralTransferStepByDice(int srcTri, int destTri, int dice, bool destTriCondition)
        {
            // source triangle is not empty and owned by the current player.
            bool legalSource = !_board.IsTriangleEmpty(srcTri) && _board.IsTriangleOwnedByPlayer(srcTri, CurrentTurn);
            // source and destination triangles indexes fit a dice number.
            bool stepIsLegalByDiceResult = IsLegalStepByDiceResult(srcTri, destTri, dice);
            return legalSource && destTriCondition && stepIsLegalByDiceResult;
        }

        public bool IsLegalRegularTransferStepByDice(int srcTri, int destTri, int dice)
        {
            // destination triangle is empty or owned by ther current player.
            bool destTriCondition = _board.IsTriangleEmpty(destTri) || _board.IsTriangleOwnedByPlayer(destTri, CurrentTurn);
            return IsLegalGeneralTransferStepByDice(srcTri, destTri, dice, destTriCondition);
        }

        public bool IsLegalHitStepByDice(int srcTri, int destTri, int dice)
        {
            // destination triangle is blot and owned by the rival player.
            bool destTriCondition = _board.IsTriangleBlot(destTri) && _board.IsTriangleOwnedByPlayer(destTri, Player.RivalPlayer(CurrentTurn));
            return IsLegalGeneralTransferStepByDice(srcTri, destTri, dice, destTriCondition);
        }

        public bool IsLegalGeneralTransferStep(int srcTri, int destTri, Func<int, int, int, bool> pred)
        {
            if (PlayersArray[CurrentTurn].CheckersHit > 0)
            {
                return false;
            }
            foreach (int dice in _dicesList)
            {
                if (pred(srcTri, destTri, dice))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsLegalRegularTransferStep(int srcTri, int destTri)
        {
            return IsLegalGeneralTransferStep(srcTri, destTri, IsLegalRegularTransferStepByDice);
        }

        public bool IsLegalHitStep(int srcTri, int destTri)
        {
            return IsLegalGeneralTransferStep(srcTri, destTri, IsLegalHitStepByDice);
        }


        public bool IsDestinationTriangleIsHit(int destTri)
        {
            return _board.IsTriangleBlot(destTri) && _board.IsTriangleOwnedByPlayer(destTri, Player.RivalPlayer(CurrentTurn));
        }


        public bool IsLegalRetrievalStep(int destTri)
        {
            if (PlayersArray[CurrentTurn].CheckersHit == 0)
            {
                return false;
            }
            bool isBaseTriangle = _board.IsBaseTriangle(CurrentTurn, destTri);
            // fit some dice
            foreach (int dice in _dicesList)
            {
                if (CurrentTurn == 0)
                {
                    if (isBaseTriangle && dice == destTri + 1 && !_board.IsTriangleRuledByPlayer(destTri, Player.RivalPlayer(CurrentTurn)))
                    {
                        return true;
                    }
                }
                else
                {
                    if (isBaseTriangle && dice == _board.TrianglesNumber - destTri && !_board.IsTriangleRuledByPlayer(destTri, Player.RivalPlayer(CurrentTurn)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AreAllCheckersInRivalBase()
        {
            if (CurrentTurn == 0)
            {
                return _board.ClosestToBaseFilledTriangleIndex(CurrentTurn) >= 18;
            }
            else
            {
                return _board.ClosestToBaseFilledTriangleIndex(CurrentTurn) <= 5;
            }
        }

        public bool IsLegalRegularRemoveByDice(int srcTri, int dice)
        {
            if (CurrentTurn == 0)
            {
                return AreAllCheckersInRivalBase() && _board.IsTriangleOwnedByPlayer(srcTri, CurrentTurn) && dice == _board.TrianglesNumber - srcTri;
            }
            else
            {
                return AreAllCheckersInRivalBase() && _board.IsTriangleOwnedByPlayer(srcTri, CurrentTurn) && dice == srcTri + 1;
            }
        }

        public bool IsLegalSpecialRemoveByDice(int srcTri, int dice)
        {
            int offset = 0;
            if (CurrentTurn == 0)
            {
                offset = _board.TrianglesNumber - _board.ClosestToBaseFilledTriangleIndex(CurrentTurn);
            }
            else
            {
                offset = _board.ClosestToBaseFilledTriangleIndex(CurrentTurn);
            }
            return (AreAllCheckersInRivalBase()) && (srcTri == _board.ClosestToBaseFilledTriangleIndex(CurrentTurn)) && (dice > offset);
        }

        /// <summary>
        /// Check if Removal step is legal according to dice result.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="dice">Dice result</param>
        /// <returns></returns>
        public bool IsLegalRemoveByDice(int srcTri, int dice)
        {
            return IsLegalRegularRemoveByDice(srcTri, dice) || IsLegalSpecialRemoveByDice(srcTri, dice);
        }


        public bool IsLegalRemovalStep(int srcTri)
        {
            if (PlayersArray[CurrentTurn].CheckersHit > 0)
            {
                return false;
            }
            foreach (int dice in _dicesList)
            {
                if (IsLegalRemoveByDice(srcTri, dice))
                {
                    return true;
                }
            }
            return false;
        }


        public void AddDiceToList(int dice)
        {
            _dicesList.Add(dice);
        }

        /// <summary>
        /// Removes from dice list the dice result that fit the general transfer step.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        public void UpdateDiceListAfterGeneralTransferStep(int srcTri, int destTri)
        {
            _dicesList.Remove(Math.Abs(srcTri - destTri));
        }

        /// <summary>
        /// Removes from dice list the dice result that fit the retreival step.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        public void UpdateDiceListAfterRetreivalStep(int destTri)
        {
            if (CurrentTurn == 0)
            {
                _dicesList.Remove(destTri + 1);
            }
            else
            {
                _dicesList.Remove(_board.TrianglesNumber - destTri);
            }
        }

        /// <summary>
        /// Removes from dice list the dice result that fit the removal step.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        public void UpdateDiceListAfterRegularRemovalStep(int srcTri)
        {
            if (CurrentTurn == 0)
            {
                _dicesList.Remove(_board.TrianglesNumber - srcTri);
            }
            else
            {
                _dicesList.Remove(srcTri + 1);
            }
        }

        public void UpdateDiceListAfterSpecialRemovalStep()
        {
            _dicesList.RemoveAt(0);
        }

        /// <summary>
        /// Performs regular transfer (not hit) of a checker from source triangle to destination triangle,
        /// when the destination triangle is empty or owned by the current player.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        public void PerformRegularTransferStep(int srcTri, int destTri)
        {
            if (!_board.IsLegalTriangleIndex(srcTri) || !_board.IsLegalTriangleIndex(destTri) || !Player.IsLegalPlayerNumber(CurrentTurn))
            {
                throw new Exception("Execption in method BackgammonGame.TransferChecker: Illegal parameters.");
            }
            _board.RemoveCheckerFromTriangle(srcTri);
            _board.AddCheckerToTriangle(destTri, CurrentTurn);
            UpdateDiceListAfterGeneralTransferStep(srcTri, destTri);
        }

        /// <summary>
        /// Performs hit step: current player transfer his checker into the destination triangle,
        /// his checker replaces the rival's one checker, and hit it.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        public void PerformHitStep(int srcTri, int destTri)
        {
            _board.RemoveCheckerFromTriangle(srcTri);
            _board.RemoveCheckerFromTriangle(destTri);
            _board.AddCheckerToTriangle(destTri, CurrentTurn);
            PlayersArray[Player.RivalPlayer(CurrentTurn)].SetCheckerHit();
            UpdateDiceListAfterGeneralTransferStep(srcTri, destTri);
        }

        /// <summary>
        /// Performs hit action while retrieving a checker into the board.
        /// </summary>
        /// <param name="destTri">Destination triangle index</param>
        private void HitCheckerInRetrievalStep(int destTri)
        {
            _board.RemoveCheckerFromTriangle(destTri);
            _board.AddCheckerToTriangle(destTri, CurrentTurn);
            PlayersArray[Player.RivalPlayer(CurrentTurn)].SetCheckerHit();
        }

        public void PerformRetrievalStep(int destTri)
        {
            if (IsDestinationTriangleIsHit(destTri))
            {
                HitCheckerInRetrievalStep(destTri);
            }
            else
            {
                _board.AddCheckerToTriangle(destTri, CurrentTurn);
            }
            PlayersArray[CurrentTurn].SetCheckerBack();
            UpdateDiceListAfterRetreivalStep(destTri);
        }

        private bool IsSpecialRemoval(int srcTri)
        {
            return (CurrentTurn == 0 && !_dicesList.Contains(TrianglesNumber - srcTri) ||
                CurrentTurn == 1 && !_dicesList.Contains(srcTri + 1));
        }

        public void PerformRemovalStep(int srcTri)
        {
            if (IsSpecialRemoval(srcTri))
            {
                UpdateDiceListAfterSpecialRemovalStep();
            }
            else
            {
                UpdateDiceListAfterRegularRemovalStep(srcTri);
            }
            _board.RemoveCheckerFromTriangle(srcTri);
            PlayersArray[CurrentTurn].RemoveChecker();
        }

        public int GameWinner()
        {
            foreach (var player in _playersArray)
            {
                if (player.CheckersOnBoard == 0)
                {
                    return player.PlayerNumber;
                }
            }
            return -1;
        }

        public int GameLooser()
        {
            if (IsGameOver())
            {
                return Player.RivalPlayer(GameWinner());
            }
            return -1;
        }

        public bool IsGameOver()
        {
            return GameWinner() != -1;
        }

        public bool IsRegularWin()
        {
            return IsGameOver() && PlayersArray[GameLooser()].CheckersOnBoard < _initialCheckersNumber;
        }

        public bool IsMarsWin()
        {
            return IsGameOver() && PlayersArray[GameLooser()].CheckersOnBoard + PlayersArray[GameLooser()].CheckersHit == _initialCheckersNumber;
        }

        public bool IsTurkishMarsWin()
        {
            return IsMarsWin() && _board.CheckersNumberInPlayerBase(GameLooser()) > 0;
        }

        public bool IsStarsMarsWin()
        {
            return IsMarsWin() && PlayersArray[GameLooser()].CheckersHit > 0;
        }

        public void IncrementWinnerScoreBy(int points)
        {
            PlayersArray[GameWinner()].Score += points;
        }

        public List<int> GetPossibleDestTriangles(int srcTri)
        {
            List<int> destTriList = new List<int>();
            for (int destTri = 0; destTri < _board.TrianglesNumber; destTri++)
            {
                if (IsLegalRegularTransferStep(srcTri, destTri) || IsLegalHitStep(srcTri, destTri))
                {
                    destTriList.Add(destTri);
                }
            }
            return destTriList;
        }

        public bool CanGeneralTransferBePerformed()
        {
            for (int srcTri = 0; srcTri < _board.TrianglesNumber; srcTri++)
            {
                if (_board.IsTriangleOwnedByPlayer(srcTri, CurrentTurn) && GetPossibleDestTriangles(srcTri).Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanRetrievalBePerformed()
        {
            if (PlayersArray[CurrentTurn].CheckersHit == 0)
            {
                return false;
            }
            int part = _board.TrianglesNumber / 4;
            if (CurrentTurn == 1)
            {
                for (int destTri = _board.TrianglesNumber - part; destTri < _board.TrianglesNumber; destTri++)
                {
                    if (IsLegalRetrievalStep(destTri))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int destTri = 0; destTri < part; destTri++)
                {
                    if (IsLegalRetrievalStep(destTri))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanRemoveBePerformed()
        {
            int part = _board.TrianglesNumber / 4;
            if (CurrentTurn == 0)
            {
                for (int srcTri = _board.TrianglesNumber - part; srcTri < _board.TrianglesNumber; srcTri++)
                {
                    if (IsLegalRemovalStep(srcTri))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int srcTri = 0; srcTri < part; srcTri++)
                {
                    if (IsLegalRemovalStep(srcTri))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanPlayerPerformAnyStep()
        {
            return CanRetrievalBePerformed() || CanRemoveBePerformed() || CanGeneralTransferBePerformed();
        }
    }
}
