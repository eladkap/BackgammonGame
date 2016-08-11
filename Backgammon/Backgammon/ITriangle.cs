namespace Backgammon
{
    interface ITriangle
    {
        /// <summary>
        /// Property: Triangle index from 0 to 23.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Property: Number of the player who owns the triangle. -1 if the triangle is empty.
        /// </summary>
        int PlayerNumber { get; }

        /// <summary>
        /// Property: Number of checkers the triangle fill.
        /// </summary>
        int CheckersNumber { get; }

        /// <summary>
        /// Sets the triangle properties.
        /// </summary>
        /// <param name="playerNumber">Player number 0 or 1</param>
        /// <param name="checkersNumber">Checkers number</param>
        void SetTriangle(int playerNumber, int checkersNumber);

        /// <summary>
        /// Checks if the triangle is empty of checkers.
        /// </summary>
        /// <returns>true if the triangle is empty of checkers and false otherwise</returns>
        bool IsEmpty();

        /// <summary>
        /// Check if the triangle contains one checker.
        /// </summary>
        /// <returns>true if the triangle fills one checker and false otherwise </returns>
        bool IsBlot();

        /// <summary>
        /// Check if player owns the triangle, meaning that the triangle contains checkers of the player.
        /// </summary>
        /// <param name="playerNumber">Player number 0 or 1</param>
        /// <returns>true if player owns the triangle and false otherwise</returns>
        bool IsOwnedByPlayer(int playerNumber);

        /// <summary>
        /// Checks if the triangle is ruled by the player, meaning that the player owns the triangle,
        /// and the triangle contains at least 2 checkers.
        /// </summary>
        /// <param name="playerNumber"></param>
        /// <returns>true if the triangle is ruled by the player and false otherwise</returns>
        bool IsRuledByPlayer(int playerNumber);

        /// <summary>
        /// Adds one checker of the player into triangle.
        /// This action is possible if one of the 3 holds:
        /// 1. Triangle is empty.
        /// 2. Triangle is owned by the current player.
        /// 3. Triangle is blot and owned by the rival player (HIT).
        /// </summary>
        /// <param name="playerNumber">Number of the owner player</param>
        void AddChecker(int playerNumber);

        /// <summary>
        /// Removes one checker from the triangle.
        /// This action is possible only if the triangle is not empty.
        /// </summary>
        void RemoveChecker();

        /// <summary>
        /// Removes all checkers from the triangle, and no one owns the triangle.
        /// </summary>
        void Clear();
    }
}
