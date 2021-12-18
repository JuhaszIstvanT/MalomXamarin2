using System;
using System.Collections.ObjectModel;
using MalomXamarin2.Model;

namespace MalomXamarin2.ViewModel
{
    public class MalomViewModel : ViewModelBase
    {
        #region Fields
        private MalomModel _model;
        private int prevX;
        private int prevY;
        private int currentX;
        private int currentY;
        private bool next;
        #endregion

        #region Properties
        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<MalomField> Fields { get; set; }

        public int GameStepCount { get { return _model.stepNumber; } }
        #endregion

        #region Events
        public event EventHandler NewGame;

        public event EventHandler LoadGame;

        public event EventHandler SaveGame;

        public event EventHandler ExitGame;
        #endregion

        #region Constructors
        public MalomViewModel(MalomModel model)
        {
            _model = model;
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);
            _model.GameWon += new EventHandler<GameWonEventArgs>(Model_GameWon);

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            Fields = new ObservableCollection<MalomField>();
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    Fields.Add(new MalomField
                    {
                        X = i,
                        Y = j,
                        Number = i * 8 + j,
                        Player = Player.None,
                        PosX = calculatePosition(i, j)[0],
                        PosY = calculatePosition(i, j)[1],
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            RefreshBoard();


        }
        #endregion

        #region Private methods
        public void RefreshBoard()
        {
            foreach (MalomField field in Fields)
            {
                field.Player = _model.board[field.X, field.Y];
            }
        }

        private void StepGame(int index)
        {
            MalomField field = Fields[index];
            int x = field.X;
            int y = field.Y;

            if (_model.firstStage || (_model.stepNumber == 18 && _model.board.prevRedMalom < _model.board.currentRedMalom))
            {
                _model.StepGame(x, y);
            }
            else
            {
                if (_model.board[x, y] == _model.currentPlayer && !_model.canCapture)
                {
                    next = true;
                    currentX = x;
                    currentY = y;
                    return;
                }
                if (next && _model.board[x, y] == Player.None)
                {
                    prevX = currentX;
                    prevY = currentY;
                    currentX = x;
                    currentY = y;
                    next = false;
                    _model.StepGame(prevX, prevY, currentX, currentY);
                    return;
                }

                if (_model.board[x, y] == _model.currentPlayer && _model.canCapture)
                {
                    if (_model.board[x, y] == Player.Red && !_model.board.CheckMalom(x, y, Player.Red))
                    {
                        _model.RemovePiece(x, y);
                        _model.canCapture = false;
                        return;
                    }
                    else if (_model.board[x, y] == Player.Red && _model.board.AllInMalom(Player.Red))
                    {
                        _model.RemovePiece(x, y);
                        _model.canCapture = false;
                        return;
                    }
                    else if (_model.board[x, y] == Player.Blue && !_model.board.CheckMalom(x, y, Player.Blue))
                    {
                        _model.RemovePiece(x, y);
                        _model.canCapture = false;
                        return;
                    }
                    else if (_model.board[x, y] == Player.Blue && _model.board.AllInMalom(Player.Blue))
                    {
                        _model.RemovePiece(x, y);
                        _model.canCapture = false;
                        return;
                    }
                }
            }
            field.Player = _model.board[x, y];
            RefreshBoard();
            OnPropertyChanged();
        }

        private int[] calculatePosition(int xC, int yC)
        {
            int x, y;
            int[] point = new int[2];
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (j < 4)
                    {
                        x = 100 * (i + 1) + (j % 3) * (300 - i * 100);
                        y = 100 * (i + 1) + (j / 3) * (300 - i * 100);
                    }
                    else
                    {
                        x = 100 * (i + 1) + ((j + 1) % 3) * (300 - i * 100);
                        y = 100 * (i + 1) + ((j + 1) / 3) * (300 - i * 100);
                    }
                    if (i == xC && j == yC)
                    {
                        point[0] = x + 8;
                        point[1] = y - 12;
                        return point;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Game event handlers
        private void Model_GameWon(object sender, GameWonEventArgs e)
        {
            _model.NewGame();
        }

        private void Model_FieldChanged(object sender, FieldChangedEventArgs e)
        {
            RefreshBoard();
        }
        #endregion

        #region Event methods
        private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
            RefreshBoard();
        }

        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
            RefreshBoard();
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }
        #endregion
    }
}

