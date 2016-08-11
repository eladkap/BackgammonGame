using System.Collections.Generic;
using System;

namespace Backgammon
{
    interface IBackgammonGame
    {

        /// <summary>
        /// Property: Players array.
        /// </summary>
        Player[] PlayersArray { get; }

        /// <summary>
        /// Property: Players number.
        /// </summary>
        int PlayersNum { get; }

        /// <summary>
        /// Property: List of dice results in the last roll.
        /// </summary>
        List<int> DicesList { get; }

        /// <summary>
        /// Property: Dice faces number.
        /// </summary>
        int DiceFaces { get; }

        /// <summary>
        /// Property: Current turn: The index of the current player.
        /// </summary>
        int CurrentTurn { get; }

        /// <summary>
        /// Initializes players.
        /// </summary>
        /// <param name="player1">Player 1</param>
        /// <param name="player2">Player 2</param>
        void InitializePlayers(Player player1, Player player2);

        /// <summary>
        /// Initializes classic board with classic configurations.
        /// </summary>
        /// <param name="trianglesNumber"></param>
        void InitializeClassicBoard(int trianglesNumber);

        /// <summary>
        /// Roll one dice.
        /// </summary>
        /// <param name="gameRoutine">true means we are in the game routine, and false means we are in the pregame (decide who starts)</param>
        /// <returns>dice result 1-6</returns>
        int RollDice(bool gameRoutine);

        /// <summary>
        /// Check if the dices are doubled.
        /// </summary>
        /// <returns>True if the two dices have the same number</returns>
        bool DicesAreDouble();

        /// <summary>
        /// Switches turn from current player to the rival player, and clear dice list for next rolling.
        /// </summary>
        void SwitchTurn();

        /// <summary>
        /// Set starting player in the pregame.
        /// </summary>
        /// <param name="startingPlayer"></param>
        void SetCurrentPlayer(int startingPlayer);

        /// <summary>
        /// Checks if source and destination triangles fit a dice result, meaning that the step is legal according to the dice.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        /// <param name="dice">Dice result</param>
        /// <returns></returns>
        bool IsLegalStepByDiceResult(int srcTri, int destTri, int dice);

        /// <summary>
        /// Checks if any general step is legal according to a dice result and according to some condition on the destination triangle -
        /// for hit step or for regular transfer step (transfer without hit)
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        /// <param name="dice">Dice result</param>
        /// <param name="destTriCondition">Some condition on the destination triangle: for regular transfer or hit</param>
        /// <returns></returns>
        bool IsLegalGeneralTransferStepByDice(int srcTri, int destTri, int dice, bool destTriCondition);

        /// <summary>
        /// Check if the regular transfer step (without hit) is legal according to a dice result.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        /// <param name="dice">Dice result</param>
        /// <returns></returns>
        bool IsLegalRegularTransferStepByDice(int srcTri, int destTri, int dice);

        /// <summary>
        /// Check if the hit step is legal according to a dice result.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        /// <param name="dice">Dice result</param>
        /// <returns></returns>
        bool IsLegalHitStepByDice(int srcTri, int destTri, int dice);

        /// <summary>
        /// Checks if any general step is legal according to any dice result from all dices,
        /// and according to some predicator on source triangle, destination triangle and dice
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        /// <param name="pred">Predicator</param>
        /// <returns></returns>
        bool IsLegalGeneralTransferStep(int srcTri, int destTri, Func<int, int, int, bool> pred);

        /// <summary>
        /// Checks if regular transfer step is legal according to any dice result from all dices.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        /// <returns></returns>
        bool IsLegalRegularTransferStep(int srcTri, int destTri);

        /// <summary>
        /// Checks if hit step is legal according to any dice result from all dices.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <param name="destTri">Destination triangle index</param>
        /// <returns></returns>
        bool IsLegalHitStep(int srcTri, int destTri);

        /// <summary>
        /// Checks if destination triangle is going to be hit while retrieving a checker back into the board.
        /// </summary>
        /// <param name="destTri">Destination triangle index</param>
        /// <returns></returns>
        bool IsDestinationTriangleIsHit(int destTri);

        /// <summary>
        /// Check if retrieval step is legal by any dice from dice list.
        /// </summary>
        /// <param name="destTri">Destination triangle index</param>
        /// <returns></returns>
        bool IsLegalRetrievalStep(int destTri);

        /// <summary>
        /// Check if Removal step is legal according to any dice from dice list.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <returns></returns>
        bool IsLegalRemovalStep(int srcTri);

        /// <summary>
        /// Adds dice result into dice list - after rolling.
        /// </summary>
        /// <param name="dice">Dice result</param>
        void AddDiceToList(int dice);

        /// <summary>
        /// Performs retrieval step.
        /// </summary>
        /// <param name="destTri">Destination triangle index</param>
        void PerformRetrievalStep(int destTri);

        /// <summary>
        /// Performs removal step.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        void PerformRemovalStep(int srcTri);

        /// <summary>
        /// Returns all possible destination triangles given a source triangle.
        /// </summary>
        /// <param name="srcTri">Source triangle index</param>
        /// <returns>list of indexes of all possible destination triangles the current player can tranfer to</returns>
        List<int> GetPossibleDestTriangles(int srcTri);

        /// <summary>
        /// Check if current player can play any general transfer step.
        /// </summary>
        /// <returns>True if the current player can play any general transfer step, and false otherwise</returns>
        bool CanGeneralTransferBePerformed();

        /// <summary>
        /// Check if current player can play any retrieval step.
        /// </summary>
        /// <returns>True if the current player can play any retrieval step, and false otherwise</returns>
        bool CanRetrievalBePerformed();

        /// <summary>
        /// Check if current player can play any removal step.
        /// </summary>
        /// <returns>True if the current player can play any removal step, and false otherwise</returns>
        bool CanRemoveBePerformed();

        /// <summary>
        /// Check if current player can perform any step (general transfer, retrieve or remove).
        /// </summary>
        /// <returns>true if the current player can perform any step, and false otherwise 
        /// (meaning: the current player "stucks",and then the turn should go to the other player</returns>
        bool CanPlayerPerformAnyStep();

        /// <summary>
        /// Returns the winner player number.
        /// </summary>
        /// <returns>Player number who won the game if the game is over, and -1 if the game is not over</returns>
        int GameWinner();

        /// <summary>
        /// Returns the looser player number.
        /// </summary>
        /// <returns>Player number who lost the game if the game is over, and -1 if the game is not over</returns>
        int GameLooser();

        /// <summary>
        /// Check if the game is over.
        /// </summary>
        /// <returns>True if game is over, and false otherwise</returns>
        bool IsGameOver();

        /// <summary>
        /// Checks if the win is regular - 1 points
        /// Regular win: The loosing player removed at least one checker.
        /// </summary>
        /// <returns></returns>
        bool IsRegularWin();

        /// <summary>
        /// Checks if the win is Mars - 2 points
        /// Mars: The loosing player didn't removed any checkers.
        /// </summary>
        /// <returns></returns>
        bool IsMarsWin();

        /// <summary>
        /// Checks if the win is Turkish Mars - 3 points
        /// Turkish Mars: The loosing player didn't removed any checkers and has at least one checker in his base.
        /// </summary>
        /// <returns></returns>
        bool IsTurkishMarsWin();

        /// <summary>
        /// Checks if the win is Stars Mars - 4 points
        /// Stars Mars: The loosing player didn't removed any checkers and has at least one captured checker.
        /// </summary>
        /// <returns></returns>
        bool IsStarsMarsWin();
    }
}
