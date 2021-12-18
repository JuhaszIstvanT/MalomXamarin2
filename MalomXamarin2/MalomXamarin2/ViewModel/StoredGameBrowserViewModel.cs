using System;
using System.Collections.ObjectModel;
using MalomXamarin2.Model;

namespace MalomXamarin2.ViewModel
{
    public class StoredGameBrowserViewModel : ViewModelBase
    {
        private StoredGameBrowserModel _model;

        public event EventHandler<StoredGameEventArgs> GameLoading;
        public event EventHandler<StoredGameEventArgs> GameSaving;

        public StoredGameBrowserViewModel(StoredGameBrowserModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            _model.StoreChanged += new EventHandler(Model_StoreChanged);

            NewSaveCommand = new DelegateCommand(param => OnGameSaving((string)param));
            StoredGames = new ObservableCollection<StoredGameViewModel>();
            UpdateStoredGames();
        }

        public DelegateCommand NewSaveCommand { get; private set; }

        public ObservableCollection<StoredGameViewModel> StoredGames { get; private set; }

        private void UpdateStoredGames()
        {
            StoredGames.Clear();

            foreach (StoredGameModel item in _model.StoredGames)
            {
                StoredGames.Add(new StoredGameViewModel
                {
                    Name = item.Name,
                    Modified = item.Modified,
                    LoadGameCommand = new DelegateCommand(param => OnGameLoading((string)param)),
                    SaveGameCommand = new DelegateCommand(param => OnGameSaving((string)param))
                });
            }
        }

        private void Model_StoreChanged(object sender, EventArgs e)
        {
            UpdateStoredGames();
        }

        private void OnGameLoading(string name)
        {
            if (GameLoading != null)
                GameLoading(this, new StoredGameEventArgs { Name = name });
        }

        private void OnGameSaving(string name)
        {
            if (GameSaving != null)
                GameSaving(this, new StoredGameEventArgs { Name = name });
        }
    }
}

