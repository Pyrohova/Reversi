using ReversiCore.Enums;
using System;

namespace ReversiCore
{
    public class NewGameEventArgs : EventArgs
    {
        public GameMode NewGameMode { get; set; }
    }
}
