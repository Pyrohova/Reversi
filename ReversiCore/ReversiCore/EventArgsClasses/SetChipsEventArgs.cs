using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class SetChipsEventArgs : EventArgs
    {
        public Chip NewChip { get; set; }
        public List<Chip> ChangedChips { get; set; }
    }
}
