namespace Backgammon
{
    interface IPlayer
    {
        /// <summary>
        /// Property: player name.
        /// </summary>
        string PlayerName { get; }

        /// <summary>
        /// Property: player number: 0 or 1.
        /// </summary>
        int PlayerNumber { get; }

        /// <summary>
        /// Property: checker color.
        /// </summary>
        CheckerColor CheckerColor { get; }

        /// <summary>
        /// Property: Number of checkers on board.
        /// </summary>
        int CheckersOnBoard { get; }

        /// <summary>
        /// Property: Number of hit checkers (captured checkers).
        /// </summary>
        int CheckersHit { get; }

        /// <summary>
        /// Property: Player score.
        /// </summary>
        int Score { get; set; }

        /// <summary>
        /// Sets the player properties.
        /// </summary>
        /// <param name="playerName">Player name</param>
        /// <param name="playerNumber">Player number</param>
        /// <param name="checkerColor">Checker color</param>
        /// <param name="checkersNumber">Checkers number</param>
        /// <param name="score">Score</param>
        void SetProperties(string playerName, int playerNumber, CheckerColor checkerColor, int checkersNumber, int score);

        /// <summary>
        /// Returns an action to perform in the next step.
        /// Returns 'x' in action if there is no possible steps.
        /// Abstract method:
        /// For human players - get an input from the user.
        /// For AI players - chooses the best step using an algorithm.
        /// </summary>
        /// <param name="srcTri">Source triangle</param>
        /// <param name="destTri">Destination triangle</param>
        /// <returns>An action 't' or 'c' or 'o' (transfer or retrieval or removal respectively)</returns>
        void ChooseStep(object obj);

        /// <summary>
        /// Add hit checker after hit action.
        /// </summary>
        void SetCheckerHit();

        /// <summary>
        /// Substruct hit checker after retrieving back the checker to the board.
        /// </summary>
        void SetCheckerBack();

        /// <summary>
        /// Substruct one checker from checkers on board after removing a checker from the board.
        /// </summary>
        void RemoveChecker();
    }
}
