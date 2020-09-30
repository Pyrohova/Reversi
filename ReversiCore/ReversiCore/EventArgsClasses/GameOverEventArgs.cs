using ReversiCore.Enums;
using System;

namespace ReversiCore
{
    public class GameOverEventArgs : EventArgs
    {
        public Color? WinnerColor { get; set; } //Color of the winner player of the game
    }
}
