using System;

namespace Backgammon
{
    public class Triangle : ITriangle
    {
        /// <summary>
        /// Index of the triangle between 0 to 23, read only because the index cannot be changed.
        /// </summary>
        private readonly int _index;

        /// <summary>
        /// Checkers numbers located in the triangle.
        /// </summary>
        private int _checkersNumber;

        /// <summary>
        /// The player who owns the triangle(0 or 1), or -1 if triangle is empty.
        /// </summary>
        private int _playerNumber;
 
        public Triangle(int index, int checkersNumber, int playerNumber)
        {
            if (!IsLegalIndex(index) || !IsLegalCheckersNumber(checkersNumber) || !Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Exception in full constructor of Triangle: Illegal parameters.");
            }
            _index = index;
            SetTriangle(playerNumber, checkersNumber);
        }

        public Triangle() : this(0, 0, -1)
        {
        }

        public int Index { get { return _index; } }

        public int PlayerNumber { get { return _playerNumber; } }
 
        public int CheckersNumber { get { return _checkersNumber; } }

        public static bool IsLegalIndex(int index)
        {
            return index >= 0 && index < Constants.TrianglesNumber;
        }

        public static bool IsLegalCheckersNumber(int checkersNumber)
        {
            return checkersNumber >= 0 && checkersNumber <= Constants.CheckersNumber;
        }

        public void SetTriangle(int playerNumber, int checkersNumber)
        {
            _playerNumber = playerNumber;
            _checkersNumber = checkersNumber;
        }

        public bool IsEmpty()
        {
            return CheckersNumber == 0;
        }

        public bool IsBlot()
        {
            return CheckersNumber == 1;
        }

        public bool IsOwnedByPlayer(int playerNumber)
        {
            if (!Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Exception in method Triangle.IsOwnedByPlayer: playerNumber is invalid (need to be 0 or 1).");
            }
            return PlayerNumber == playerNumber;
        }

        public bool IsRuledByPlayer(int playerNumber)
        {
            if (!Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Exception in method Triangle.IsRuledByPlayer: playerNumber is invalid.");
            }
            return _checkersNumber > 1 && IsOwnedByPlayer(playerNumber);
        }

        public void AddChecker(int playerNumber)
        {
            if (!Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Exception in method Triangle.AddChecker: playerNumber is invalid (need to be 0 or 1).");
            }
            if (IsRuledByPlayer(Player.RivalPlayer(playerNumber)))
            {
                throw new Exception("Execption in method Triangle.AddChecker: triangle is ruled by the rival player.");
            }
            if (IsEmpty() || IsOwnedByPlayer(playerNumber))
            {
                ++_checkersNumber;
            }
            _playerNumber = playerNumber;
        }

        public void RemoveChecker()
        {
            if (IsEmpty())
            {
                throw new Exception("Execption in method Triangle.RemoveChecker: triangle is already empty.");
            }
            --_checkersNumber;
            if (CheckersNumber == 0)
            {
                _playerNumber = -1;
            }
        }

        public void Clear()
        {
            _playerNumber = -1;
            _checkersNumber = 0;
        }
    }
}
