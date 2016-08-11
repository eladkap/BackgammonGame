using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    interface IBoard
    {
        /// <summary>
        /// Property: Triangles array.
        /// </summary>
        Triangle[] TrianglesArray { get; }

        /// <summary>
        /// Property: Triangles number.
        /// </summary>
        int TrianglesNumber { get; }

        /// <summary>
        /// Sets triangle.
        /// </summary>
        /// <param name="triangleNumber">triangle number [0-23]</param>
        /// <param name="playerNumber">Player number</param>
        /// <param name="checkersNumber">Checkers number</param>
        void SetTriangle(int triangleNumber, int playerNumber, int checkersNumber);

        /// <summary>
        /// Adds checker to triangle.
        /// </summary>
        /// <param name="tri">Triangle number</param>
        /// <param name="playerNumber">Player number</param>
        void AddCheckerToTriangle(int tri, int playerNumber);

        /// <summary>
        /// Removes checker from triangle.
        /// </summary>
        /// <param name="tri">Triangle number</param>
        void RemoveCheckerFromTriangle(int tri);

        /// <summary>
        /// Sets the board according to triangles list.
        /// </summary>
        /// <param name="trianglesList">Triangles list</param>
        void Setup(List<Triangle> trianglesList);

        /// <summary>
        /// Sets the classic board.
        /// </summary>
        void SetupClassic();

        /// <summary>
        /// Returns list of triangles of the classic setup belong to some player.
        /// </summary>
        /// <param name="playerNumber">Player number</param>
        /// <returns>List of triangles of the classic setup</returns>
        List<Triangle> GetClassicTrianglesByPlayer(int playerNumber);

        /// <summary>
        /// Returns list of triangles that are symetric to the list parameter.
        /// </summary>
        /// <param name="list">Triangles list</param>
        /// <returns>Symetric triangles list</returns>
        List<Triangle> GetClassicSymetricTrianglesByPlayer(List<Triangle> list);

        /// <summary>
        /// Returns list of all triangles of the classic setup.
        /// </summary>
        /// <returns>List of all triangles of the classic setup</returns>
        List<Triangle> GetClassicTrianglesList();

        /// <summary>
        /// Checks if the triangle is owned by the player, meaning that the
        /// triangle has one or more checkers belong to the player.
        /// </summary>
        /// <param name="triangleNumber">Triangle number</param>
        /// <param name="playerNum">Player number</param>
        /// <returns>true if triangle is owned by the player and false otherwise</returns>
        bool IsTriangleOwnedByPlayer(int triangleNumber, int playerNum);

        /// <summary>
        /// Checks if the triangle is ruled by the player, meaning that the
        /// triangle has two or more checkers belong to the player.
        /// </summary>
        /// <param name="triangleNumber">Triangle number</param>
        /// <param name="playerNum">Player number</param>
        /// <returns>true if triangle is ruled by the player and false otherwise</returns>
        bool IsTriangleRuledByPlayer(int triangleNumber, int playerNum);

        /// <summary>
        /// Checks if the triangle has only one checker.
        /// </summary>
        /// <param name="triangleNumber">Triangle number</param>
        /// <returns>true if triangle has one checker and false otherwise</returns>
        bool IsTriangleBlot(int triangleNumber);

        /// <summary>
        /// Checks if the triangle is empty.
        /// </summary>
        /// <param name="triangleNumber">Triangle number</param>
        /// <returns>true if triangle is empty and false otherwise</returns>
        bool IsTriangleEmpty(int triangleNumber);

        /// <summary>
        /// Checks if the triangle belongs to the base of the player.
        /// Player 0: Triangles 1-6.
        /// Player 1: Triangles 19-24.
        /// </summary>
        /// <param name="playerNumber">Player number</param>
        /// <param name="triangleIndex">Triangle number</param>
        /// <returns>true if the triangle is in player's base and false otherwise</returns>
        bool IsBaseTriangle(int playerNumber, int triangleIndex);

        /// <summary>
        /// Returns the number the player's checkers in his base.
        /// </summary>
        /// <param name="playerNumber">Player number</param>
        /// <returns>Number of player's checkers in his base</returns>
        int CheckersNumberInPlayerBase(int playerNumber);

        /// <summary>
        /// Returns the index of the closest owned and not empty triangle to player's base.
        /// </summary>
        /// <param name="playerNumber">Player number</param>
        /// <returns>The index of the closest owned and not empty triangle to player's base</returns>
        int ClosestToBaseFilledTriangleIndex(int playerNumber);

        /// <summary>
        /// Clears all the triangles in the board.
        /// </summary>
        void ClearTriangles();
    }
}
