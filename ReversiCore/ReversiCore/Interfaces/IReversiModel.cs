using System;

namespace ReversiCore.Interfaces
{
    public interface IReversiModel
    {
        event EventHandler<NewGameEventArgs> NewGame;
        event EventHandler<SetChipsEventArgs> SetChips; 
        event EventHandler<CountChangedEventArgs> CountChanged;
        event EventHandler<SwitchMoveEventArgs> SwitchMove;
        event EventHandler<GameOverEventArgs> GameOver;
    }
}
