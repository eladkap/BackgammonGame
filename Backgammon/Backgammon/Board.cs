using System;
using System.Collections.Generic;

namespace Backgammon
{
    public class Board : IBoard
    {
        /// <summary>
        /// Triangles array with size 24.
        /// </summary>
        private Triangle[] _trianglesArray;

        public Board(int trianglesNumber)
        {
            if (trianglesNumber <= 0)
            {
                throw new Exception("Exception in method Board.Board: nonpositive number.");
            }
            _trianglesArray = new Triangle[trianglesNumber];
            InitializeTriangles();
            SetupClassic();
        }

        public bool IsLegalTriangleIndex(int index)
        {
            return index >= 0 && index < TrianglesNumber;
        }

        public Triangle[] TrianglesArray { get { return _trianglesArray; } }

        public int TrianglesNumber { get { return _trianglesArray.Length; } }

        public Triangle this[int index]
        {
            get
            {
                if (!IsLegalTriangleIndex(index))
                {
                    throw new Exception("Exception in method Board.operator[]: Out of bounds.");
                }
                return TrianglesArray[index];
            }
        }

        private void InitializeTriangles()
        {
            for (int i = 0; i < TrianglesNumber; i++)
            {
                _trianglesArray[i] = new Triangle(i, 0, -1);
            }
        }

        public void SetTriangle(int triangleNumber, int playerNumber, int checkersNumber)
        {
            if (!IsLegalTriangleIndex(triangleNumber) || !Player.IsLegalPlayerNumber(playerNumber) || checkersNumber < 0)
            {
                throw new Exception("Exception in method Board.SetTriangle: Illegal parameters.");
            }
            _trianglesArray[triangleNumber].SetTriangle(playerNumber, checkersNumber);
        }

        /// <summary>
        /// Adds checker to triangle
        /// </summary>
        /// <param name="tri">Triangle number</param>
        /// <param name="playerNumber">Player number</param>
        public void AddCheckerToTriangle(int tri, int playerNumber)
        {
            if (!IsLegalTriangleIndex(tri) || !Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Exception in method Board.AddCheckerToTriangle: Illegal parameters.");
            }
            TrianglesArray[tri].AddChecker(playerNumber);
        }

        public void RemoveCheckerFromTriangle(int tri)
        {
            if (!IsLegalTriangleIndex(tri))
            {
                throw new Exception("Execption in method Board.RemoveCheckerFromTriangle: Illegal parameters.");
            }
            TrianglesArray[tri].RemoveChecker();
        }

        public void Setup(List<Triangle> trianglesList)
        {
            if (trianglesList == null)
            {
                throw new Exception("Exception in method Board.Setup: null pointer.");
            }
            foreach (Triangle tri in trianglesList)
            {
                SetTriangle(tri.Index, tri.PlayerNumber, tri.CheckersNumber);
            }
        }
  
        public void SetupClassic()
        {
            Setup(GetClassicTrianglesList());
        }

        public List<Triangle> GetClassicTrianglesByPlayer(int playerNumber)
        {
            if (!Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Exception in method Board.GetClassicTrianglesByPlayer: Illegal parameter.");
            }
            List<Triangle> trianglesList = new List<Triangle>();
            trianglesList.Add(new Triangle(0, 2, playerNumber));
            trianglesList.Add(new Triangle(11, 5, playerNumber));
            trianglesList.Add(new Triangle(16, 3, playerNumber));
            trianglesList.Add(new Triangle(18, 5, playerNumber));
            return trianglesList;
        }

        public List<Triangle> GetClassicSymetricTrianglesByPlayer(List<Triangle> list)
        {
            if (list == null)
            {
                throw new Exception("Exception in method Board.GetClassicSymetricTrianglesByPlayer: null pointer.");
            }
            List<Triangle> trianglesList = new List<Triangle>();
            int x = TrianglesNumber - 1;
            foreach (Triangle tri in list)
            {
                trianglesList.Add(new Triangle(x - tri.Index, tri.CheckersNumber, Player.RivalPlayer(tri.PlayerNumber)));
            }
            return trianglesList;
        }

        public List<Triangle> GetClassicTrianglesList()
        {
            List<Triangle> trianglesList1 = GetClassicTrianglesByPlayer(0);
            List<Triangle> trianglesList2 = GetClassicSymetricTrianglesByPlayer(trianglesList1);
            List<Triangle> trianglesList12 = new List<Triangle>();
            trianglesList12.AddRange(trianglesList1);
            trianglesList12.AddRange(trianglesList2);
            return trianglesList12;
        }

        public bool IsTriangleOwnedByPlayer(int triangleNumber, int playerNum)
        {
            if (!IsLegalTriangleIndex(triangleNumber) || !Player.IsLegalPlayerNumber(playerNum))
            {
                throw new Exception("Execption in method Board.IsTriangleOwnedByPlayer: Illegal parameters.");
            }
            return TrianglesArray[triangleNumber].IsOwnedByPlayer(playerNum);
        }

        public bool IsTriangleRuledByPlayer(int triangleNumber, int playerNum)
        {
            if (!IsLegalTriangleIndex(triangleNumber) || !Player.IsLegalPlayerNumber(playerNum))
            {
                throw new Exception("Execption in method Board.IsTriangleRuledByPlayer: Illegal parameters.");
            }
            return TrianglesArray[triangleNumber].IsRuledByPlayer(playerNum);
        }
     
        public bool IsTriangleBlot(int triangleNumber)
        {
            if (!IsLegalTriangleIndex(triangleNumber))
            {
                throw new Exception("Execption in method Board.IsTriangleBlot: Illegal parameters.");
            }
            return TrianglesArray[triangleNumber].IsBlot();
        }

        public bool IsTriangleEmpty(int triangleNumber)
        {
            if (!IsLegalTriangleIndex(triangleNumber))
            {
                throw new Exception("Execption in method Board.IsTriangleEmpty: Illegal parameters.");
            }
            return TrianglesArray[triangleNumber].IsEmpty();
        }
 
        public bool IsBaseTriangle(int playerNumber, int triangleIndex)
        {
            if (!IsLegalTriangleIndex(triangleIndex) || !Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Execption in method Board.IsBaseTriangle: Illegal parameters.");
            }
            if (playerNumber == 0)
            {
                return triangleIndex >= 0 && triangleIndex < TrianglesNumber / 4;
            }
            else if (playerNumber == 1)
            {
                return triangleIndex >= TrianglesNumber - TrianglesNumber / 4 && triangleIndex < TrianglesNumber;
            }
            return false;
        }
 
        public int CheckersNumberInPlayerBase(int playerNumber)
        {
            if (!Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Execption in method Board.CheckersNumberInPlayerBase: Illegal parameters.");
            }
            int count = 0;
            for (int triIndex = 0; triIndex < TrianglesArray.Length; ++triIndex)
            {
                if (IsBaseTriangle(playerNumber, triIndex) && TrianglesArray[triIndex].IsOwnedByPlayer(playerNumber))
                {
                    count += TrianglesArray[triIndex].CheckersNumber;
                }
            }
            return count;
        }

        public int ClosestToBaseFilledTriangleIndex(int playerNumber)
        {
            if (!Player.IsLegalPlayerNumber(playerNumber))
            {
                throw new Exception("Execption in method Board.ClosestToBaseFilledTriangleIndex: Illegal parameters.");
            }
            if (playerNumber == 0)
            {
                for (int triIndex = 0; triIndex < TrianglesNumber; triIndex++)
                {
                    if (!TrianglesArray[triIndex].IsEmpty() && TrianglesArray[triIndex].IsOwnedByPlayer(playerNumber))
                    {
                        return triIndex;
                    }
                }
            }
            else if (playerNumber == 1)
            {
                for (int triIndex = TrianglesNumber - 1; triIndex >= 0; triIndex--)
                {
                    if (!TrianglesArray[triIndex].IsEmpty() && TrianglesArray[triIndex].IsOwnedByPlayer(playerNumber))
                    {
                        return triIndex;
                    }
                }
            }
            return -1;
        }

        public void ClearTriangles()
        {
            foreach (var triangle in TrianglesArray)
            {
                triangle.Clear();
            }
        }
    }
}

