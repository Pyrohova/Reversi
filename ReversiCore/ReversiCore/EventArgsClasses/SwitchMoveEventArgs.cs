using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class SwitchMoveEventArgs : EventArgs
    {
        public SortedSet<Cell> AllowedCells { get; set; } //Cells where current player can put a chip into
        public Color CurrentPlayerColor { get; set; } //Color of the current player
    }
}
