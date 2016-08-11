namespace Backgammon
{
    interface IDice
    {
        /// <summary>
        /// Property: faces number.
        /// </summary>
        int Faces { get; }

        /// <summary>
        /// Rolls a dice.
        /// </summary>
        /// <returns>Dice result: 1 - 6 in an ordinary dice</returns>
        int Roll();
    }
}
