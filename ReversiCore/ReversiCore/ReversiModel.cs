using ReversiCore.Enums;
using ReversiCore.Interfaces;
using System;

namespace ReversiCore
{
    public class ReversiModel : IReversiModel
    {
        private Color currentTurnColor;
        private BoardManipulator boardManipulator;

        public event EventHandler<NewGameEventArgs> NewGame;
        public event EventHandler<SetChipsEventArgs> SetChips;
        public event EventHandler<CountChangedEventArgs> CountChanged;
        public event EventHandler<SwitchMoveEventArgs> SwitchMove;
        public event EventHandler<GameOverEventArgs> GameOver;

        public ReversiModel()
        {
            currentTurnColor = Color.Black;
            boardManipulator = new BoardManipulator();
        }

        
    }
}
