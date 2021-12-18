using System;

namespace MalomXamarin2.Model
{
    public class FieldChangedEventArgs : EventArgs
    {
        public Player Player { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public FieldChangedEventArgs(int x, int y, Player player)
        {
            Player = player;
            X = x;
            Y = y;
        }
    }
}

