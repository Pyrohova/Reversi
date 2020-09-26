using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore.Interfaces
{
    public interface IReversiModel
    {
        event EventHandler<NewGameEventArgs> NewGameStarted;
        event EventHandler<SetChipsEventArgs> SetChips;
        event EventHandler<WrongMoveEventArgs> WrongMove;
        event EventHandler<CountChangedEventArgs> CountChanged;
        event EventHandler<GameOverEventArgs> GameOver;
        Dictionary<Color, EventHandler<SwitchMoveEventArgs>> SwitchMove { get; set; }
    }
}
