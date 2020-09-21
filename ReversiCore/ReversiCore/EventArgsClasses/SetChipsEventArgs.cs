using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class SetChipsEventArgs : EventArgs
    {
        public Chip NewChip { get; set; }
        public IEnumerable<Chip> ChangedChips { get; set; }
    }
}
