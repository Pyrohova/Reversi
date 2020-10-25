using ReversiCore.Enums;
using ReversiCore.Interfaces;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    //Model of the reversi game
    public class ReversiModel : IReversiModel
    {
        private TurnHolder turnHolder; //Responsible for current and opposite turns
        private Board board; //Game board
        private CountHolder countHolder; //Responsible for current score of both players (Black and White)
        private SortedSet<Cell> currentAllowedCells; //Set of cells where current player can put their chip

        public event EventHandler NewGameStarted; //Invokes when new game has been started
        public event EventHandler<SetChipsEventArgs> SetChips; //Invokes when chips layout has been changed
        public event EventHandler<SwitchMoveEventArgs> SwitchMove; //Invokes when turn has been switched
        public event EventHandler<WrongMoveEventArgs> WrongMove; //Invokes when wrong move has been made by one of the users
        public event EventHandler<CountChangedEventArgs> CountChanged; //Invokes when players score has been changed
        public event EventHandler<GameOverEventArgs> GameOver; //Invokes when game has been finished
        
        public ReversiModel()
        {
            turnHolder = new TurnHolder();
            board = new Board();
            currentAllowedCells = new SortedSet<Cell>();
            countHolder = new CountHolder();
        }


        /*
         * Method for controller to start new game 
         */
        public void NewGame()
        {
            turnHolder.Reset();
            
            NewGameStarted?.Invoke(this, new EventArgs());

            SetStartBoardPosition();

            countHolder.Reset();

            FinishTurnAndMoveOn();
        }


        /*
         * Method for controller to put new chips
         * -----------------------------------------
         * x - x coordinate of new chip
         * y - y coordinate of new chip
         */
        public void PutChip(int x, int y)
        {
            Chip newChip = new Chip(turnHolder.CurrentTurnColor, new Cell(x, y));

            if (!NewChipIsAllowed(newChip))
            {
                return;
            }

            board.AddChip(newChip);

            List<Chip> changedChips = board.GetChangedChips(newChip, turnHolder.CurrentTurnColor);
            countHolder.Increase(turnHolder.CurrentTurnColor, changedChips.Count + 1);
            countHolder.Decrease(turnHolder.OppositeTurnColor, changedChips.Count);

            SetChips?.Invoke(this, new SetChipsEventArgs { NewChip = newChip, ChangedChips = changedChips });

            turnHolder.Switch();

            FinishTurnAndMoveOn();
        }


        /*
         * Metod prepares model and players for the next move 
         * and notifies players that move has been switched.
         * If next move is impossible, method notifies players 
         * about the end of the game.
         */
        private void FinishTurnAndMoveOn()
        {
            CountChanged?.Invoke(this, new CountChangedEventArgs
            {
                CountWhite = countHolder.GetPlayerCount(Color.White),
                CountBlack = countHolder.GetPlayerCount(Color.Black),
            });

            currentAllowedCells = board.GetAllowedCells(turnHolder.CurrentTurnColor);

            if (currentAllowedCells.Count == 0)
            {
                EndGame();
                return;
            }

            SwitchMove?.Invoke(
                this, 
                new SwitchMoveEventArgs 
                    { 
                        AllowedCells = currentAllowedCells, 
                        CurrentPlayerColor = turnHolder.CurrentTurnColor 
                    }
                );
        }


        /* 
         * Method asks both model and players to put board field into start position
         */
        private void SetStartBoardPosition()
        {
            board.SetStartPosition();

            foreach(Chip chip in board.StartChips)
            {
                SetChips?.Invoke(this, new SetChipsEventArgs { NewChip = chip, ChangedChips = new List<Chip>() });
            }
        }


        /*
         * Method checks if chip that tries to be put on the board field is allowed to do so
         * -----------------------------------------
         * chip - chip that tries to be put
         */
        private bool NewChipIsAllowed(Chip chip)
        {
            if (chip.Color != turnHolder.CurrentTurnColor || !currentAllowedCells.Contains(chip.Cell))
            {
                WrongMove?.Invoke(this, new WrongMoveEventArgs { WrongChip = chip });
                return false;
            }

            return true;
        }


        /*
         * Method notifies players about an end of the game
         */
        private void EndGame()
        {
            GameOver?.Invoke(this, new GameOverEventArgs { WinnerColor = countHolder.GetWinner() });
        }
    }
}
