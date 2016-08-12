using System;
using Backgammon;

namespace BackgammonConsoleApp
{
    class Program
    {
        static BackgammonGame InitializeGame(bool isFirstRound, int score1, int score2)
        {
            int diceFaces = Constants.DiceFaces;
            int triangles = Constants.TrianglesNumber;
            int checkerNumber = Constants.CheckersNumber;
            Player player1 = new HumanPlayer("Human 1", 0, CheckerColor.White, checkerNumber, isFirstRound ? 0 : score1);
            Player player2 = new HumanPlayer("Human 2", 1, CheckerColor.Green, checkerNumber, isFirstRound ? 0 : score2);
            return CreateBackgammonGame(player1, player2, triangles, diceFaces);
        }

        static BackgammonGame CreateBackgammonGame(Player player1, Player player2, int trianglesNumber, int diceFaces)
        {
            BackgammonGame game = new BackgammonGame(player1, player2, trianglesNumber, diceFaces);
            return game;
        }

        static void ChangeConsoleColor(BackgammonGame game, int playerNumber)
        {
            Console.ForegroundColor = (ConsoleColor)game.PlayersArray[playerNumber].CheckerColor;
        }

        static int RollDice(BackgammonGame game, int playerNumber)
        {
            int result = 0;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"Player {playerNumber + 1}: Press enter to roll dice.");
            Console.ReadLine();
            result = game.RollDice(false);
            Console.WriteLine($"Dice {playerNumber}: {result}");
            return result;
        }

        static void RollTwoDices(BackgammonGame game)
        {
            Console.Write($"Player {game.CurrentTurn + 1}: Press enter to roll two dices.");
            Console.ReadLine();
            int dice1 = game.RollDice(true);
            int dice2 = game.RollDice(true);
            if (dice1 == dice2)
            {
                game.AddDiceToList(dice1);
                game.AddDiceToList(dice1);
            }
        }

        static void DecideFirstPlayer(BackgammonGame game)
        {
            bool tie = false;
            int result1 = 0;
            int result2 = 0;
            int startingPlayer = 0;
            do
            {
                result1 = RollDice(game, 0);
                result2 = RollDice(game, 1);
                tie = result1 == result2;
            }
            while (tie);
            startingPlayer = result1 > result2 ? 0 : 1;
            game.SetCurrentPlayer(startingPlayer);
        }

        static bool TryPerformGeneralTransferStep(BackgammonGame game, int srcTri, int destTri)
        {

            if (game.CanRetrievalBePerformed())
            {
                Console.WriteLine("You have captured checkers to retrive. Press any key and try again...");
                Console.ReadKey();
                return false;
            }
            if (game.IsLegalRegularTransferStep(srcTri, destTri))
            {
                Console.WriteLine("*********************Regular transfer step***********************");
                game.PerformRegularTransferStep(srcTri, destTri);
                return true;
            }
            else if (game.IsLegalHitStep(srcTri, destTri))
            {
                Console.WriteLine("*********************Hit step**********************************");
                game.PerformHitStep(srcTri, destTri);
                return true;
            }
            else
            {
                Console.WriteLine("Illegal transfer step. Press any key and try again...");
                Console.ReadKey();
                return false;
            }
        }

        static bool TryPerformRetrievalStep(BackgammonGame game, int destTri)
        {
            if (game.IsLegalRetrievalStep(destTri))
            {
                game.PerformRetrievalStep(destTri);
                return true;
            }
            else
            {
                Console.WriteLine("Illegal retrieval step. Press any key...");
                Console.ReadKey();
                return false;
            }
        }

        static bool TryPerformRemovalStep(BackgammonGame game, int srcTri)
        {
            if (game.CanRetrievalBePerformed())
            {
                Console.WriteLine("You have captured checkers to retrive. Press any key and try again...");
                Console.ReadKey();
                return false;
            }
            if (game.IsLegalRemovalStep(srcTri))
            {
                game.PerformRemovalStep(srcTri);
                return true;
            }
            else
            {
                Console.WriteLine("Illegal removal step. Press any key...");
                Console.ReadKey();
                return false;
            }
        }

        // action: 'o' for removal OR 'c' for retrieval
        static void TryPerformLegalStep(object obj, PlayerMovedEventArgs e) // BackgammonGame game, char action, int srcTri, int destTri
        {
            char action = e.action;
            int srcTri = e.srcTri;
            int destTri = e.destTri;
            BackgammonGame game = (BackgammonGame)obj;
            srcTri--;
            destTri--;
            // transfer step
            if (action == 't')
            {
                game.SetStepResult(TryPerformGeneralTransferStep(game, srcTri, destTri));
                return;
            }
            // retrieval step
            if (action == 'c')
            {
                game.SetStepResult(TryPerformRetrievalStep(game, destTri));
                return;
            }
            // removal step
            if (action == 'o')
            {
                game.SetStepResult(TryPerformRemovalStep(game, srcTri));
                return;
            }
            game.SetStepResult(false);
        }

        static void PerformStep(BackgammonGame game, int stepNum)
        {
            bool success = false;
            do
            {
                ShowGameStatus(game);
                ChangeConsoleColor(game, game.CurrentTurn);
                game.PlayersArray[game.CurrentTurn].PlayerMoved += TryPerformLegalStep;
                game.PlayersArray[game.CurrentTurn].ChooseStep(game);
                success = game.CurrentStepResult;
                game.PlayersArray[game.CurrentTurn].PlayerMoved -= TryPerformLegalStep;
            }
            while (!success);
        }

        static void PerformSteps(BackgammonGame game, int stepsNum)
        {
            for (int stepNum = 1; stepNum <= stepsNum; stepNum++)
            {
                if (game.CanPlayerPerformAnyStep())
                {
                    PerformStep(game, stepNum);
                }
                else
                {
                    ShowGameStatus(game);
                    Console.WriteLine("************** No more steps are possible. For next turn press any key... ********************");
                    Console.ReadKey();
                    break;
                }
                ShowGameStatus(game);
            }
        }

        static void PerformTwoSteps(BackgammonGame game)
        {
            PerformSteps(game, 2);
        }

        static void PerformFourSteps(BackgammonGame game)
        {
            PerformSteps(game, 4);
        }

        static void PerformMove(BackgammonGame game)
        {
            if (game.DicesAreDouble())
            {
                Console.WriteLine($"DOUBLE {game.DicesList[0]} !");
                PerformFourSteps(game);
            }
            else
            {
                PerformTwoSteps(game);
            }
        }

        static void SwitchTurn(BackgammonGame game)
        {
            game.SwitchTurn();
            ChangeConsoleColor(game, game.CurrentTurn); ;
        }

        static void PerformGame(BackgammonGame game)
        {
            while (!game.IsGameOver())
            {
                RollTwoDices(game);
                PerformMove(game);
                SwitchTurn(game);
                ShowGameStatus(game);
            }
        }

        static void ShowGameStatus(BackgammonGame game)
        {
            Console.Clear();
            PrintScore(game);
            PrintHits(game);
            PrintCheckersOnBoard(game);
            ShowBoardGame(game);
            PrintDicesList(game);
        }

        static void ShowBoardGame(BackgammonGame game)
        {
            Console.WriteLine("----------------------------------------------------");
            PrintBoardClassic(game, false, Constants.CheckerSymbol);
            Console.WriteLine("----------------------------------------------------");
        }

        static void PrintDicesList(BackgammonGame game)
        {
            if (game.DicesList.Count == 0)
            {
                return;
            }
            Console.Write("dices: { ");
            foreach (var dice in game.DicesList)
            {
                Console.Write(dice + " ");
            }
            Console.WriteLine("}");
        }

        static void ShowWhoStarts(BackgammonGame game, int playerNumber)
        {
            ChangeConsoleColor(game, game.CurrentTurn);
            Console.WriteLine($"Player { playerNumber + 1} starts.");
            Console.WriteLine("Press any key to start game...");
            Console.ReadKey();
        }

        static void ShowWinner(BackgammonGame game)
        {
            Console.WriteLine($"Player {game.GameWinner() + 1} won this round.");
        }

        static void ShowWinTypeAndUpdateScore(BackgammonGame game)
        {
            if (game.IsRegularWin())
            {
                game.IncrementWinnerScoreBy(1);
                Console.WriteLine("Regular win");
            }
            else if (game.IsMarsWin())
            {
                game.IncrementWinnerScoreBy(2);
                Console.WriteLine("Mars win");
            }
            else if (game.IsTurkishMarsWin())
            {
                game.IncrementWinnerScoreBy(3);
                Console.WriteLine("Turkish mars win");
            }
            else if (game.IsStarsMarsWin())
            {
                game.IncrementWinnerScoreBy(4);
                Console.WriteLine("Stars mars win");
            }
        }

        static bool InputNextRoundOrQuit()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.KeyChar == 'q' || key.KeyChar == 'Q')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool FinishGame(BackgammonGame game)
        {
            ShowWinner(game);
            ShowWinTypeAndUpdateScore(game);
            Console.WriteLine("Press 'q' key to exit the application OR any other key to continue to next round.");
            return (InputNextRoundOrQuit());
        }

        static void PrintMatrix(BackgammonGame game, char[,] mat, bool numFlag, char letter)
        {
            for (int i = 0; i < mat.GetLength(0); ++i)
            {
                for (int j = 0; j < mat.GetLength(1); ++j)
                {
                    if (mat[i, j] == '1')
                    {
                        Console.ForegroundColor = (ConsoleColor)game.PlayersArray[0].CheckerColor;
                        Console.Write("{0,4}", CheckerSymbol(mat[i, j], numFlag, letter));
                    }
                    else if (mat[i, j] == '2')
                    {
                        Console.ForegroundColor = (ConsoleColor)game.PlayersArray[1].CheckerColor;
                        Console.Write("{0,4}", CheckerSymbol(mat[i, j], numFlag, letter));
                    }
                    else if (mat[i, j] == '.')
                    {
                        Console.ForegroundColor = (ConsoleColor)game.PlayersArray[0].CheckerColor;
                        Console.Write("{0,4}", mat[i, j]);
                    }
                    else if (mat[i, j] == ',')
                    {
                        Console.ForegroundColor = (ConsoleColor)game.PlayersArray[1].CheckerColor;
                        Console.Write("{0,4}", '.');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("{0,4}", mat[i, j]);
                    }
                    if (j == 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("{0,4}", '|');
                    }
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void PrintScore(BackgammonGame game)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            string name1 = game.PlayersArray[0].PlayerName;
            string name2 = game.PlayersArray[1].PlayerName;
            int score1 = game.PlayersArray[0].Score;
            int score2 = game.PlayersArray[1].Score;
            Console.WriteLine($"\t{name1}\t\t{score1}\t:\t{score2}\t\t{name2}\n");
        }

        public static void PrintHits(BackgammonGame game)
        {
            Console.ForegroundColor = game.PlayersArray[0].CheckersHit > 0 ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.Write($"\tHits: {game.PlayersArray[0].CheckersHit}\t\t\t");
            Console.ForegroundColor = game.PlayersArray[1].CheckersHit > 0 ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.WriteLine($"\t\t\t\tHits: {game.PlayersArray[1].CheckersHit}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void PrintCheckersOnBoard(BackgammonGame game)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"\ton board: {game.PlayersArray[0].CheckersOnBoard}\t\t\t");
            Console.WriteLine($"\t\t\ton board: {game.PlayersArray[1].CheckersOnBoard}");
        }

        public static void PrintBoardClassic(BackgammonGame game, bool numFlag, char letter)
        {
            char[,] mat = CreateBoardMatrix(game);
            PrintIndexes(13, 24);
            PrintMatrix(game, mat, numFlag, letter);
            PrintIndexes(12, 1);
        }

        private static void PrintIndexes(int startIndex, int endIndex)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            int i = 0;
            int k = 0;
            if (startIndex < endIndex)
            {
                for (k = 1, i = startIndex; i <= endIndex; ++i, k++)
                {
                    if (k == (endIndex - startIndex) / 2 + 2)
                    {
                        Console.Write("{0,4}", "");
                    }
                    Console.Write("{0,4}", i);
                }
            }
            else
            {
                for (k = 1, i = startIndex; i >= endIndex; --i, k++)
                {
                    if (k == (startIndex - endIndex) / 2 + 2)
                    {
                        Console.Write("{0,4}", "");
                    }
                    Console.Write("{0,4}", i);
                }
            }
            Console.WriteLine();
        }

        static char CheckerSymbol(char playerNumberChar, bool numFlag, char letter)
        {
            if (numFlag)
            {
                return playerNumberChar;
            }
            return letter;
        }

        static char[,] CreateBoardMatrix(BackgammonGame game)
        {
            int boardWidth = game.TrianglesNumber / 2;
            int triangleHeight = Constants.MaxCheckersInTriangle;
            char[,] mat = new char[triangleHeight * 2 + 3, boardWidth * 2]; // [13,12]
            int trianglesNum = game.TrianglesNumber;
            int triIndex = 0;
            int i = 0, k = 0;

            for (triIndex = 0; triIndex < trianglesNum; ++triIndex)
            {
                int playerNumber = game.Board[triIndex].PlayerNumber;
                int checkerNumber = game.Board[triIndex].CheckersNumber;
                // triangles 1-12
                if (triIndex < boardWidth)
                {
                    for (i = 1, k = mat.GetLength(0) - 1; i <= Math.Min(checkerNumber, triangleHeight); --k, ++i)
                    {
                        mat[k, boardWidth - triIndex - 1] = (char)((playerNumber + 1) + '0');
                    }
                    if (checkerNumber > triangleHeight)
                    {
                        if (playerNumber == 0)
                        {
                            mat[k, boardWidth - triIndex - 1] = '.';
                        }
                        else
                        {
                            mat[k, boardWidth - triIndex - 1] = ',';
                        }
                    }
                }
                // triangles 13-24
                else
                {
                    for (i = 1, k = 0; i <= Math.Min(checkerNumber, triangleHeight); ++k, ++i)
                    {
                        mat[k, triIndex - boardWidth] = (char)((playerNumber + 1) + '0');
                    }

                    if (checkerNumber > triangleHeight)
                    {
                        if (playerNumber == 0)
                        {
                            mat[k, triIndex - boardWidth] = '.';
                        }
                        else
                        {
                            mat[k, triIndex - boardWidth] = ',';
                        }
                    }
                }
            }
            return mat;
        }

        static void Main(string[] args)
        {
            bool isQuit = false;
            bool isFirstRound = true;
            BackgammonGame game = null;
            int score1 = 0, score2 = 0;
            while (!isQuit)
            {
                game = InitializeGame(isFirstRound, score1, score2);
                ShowGameStatus(game);
                DecideFirstPlayer(game);
                ShowWhoStarts(game, game.CurrentTurn);
                ShowGameStatus(game);
                PerformGame(game);
                isQuit = FinishGame(game);
                score1 = game.PlayersArray[0].Score;
                score2 = game.PlayersArray[1].Score;
                isFirstRound = false;
            }
            Console.WriteLine("Game over.");
        }
    }
}