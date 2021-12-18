using System;
using MalomXamarin2.Persistence;

namespace MalomXamarin2.Model
{
    public class MalomModel
    {
        #region Fields

        public Player currentPlayer;
        public int stepNumber;
        public int bluePiece;
        public int redPiece;
        public bool firstStage;
        public bool canCapture;
        public MalomBoard board;
        private IMalomDataAccess dataAccess;

        #endregion

        #region Events


        public event EventHandler<GameWonEventArgs> GameWon;

        public event EventHandler<FieldChangedEventArgs> FieldChanged;


        #endregion

        #region Constructor

        public MalomModel(IMalomDataAccess persistence)
        {
            dataAccess = persistence;
            board = new MalomBoard();
        }

        #endregion

        #region Methods
        public void NewGame()
        {
            board = new MalomBoard();
            stepNumber = 0;
            bluePiece = 0;
            redPiece = 0;
            firstStage = true;
            currentPlayer = Player.Blue;
            board.steps = 0;
        }

        public void StepGame(int x, int y)
        {
            if (x < 0 || x >= 3)
                throw new ArgumentException("Bad row index.", "x");
            if (y < 0 || y >= 8)
                throw new ArgumentException("Bad column index.", "y");

            Player notCurrentPlayer = currentPlayer == Player.Blue ? Player.Red : Player.Blue;
            if (board[x, y] == Player.None && (board.prevBlueMalom < board.currentBlueMalom || board.prevRedMalom < board.currentRedMalom))
                return;
            if (board[x, y] != Player.None && board[x, y] != notCurrentPlayer)
            {
                if (board[x, y] == Player.Red && !board.CheckMalom(x, y, Player.Red) && board.prevBlueMalom < board.currentBlueMalom)
                {
                    RemovePiece(x, y);
                    return;
                }
                else if (board[x, y] == Player.Red && board.AllInMalom(Player.Red) && board.prevBlueMalom < board.currentBlueMalom)
                {
                    RemovePiece(x, y);
                    return;
                }
                else if (board[x, y] == Player.Blue && !board.CheckMalom(x, y, Player.Blue) && board.prevRedMalom < board.currentRedMalom)
                {
                    RemovePiece(x, y);
                    return;
                }
                else if (board[x, y] == Player.Blue && board.AllInMalom(Player.Blue) && board.prevRedMalom < board.currentRedMalom)
                {
                    RemovePiece(x, y);
                    return;
                }
                else
                {
                    return;
                }

            }
            else if (board[x, y] == Player.None)
            {
                board[x, y] = currentPlayer;
                OnFieldChanged(x, y, currentPlayer);
                if (currentPlayer == Player.Blue)
                    ++bluePiece;
                else
                    ++redPiece;
                ++stepNumber;
                ++board.steps;
            }
            else
            {
                return;
            }

            board.CheckMalom();

            currentPlayer = currentPlayer == Player.Blue ? Player.Red : Player.Blue;

            if (stepNumber >= 18)
                firstStage = false;
        }

        public void StepGame(int fromX, int fromY, int toX, int toY)
        {
            ++stepNumber;
            ++board.steps;
            if (fromX < 0 || fromX >= 3 || toX < 0 || toX >= 3)
                throw new ArgumentException("Bad row index.", "x");
            if (fromY < 0 || fromY >= 8 || toY < 0 || toY >= 8)
                throw new ArgumentException("Bad column index.", "y");

            if (!board.CanMove(currentPlayer))
            {
                currentPlayer = currentPlayer == Player.Blue ? Player.Red : Player.Blue;
            }

            if (board.IsValidMove(fromX, fromY, toX, toY, currentPlayer))
            {
                board[fromX, fromY] = Player.None;
                board[toX, toY] = currentPlayer;
                OnFieldChanged(fromX, fromY, Player.None);
                OnFieldChanged(toX, toY, currentPlayer);
                if (board.CheckMalom(toX, toY, currentPlayer))
                    canCapture = true;
            }
            else
            {
                return;
            }

            currentPlayer = currentPlayer == Player.Blue ? Player.Red : Player.Blue;

            CheckWin();
        }


        public void RemovePiece(int x, int y)
        {
            if (board[x, y] == Player.Blue)
                --bluePiece;
            else
                --redPiece;

            board[x, y] = Player.None;
            OnFieldChanged(x, y, Player.None);
            board.CheckMalom();
            CheckWin();
        }

        public void CheckWin()
        {
            if (bluePiece < 3 && !firstStage)
                OnGameWon(Player.Red);
            else if (redPiece < 3 && !firstStage)
                OnGameWon(Player.Blue);
        }

        #endregion

        #region Persistence
        public void SaveGame(String path)
        {
            if (dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            dataAccess.Save(path, board);
        }

        public void LoadGame(String path)
        {
            if (dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            board = dataAccess.Load(path);

            int blue = 0;
            int red = 0;
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 8; ++j)
                {
                    if (board[i, j] == Player.Blue)
                        ++blue;
                    else if (board[i, j] == Player.Red)
                        ++red;
                }

            int bluep = 0;
            int bluec = 0;
            int redp = 0;
            int redc = 0;
            for (int i = 0; i < 16; ++i)
            {
                if (board.prevMalom[i] == Player.Blue) ++bluep;
                else if (board.prevMalom[i] == Player.Red) ++redp;
                if (board.currentMalom[i] == Player.Blue) ++bluec;
                else if (board.currentMalom[i] == Player.Red) ++redc;
            }

            board.prevBlueMalom = bluep;
            board.currentBlueMalom = bluec;
            board.prevRedMalom = redp;
            board.currentRedMalom = redc;
            bluePiece = blue;
            redPiece = red;
            stepNumber = board.steps;
            currentPlayer = stepNumber % 2 == 0 ? Player.Blue : Player.Red;
            firstStage = stepNumber < 18;

            CheckWin();
        }

        #endregion


        #region Event triggers

        private void OnGameWon(Player player)
        {
            if (GameWon != null)
                GameWon(this, new GameWonEventArgs(player));
        }

        private void OnFieldChanged(int x, int y, Player player)
        {
            if (FieldChanged != null)
                FieldChanged(this, new FieldChangedEventArgs(x, y, player));
        }

        #endregion
    }
}
