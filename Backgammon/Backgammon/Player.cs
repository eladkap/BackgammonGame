using System;

namespace Backgammon
{
    public abstract class Player : IPlayer
    {
        public event EventHandler<PlayerMovedEventArgs> PlayerMoved;

        /// <summary>
        /// Player name.
        /// </summary>
        protected string _playerName;

        /// <summary>
        /// Player number 0 or 1.
        /// </summary>
        protected int _playerNumber;

        /// <summary>
        /// Checker color.
        /// </summary>
        protected CheckerColor _checkerColor;

        /// <summary>
        /// NUmber of checkers on board.
        /// </summary>
        protected int _checkersOnBoard;

        /// <summary>
        /// Number of captured (hit) checkers.
        /// </summary>
        protected int _checkersHit;

        /// <summary>
        /// Player's current score. The score is updated in every end of round.
        /// </summary>
        protected int _score;

        /// <summary>
        /// Reference to game object;
        /// </summary>
        internal BackgammonGame _game;

        public Player(string playerName, int playerNumber, CheckerColor checkerColor, int checkersNumber, int score)
        {
            SetProperties(playerName, playerNumber, checkerColor, checkersNumber, score);
        }

        public Player() : this("Player", 0, CheckerColor.White, 15, 0)
        {
        }

        protected virtual void OnPlayerMoved(PlayerMovedEventArgs e)
        {
            if (PlayerMoved != null)
            {
                PlayerMoved(this.Game, e);
            }
        }

        public static bool IsLegalPlayerNumber(int playerNumber)
        {
            return playerNumber >= -1 && playerNumber <= 1;
        }

        public string PlayerName { get { return _playerName; } }

        public int PlayerNumber { get { return _playerNumber; } }

        public CheckerColor CheckerColor { get { return _checkerColor; } }

        public int CheckersOnBoard { get { return _checkersOnBoard; } }

        public BackgammonGame Game { get { return _game; } }

        public void SetCheckersOnBoard(int checkersNum)
        {
            _checkersOnBoard = checkersNum;
        }

        public int CheckersHit { get { return _checkersHit; } }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Execption in method Player.set(score): score is negative.");
                }
                _score = value;
            }
        }

        public void SetProperties(string playerName, int playerNumber, CheckerColor checkerColor, int checkersNumber, int score)
        {
            _playerName = playerName;
            _playerNumber = playerNumber;
            _checkerColor = checkerColor;
            _checkersOnBoard = checkersNumber;
            _checkersHit = 0;
            _score = score;
        }

        public void SetCheckerHit()
        {
            if (CheckersOnBoard == 0)
            {
                throw new Exception("Execption in method Player.SetCheckerHit(): CheckersOnBoard is already zero.");
            }
            _checkersOnBoard--;
            _checkersHit++;
        }

        public void SetCheckerBack()
        {
            if (CheckersHit == 0)
            {
                throw new Exception("Execption in method Player.SetCheckerBack(): CheckersHit is already zero.");
            }
            _checkersOnBoard++;
            _checkersHit--;
        }

        public void RemoveChecker()
        {
            if (CheckersOnBoard == 0)
            {
                throw new Exception("Execption in method Player.SetCheckerHit(): CheckersOnBoard is already zero.");
            }
            _checkersOnBoard--;
        }

        public abstract void ChooseStep(object obj);

        /// <summary>
        /// Returns the number of the rival player.
        /// </summary>
        /// <param name="playerNumber">player number</param>
        /// <returns>Number of the rival player</returns>
        public static int RivalPlayer(int playerNumber)
        {
            return 1 - playerNumber;
        }
    }
}
