using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class SwitchMoveEventArgs : EventArgs
    {
        public IEnumerable<Cell> AllowedCells { get; set; }
        public Color CurrentPlayerColor { get; set; }
    }
}
