using System;

namespace Backgammon
{
    internal class Dice : IDice
    {
        /// <summary>
        /// Random variable getting value between 1 and dice faces number (6).
        /// </summary>
        private Random _rnd;

        /// <summary>
        /// Number of dice face (6).
        /// </summary>
        private readonly int _faces;

        public Dice(int faces)
        {
            int seed = (int)DateTime.Now.Ticks;
            _rnd = new Random(seed);
            _faces = faces;
        }

        public int Faces { get { return _faces; } }

        public int Roll()
        {
            return _rnd.Next(_faces) + 1;
        }
    }
}
