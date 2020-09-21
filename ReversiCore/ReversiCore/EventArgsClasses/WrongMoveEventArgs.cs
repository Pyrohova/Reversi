using System;

namespace ReversiCore
{
    public class WrongMoveEventArgs : EventArgs
    {
        public Chip WrongChip { get; set; }
    }
}
