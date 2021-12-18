using System;

namespace MalomXamarin2.Model
{
    public class GameWonEventArgs : EventArgs
    {
        public Player Player { get; private set; }

        public GameWonEventArgs(Player player) { Player = player; }
    }
}

