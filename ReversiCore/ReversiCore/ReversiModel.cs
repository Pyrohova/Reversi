using ReversiCore.Interfaces;
using System;

namespace ReversiCore
{
    public class ReversiModel : IReversiModel
    {
        public event EventHandler<NewGameEventArgs> NewGame;
        public event EventHandler<SetChipsEventArgs> SetChips;
        public event EventHandler<SwitchMoveEventArgs> SwitchMove;
        public event EventHandler<GameOverEventArgs> GameOver;

        public ReversiModel()
        {
        }
    }
}
