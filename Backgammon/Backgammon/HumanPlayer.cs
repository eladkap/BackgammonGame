using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class HumanPlayer : Player
    {
        public HumanPlayer() : base()
        {
        }

        public HumanPlayer(string playerName, int playerNumber, CheckerColor checkerColor, int checkersNumber, int score)
            : base(playerName, playerNumber, checkerColor, checkersNumber, score)
        {
        }

        static bool TryParseInput(string inputString, out char action, out int srcTri, out int destTri)
        {
            action = 't';
            srcTri = -1;
            destTri = -1;
            if (inputString == null)
            {
                return false;
            }
            char[] seperators = { ',', ' ' };
            string[] strings = inputString.Split(seperators);
            if (strings == null || strings.Length != 2)
            {
                return false;
            }
            bool legalSrc = int.TryParse(strings[0], out srcTri) && Triangle.IsLegalIndex(srcTri - 1);
            bool legalDest = int.TryParse(strings[1], out destTri) && Triangle.IsLegalIndex(destTri - 1);
            bool isRetrievalAction = strings[0].ToLower().Equals("c") && legalDest;
            bool isRemovalAction = legalSrc && strings[1].ToLower().Equals("o");
            if (isRetrievalAction)
            {
                action = 'c';
            }
            else if (isRemovalAction)
            {
                action = 'o';
            }
            else
            {
                action = 't';
            }
            return (legalSrc && legalDest) || (isRetrievalAction) || (isRemovalAction);
        }

        private void GetInput(out char action, out int srcTri, out int destTri)
        {
            string inputString;
            bool isLegalInput = false;
            do
            {
                Console.WriteLine($"Player {_game.CurrentTurn + 1}, Enter input: Transfer: <source> <dest>  OR  Retrieval: 'c' <dest>  OR  Removal: <source> 'o'");
                inputString = Console.ReadLine();
                isLegalInput = TryParseInput(inputString, out action, out srcTri, out destTri);
                if (!isLegalInput)
                {
                    Console.WriteLine("Illegal step. Try again...");
                    Console.ReadKey();
                }
            } while (!isLegalInput);
        }

        public override void ChooseStep(object obj)
        {
            char action = 'x';
            int srcTri = -1;
            int destTri = -1;
            BackgammonGame game = (BackgammonGame)obj;
            if (!game.CanPlayerPerformAnyStep())
            {
                return;
            }
            GetInput(out action, out srcTri, out destTri);
            PlayerMovedEventArgs stepData = new PlayerMovedEventArgs(action, srcTri, destTri);
            OnPlayerMoved(stepData);
        }
    }
}
