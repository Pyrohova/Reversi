using System;

namespace ReversiCore.Interfaces
{
    public interface IReversiModel
    {
        event EventHandler<NewGameEventArgs> NewGameStarted;
        event EventHandler<SetChipsEventArgs> SetChips;
        event EventHandler<WrongMoveEventArgs> WrongMove;
        event EventHandler<CountChangedEventArgs> CountChanged;
        event EventHandler<SwitchMoveEventArgs> SwitchMove;
        event EventHandler<GameOverEventArgs> GameOver;
    }
}
