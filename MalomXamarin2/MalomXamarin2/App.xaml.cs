using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MalomXamarin2.Model;
using MalomXamarin2.Persistence;
using MalomXamarin2.View;
using MalomXamarin2.ViewModel;

namespace MalomXamarin2
{
    public partial class App : Application
    {
        #region Fields
        private IMalomDataAccess _malomDataAccess;
        private MalomModel _malomModel;
        private MalomViewModel _malomViewModel;
        private GamePage _gamePage;
        private SettingsPage _settingsPage;

        private IStore _store;
        private StoredGameBrowserModel _storedGameBrowserModel;
        private StoredGameBrowserViewModel _storedGameBrowserViewModel;
        private LoadGamePage _loadGamePage;
        private SaveGamePage _saveGamePage;

        private NavigationPage _mainPage;
        #endregion

        #region Application methods

        public App()
        {
            _malomDataAccess = DependencyService.Get<IMalomDataAccess>();

            _malomModel = new MalomModel(_malomDataAccess);
            _malomModel.GameWon += new EventHandler<GameWonEventArgs>(MalomModel_GameWon);

            _malomViewModel = new MalomViewModel(_malomModel);
            _malomViewModel.NewGame += new EventHandler(MalomViewModel_NewGame);
            _malomViewModel.LoadGame += new EventHandler(MalomViewModel_LoadGame);
            _malomViewModel.SaveGame += new EventHandler(MalomViewModel_SaveGame);
            _malomViewModel.ExitGame += new EventHandler(MalomViewModel_ExitGame);

            _gamePage = new GamePage();
            _gamePage.BindingContext = _malomViewModel;

            _store = DependencyService.Get<IStore>();
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameLoading);
            _storedGameBrowserViewModel.GameSaving += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameSaving);

            _loadGamePage = new LoadGamePage();
            _loadGamePage.BindingContext = _storedGameBrowserViewModel;

            _saveGamePage = new SaveGamePage();
            _saveGamePage.BindingContext = _storedGameBrowserViewModel;

            _mainPage = new NavigationPage(_gamePage);

            MainPage = _mainPage;
        }

        protected override void OnStart()
        {
            _malomModel.NewGame();
            _malomViewModel.RefreshBoard();

        }

        protected override void OnSleep()
        {
            _malomModel.SaveGame("SuspendedGame");
        }

        protected override void OnResume()
        {
            _malomModel.LoadGame("SuspendedGame");
            _malomViewModel.RefreshBoard();
        }

        #endregion

        #region ViewModel event handlers
        private void MalomViewModel_NewGame(object sender, EventArgs e)
        {
            _malomModel.NewGame();
        }

        private void MalomViewModel_LoadGame(object sender, System.EventArgs e)
        {
            _storedGameBrowserModel.Update();
            _mainPage.PushAsync(_loadGamePage);
        }

        private void MalomViewModel_SaveGame(object sender, EventArgs e)
        {
            _storedGameBrowserModel.Update();
            _mainPage.PushAsync(_saveGamePage);
        }

        private void MalomViewModel_ExitGame(object sender, EventArgs e)
        {
            _mainPage.PushAsync(_settingsPage);
        }

        private void StoredGameBrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            _mainPage.PopAsync();

            try
            {
                _malomModel.LoadGame(e.Name);
                _malomViewModel.RefreshBoard();
            }
            catch
            {
                MainPage.DisplayAlert("Malom", "Sikertelen betöltés.", "OK");
            }
        }

        private void StoredGameBrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            _mainPage.PopAsync();

            try
            {
                _malomModel.SaveGame(e.Name);
            }
            catch { }

            MainPage.DisplayAlert("Malom", "Sikeres mentés.", "OK");
        }
        #endregion

        #region Model event handlers
        private void MalomModel_GameWon(object sender, GameWonEventArgs e)
        {
            switch (e.Player)
            {
                case Player.Blue:
                    MainPage.DisplayAlert("Malom", "Gratulálok kék, győztél!", "OK");
                    break;
                case Player.Red:
                    MainPage.DisplayAlert("Malom", "Gratulálok piros, győztél!", "OK");
                    break;
            }
        }
        #endregion
    }
}
