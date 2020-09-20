using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class SetChipsEventArgs : EventArgs
    {
        public IEnumerable<Color> Chips { get; set; }
    }
}
