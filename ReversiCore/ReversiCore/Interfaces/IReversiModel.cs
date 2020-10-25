using ReversiCore.Enums;
using System;

namespace ReversiCore.Interfaces
{
    public interface IReversiModel
    {
        event EventHandler NewGameStarted; //Invokes when new game has been started
        event EventHandler<SetChipsEventArgs> SetChips; //Invokes when chips layout has been changed
        event EventHandler<SwitchMoveEventArgs> SwitchMove; //Invokes when turn has been switched
        event EventHandler<WrongMoveEventArgs> WrongMove; //Invokes when wrong move has been made by one of the users
        event EventHandler<CountChangedEventArgs> CountChanged; //Invokes when players score has been changed
        event EventHandler<GameOverEventArgs> GameOver; //Invokes when game has been finished

        /*
         * Method for controller to start new game 
         */
        void NewGame();

        /*
         * Method for controller to put new chips
         * -----------------------------------------
         * x - x coordinate of new chip
         * y - y coordinate of new chip
         */
        void PutChip(int x, int y);
    }
}
