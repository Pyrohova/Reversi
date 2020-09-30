using ReversiCore.Enums;
using System;

namespace ReversiCore
{
    public class NewGameEventArgs : EventArgs
    {
        public GameMode NewGameMode { get; set; } //Mode of the new game (HumanToHuman or HumanToRobot)
        public Color? UserPlayerColor { get; set; } //Color of the human player (not used in HumanToHuman mode)
    }
}
