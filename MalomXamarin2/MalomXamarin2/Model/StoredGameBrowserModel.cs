using MalomXamarin2.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MalomXamarin2.Model
{
    public class StoredGameBrowserModel
    {
        private IStore _store;
        public event EventHandler StoreChanged;

        public StoredGameBrowserModel(IStore store)
        {
            _store = store;
            StoredGames = new List<StoredGameModel>();
        }

        public List<StoredGameModel> StoredGames { get; private set; }

        public void Update()
        {
            if (_store == null)
                return;

            StoredGames.Clear();

            foreach (string name in _store.GetFiles())
            {
                if (name == "SuspendedGame")
                    continue;

                StoredGames.Add(new StoredGameModel
                {
                    Name = name,
                    Modified = _store.GetModifiedTime(name)
                });
            }

            StoredGames = StoredGames.OrderByDescending(item => item.Modified).ToList();

            OnSavesChanged();
        }

        private void OnSavesChanged()
        {
            if (StoreChanged != null)
                StoreChanged(this, EventArgs.Empty);
        }
    }
}

