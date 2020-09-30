using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class SetChipsEventArgs : EventArgs
    {
        public Chip NewChip { get; set; } //Chip that has been put board field
        public List<Chip> ChangedChips { get; set; } //List of chips that changed their color
    }
}
