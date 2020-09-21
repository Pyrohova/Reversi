using ReversiCore.Enums;
using System;

namespace ReversiCore
{
    public class GameOverEventArgs : EventArgs
    {
        public Color? WinnerColor { get; set; }
    }
}
