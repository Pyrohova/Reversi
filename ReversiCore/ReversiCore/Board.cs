using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal class Board
    {
        internal int Size { get; private set; }
        internal Color?[,] Field { get; set; }

        internal Board()
        {
            Size = 8;
            Field = new Color?[Size, Size];
        }
    }
}
