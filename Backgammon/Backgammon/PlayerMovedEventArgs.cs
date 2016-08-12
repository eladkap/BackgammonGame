using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class PlayerMovedEventArgs : EventArgs
    {
        /// <summary>
        /// Action to perform - 't' transfer, 'c' retrieval, 'o' Removal.
        /// </summary>
        public readonly char action;

        /// <summary>
        /// Source triangle index.
        /// </summary>
        public readonly int srcTri;

        /// <summary>
        /// Destination triangle index.
        /// </summary>
        public readonly int destTri;

        internal PlayerMovedEventArgs(char action, int srcTri, int destTri)
        {
            this.action = action;
            this.srcTri = srcTri;
            this.destTri = destTri;
        }
    }
}
