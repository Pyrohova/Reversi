using ReversiCore.Enums;
using System;

namespace ReversiCore.Interfaces
{
    public interface IReversiModel
    {
        event EventHandler<NewGameEventArgs> NewGameStarted; //Invokes when new game has been started
        event EventHandler<SetChipsEventArgs> SetChips; //Invokes when chips layout has been changed
        event EventHandler<SwitchMoveEventArgs> SwitchMove; //Invokes when turn has been switched
        event EventHandler<WrongMoveEventArgs> WrongMove; //Invokes when wrong move has been made by one of the users
        event EventHandler<CountChangedEventArgs> CountChanged; //Invokes when players score has been changed
        event EventHandler<GameOverEventArgs> GameOver; //Invokes when game has been finished
        event EventHandler RobotDisabled; //Invokes when robot player has been disabled
        event EventHandler<RobotColorSetEventArgs> RobotColorSet; //Invokes to call robot player to play for a certain color

        /*
         * Method for controller to start new game 
         * -----------------------------------------
         * newGameMode - mode of the new game (HumanToHuman or HumanToRobot)
         * userPlayerColor - color of the human player (not used in HumanToHuman mode)
         */
        void NewGame(GameMode newGameMode, Color? userPlayerColor = null);

        /*
         * Method for controller to put new chips
         * -----------------------------------------
         * x - x coordinate of new chip
         * y - y coordinate of new chip
         */
        void PutChip(int x, int y);
    }
}
