using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    /// <summary>
    /// Note: I didn't have much time to write the algorithm code for AI player, but this class is ready for implementation :)
    /// The algorithm code should be implemented in the method ChooseBestStep().
    /// </summary>
    public class AIPlayer : Player
    {
        public AIPlayer() : base()
        {
        }

        public AIPlayer(string playerName, int playerNumber, CheckerColor checkerColor, int checkersNumber, int score)
            : base(playerName, playerNumber, checkerColor, checkersNumber, score)
        {
        }

        // This method analyzes the game board using _game reference in the class,
        // and chooses the best step to perform - either regular transfer (without hit), hit transfer, removal or retrieve.
        // The output is by ref in includes:
        // action: 't' for transfer, 'c' for retreival, 'o' for removal, and 'x' if there is no possible step of any kind.
        // srcTri: the source triangle that the checker should be transfered or removed from.
        // destTri: the destination triangle that the checker should be transfered or retreive to.
        public void ChooseBestStep(out char action, out int srcTri, out int destTri)
        {
            action = 'x';
            srcTri = -1;
            destTri = -1;
            // algorithm implementation...
        }

        public override void ChooseStep(out char action, out int srcTri, out int destTri)
        {
            action = 'x';
            srcTri = -1;
            destTri = -1;
            if (!_game.CanPlayerPerformAnyStep())
            {
                return;
            }
            ChooseBestStep(out action, out srcTri, out destTri);
        }
    }
}
