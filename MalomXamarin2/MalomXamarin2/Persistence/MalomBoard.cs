using System;
using MalomXamarin2.Model;

namespace MalomXamarin2.Persistence
{
    public class MalomBoard
    {
        #region Fields

        public Player[,] board;
        public Player[] prevMalom;
        public Player[] currentMalom;
        public int prevBlueMalom;
        public int prevRedMalom;
        public int currentBlueMalom;
        public int currentRedMalom;
        public int steps;

        #endregion

        #region Constructos

        public MalomBoard()
        {
            prevBlueMalom = 0;
            prevRedMalom = 0;
            currentBlueMalom = 0;
            currentRedMalom = 0;
            board = new Player[3, 8];
            prevMalom = new Player[16];
            currentMalom = new Player[16];

            for (int i = 0; i < board.GetLength(0); ++i)
                for (int j = 0; j < board.GetLength(1); ++j)
                    board[i, j] = Player.None;

            for (int i = 0; i < 16; ++i)
            {
                prevMalom[i] = Player.None;
                currentMalom[i] = Player.None;
            }

        }

        #endregion

        #region Properties

        public Player this[int x, int y] { get { return GetValue(x, y); } set { board[x, y] = value; } }

        #endregion

        #region Public methods

        public bool AllInMalom(Player p)
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (board[i, j] == p)
                    {
                        if (!CheckMalom(i, j, p))
                            return false;
                    }
                }
            }
            return true;
        }
        public bool CheckMalom(int x, int y, Player p)
        {
            for (int i = 0; i < board.GetLength(0); ++i)
            {
                if (board[i, 0] == p && board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2] && x == i && y < 3)
                {
                    return true;
                }

                if (board[i, 5] == p && board[i, 5] == board[i, 6] && board[i, 5] == board[i, 7] && x == i && y > 4)
                {
                    return true;
                }

                if (board[i, 0] == p && board[i, 0] == board[i, 3] && board[i, 0] == board[i, 5] && x == i && (y == 0 || y == 3 || y == 5))
                {
                    return true;
                }

                if (board[i, 2] == p && board[i, 2] == board[i, 4] && board[i, 2] == board[i, 7] && x == i && (y == 2 || y == 4 || y == 7))
                {
                    return true;
                }
            }

            if (board[0, 3] == p && board[0, 3] == board[1, 3] && board[0, 3] == board[2, 3] && (x == 0 || x == 1 || x == 2) && y == 3)
            {
                return true;
            }

            if (board[0, 4] == p && board[0, 4] == board[1, 4] && board[0, 4] == board[2, 4] && (x == 0 || x == 1 || x == 2) && y == 4)
            {
                return true;
            }

            if (board[0, 1] == p && board[0, 1] == board[1, 1] && board[0, 1] == board[2, 1] && (x == 0 || x == 1 || x == 2) && y == 1)
            {
                return true;
            }

            if (board[0, 6] == p && board[0, 6] == board[1, 6] && board[0, 6] == board[2, 6] && (x == 0 || x == 1 || x == 2) && y == 6)
            {
                return true;
            }

            return false;
        }
        public void CheckMalom()
        {
            Array.Copy(currentMalom, prevMalom, prevMalom.Length);

            for (int i = 0; i < board.GetLength(0); ++i)
            {
                if (board[i, 0] != Player.None && board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2])
                {
                    currentMalom[i] = board[i, 0];
                }

                if (board[i, 5] != Player.None && board[i, 5] == board[i, 6] && board[i, 5] == board[i, 7])
                {
                    currentMalom[i + 3] = board[i, 5];
                }

                if (board[i, 0] != Player.None && board[i, 0] == board[i, 3] && board[i, 0] == board[i, 5])
                {
                    currentMalom[i + 6] = board[i, 0];
                }

                if (board[i, 2] != Player.None && board[i, 2] == board[i, 4] && board[i, 2] == board[i, 7])
                {
                    currentMalom[i + 9] = board[i, 2];
                }
            }

            if (board[0, 3] != Player.None && board[0, 3] == board[1, 3] && board[0, 3] == board[2, 3])
            {
                currentMalom[12] = board[0, 3];
            }

            if (board[0, 4] != Player.None && board[0, 4] == board[1, 4] && board[0, 4] == board[2, 4])
            {
                currentMalom[13] = board[0, 4];
            }

            if (board[0, 1] != Player.None && board[0, 1] == board[1, 1] && board[0, 1] == board[2, 1])
            {
                currentMalom[14] = board[0, 1];
            }

            if (board[0, 6] != Player.None && board[0, 6] == board[1, 6] && board[0, 6] == board[2, 6])
            {
                currentMalom[15] = board[0, 6];
            }
            prevBlueMalom = CountPrevMalom(Player.Blue);
            prevRedMalom = CountPrevMalom(Player.Red);
            currentBlueMalom = CountCurrentMalom(Player.Blue);
            currentRedMalom = CountCurrentMalom(Player.Red);
        }
        public bool CanMove(Player p)
        {
            for (int i = 0; i < board.GetLength(0); ++i)
            {
                for (int j = 0; j < board.GetLength(1); ++j)
                {
                    if (board[i, j] == p)
                    {
                        switch (i)
                        {
                            case 0:
                                switch (j)
                                {
                                    case 0:
                                        if (board[0, 1] == Player.None || board[0, 3] == Player.None) return true;
                                        break;
                                    case 1:
                                        if (board[0, 0] == Player.None || board[0, 2] == Player.None || board[1, 1] == Player.None) return true;
                                        break;
                                    case 2:
                                        if (board[0, 1] == Player.None || board[0, 4] == Player.None) return true;
                                        break;
                                    case 3:
                                        if (board[0, 0] == Player.None || board[0, 5] == Player.None || board[1, 3] == Player.None) return true;
                                        break;
                                    case 4:
                                        if (board[0, 2] == Player.None || board[0, 7] == Player.None || board[1, 4] == Player.None) return true;
                                        break;
                                    case 5:
                                        if (board[0, 3] == Player.None || board[0, 6] == Player.None) return true;
                                        break;
                                    case 6:
                                        if (board[0, 5] == Player.None || board[0, 7] == Player.None || board[1, 6] == Player.None) return true;
                                        break;
                                    case 7:
                                        if (board[0, 4] == Player.None || board[0, 6] == Player.None) return true;
                                        break;
                                }
                                break;
                            case 1:
                                switch (j)
                                {
                                    case 0:
                                        if (board[1, 1] == Player.None || board[1, 3] == Player.None) return true;
                                        break;
                                    case 1:
                                        if (board[1, 0] == Player.None || board[1, 2] == Player.None || board[0, 1] == Player.None || board[2, 1] == Player.None) return true;
                                        break;
                                    case 2:
                                        if (board[1, 1] == Player.None || board[1, 4] == Player.None) return true;
                                        break;
                                    case 3:
                                        if (board[0, 3] == Player.None || board[2, 3] == Player.None || board[1, 0] == Player.None || board[1, 5] == Player.None) return true;
                                        break;
                                    case 4:
                                        if (board[1, 2] == Player.None || board[1, 7] == Player.None || board[0, 4] == Player.None || board[2, 4] == Player.None) return true;
                                        break;
                                    case 5:
                                        if (board[1, 3] == Player.None || board[1, 6] == Player.None) return true;
                                        break;
                                    case 6:
                                        if (board[1, 5] == Player.None || board[1, 7] == Player.None || board[0, 6] == Player.None || board[2, 6] == Player.None) return true;
                                        break;
                                    case 7:
                                        if (board[1, 6] == Player.None || board[1, 4] == Player.None) return true;
                                        break;
                                }
                                break;
                            case 2:
                                switch (j)
                                {
                                    case 0:
                                        if (board[2, 1] == Player.None || board[2, 3] == Player.None) return true;
                                        break;
                                    case 1:
                                        if (board[2, 0] == Player.None || board[2, 2] == Player.None || board[1, 1] == Player.None) return true;
                                        break;
                                    case 2:
                                        if (board[2, 1] == Player.None || board[2, 4] == Player.None) return true;
                                        break;
                                    case 3:
                                        if (board[2, 0] == Player.None || board[2, 5] == Player.None || board[1, 3] == Player.None) return true;
                                        break;
                                    case 4:
                                        if (board[2, 2] == Player.None || board[2, 7] == Player.None || board[1, 4] == Player.None) return true;
                                        break;
                                    case 5:
                                        if (board[2, 3] == Player.None || board[2, 6] == Player.None) return true;
                                        break;
                                    case 6:
                                        if (board[2, 5] == Player.None || board[2, 7] == Player.None || board[1, 6] == Player.None) return true;
                                        break;
                                    case 7:
                                        if (board[2, 6] == Player.None || board[2, 4] == Player.None) return true;
                                        break;
                                }
                                break;
                        }
                    }

                }
            }
            return false;
        }
        public bool IsValidMove(int fromX, int fromY, int toX, int toY, Player p)
        {
            bool l = board[fromX, fromY] == p && board[toX, toY] == Player.None;
            if (!l)
                return false;

            switch (fromX)
            {
                case 0:
                    switch (fromY)
                    {
                        case 0:
                            return l && ((toX == 0 && toY == 1) || (toX == 0 && toY == 3));
                        case 1:
                            return l && ((toX == 0 && toY == 0) || (toX == 0 && toY == 2) || (toX == 1 && toY == 1));
                        case 2:
                            return l && ((toX == 0 && toY == 1) || (toX == 0 && toY == 4));
                        case 3:
                            return l && ((toX == 0 && toY == 0) || (toX == 0 && toY == 5) || (toX == 1 && toY == 3));
                        case 4:
                            return l && ((toX == 0 && toY == 2) || (toX == 0 && toY == 7) || (toX == 1 && toY == 4));
                        case 5:
                            return l && ((toX == 0 && toY == 3) || (toX == 0 && toY == 6));
                        case 6:
                            return l && ((toX == 0 && toY == 5) || (toX == 0 && toY == 7) || (toX == 1 && toY == 6));
                        case 7:
                            return l && ((toX == 0 && toY == 4) || (toX == 0 && toY == 6));
                    }
                    break;
                case 1:
                    switch (fromY)
                    {
                        case 0:
                            return l && ((toX == 1 && toY == 1) || (toX == 1 && toY == 3));
                        case 1:
                            return l && ((toX == 1 && toY == 0) || (toX == 1 && toY == 2) || (toX == 0 && toY == 1) || (toX == 2 && toY == 1));
                        case 2:
                            return l && ((toX == 1 && toY == 1) || (toX == 1 && toY == 4));
                        case 3:
                            return l && ((toX == 0 && toY == 3) || (toX == 2 && toY == 3) || (toX == 1 && toY == 0) || (toX == 1 && toY == 5));
                        case 4:
                            return l && ((toX == 1 && toY == 2) || (toX == 1 && toY == 7) || (toX == 0 && toY == 4) || (toX == 2 && toY == 4));
                        case 5:
                            return l && ((toX == 1 && toY == 3) || (toX == 1 && toY == 6));
                        case 6:
                            return l && ((toX == 1 && toY == 5) || (toX == 1 && toY == 7) || (toX == 0 && toY == 6) || (toX == 2 && toY == 6));
                        case 7:
                            return l && ((toX == 1 && toY == 6) || (toX == 1 && toY == 4));
                    }
                    break;
                case 2:
                    switch (fromY)
                    {
                        case 0:
                            return l && ((toX == 2 && toY == 1) || (toX == 2 && toY == 3));
                        case 1:
                            return l && ((toX == 2 && toY == 0) || (toX == 2 && toY == 2) || (toX == 1 && toY == 1));
                        case 2:
                            return l && ((toX == 2 && toY == 1) || (toX == 2 && toY == 4));
                        case 3:
                            return l && ((toX == 2 && toY == 0) || (toX == 2 && toY == 5) || (toX == 1 && toY == 3));
                        case 4:
                            return l && ((toX == 2 && toY == 2) || (toX == 2 && toY == 7) || (toX == 1 && toY == 4));
                        case 5:
                            return l && ((toX == 2 && toY == 3) || (toX == 2 && toY == 6));
                        case 6:
                            return l && ((toX == 2 && toY == 5) || (toX == 2 && toY == 7) || (toX == 1 && toY == 6));
                        case 7:
                            return l && ((toX == 2 && toY == 6) || (toX == 2 && toY == 4));
                    }
                    break;
            }
            return false;
        }

        #endregion

        #region Private methods
        private Player GetValue(int x, int y)
        {
            if (x < 0 || x >= board.GetLength(0))
                throw new ArgumentException("Bad row index.", "x");
            if (y < 0 || y >= board.GetLength(1))
                throw new ArgumentException("Bad column index.", "y");

            return board[x, y];
        }

        private int CountPrevMalom(Player p)
        {
            int counter = 0;
            for (int i = 0; i < 16; ++i)
            {
                if (prevMalom[i] == p)
                {
                    ++counter;
                }
            }
            return counter;
        }

        private int CountCurrentMalom(Player p)
        {
            int counter = 0;
            for (int i = 0; i < 16; ++i)
            {
                if (currentMalom[i] == p)
                {
                    ++counter;
                }
            }
            return counter;
        }

        #endregion
    }
}

