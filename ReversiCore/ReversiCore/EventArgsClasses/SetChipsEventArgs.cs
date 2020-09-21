using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class SetChipsEventArgs : EventArgs
    {
        public Color NewChip { get; set; }
        public IEnumerable<Color> ChangedChips { get; set; }
    }
}
