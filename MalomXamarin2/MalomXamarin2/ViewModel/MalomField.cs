using MalomXamarin2.Model;

namespace MalomXamarin2.ViewModel
{
    public class MalomField : ViewModelBase
    {
        private Player _player;

        public int PosX { get; set; }

        public int PosY { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Number { get; set; }

        public Player Player
        {
            get { return _player; }
            set
            {
                if (_player != value)
                {
                    _player = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand StepCommand { get; set; }
    }
}

