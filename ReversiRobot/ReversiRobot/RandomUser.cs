using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversiRobot
{
    //User that makes random moves into allowed cells
    public class RandomUser
    {
        private Random rand;
        private ReversiModel model; //Reversi model
        private Color? currentColor; //Current color of this player

        public RandomUser(ReversiModel reversiModel)
        {
            rand = new Random();

            currentColor = null;

            model = reversiModel;
            model.SwitchMove += OnSwitchMove;
        }


        /*
         * Method that enables robot player for the current game (playing for given color)
         * -----------------------------------------
         * newCOlor - color of robot for the current game
         */
        public void Enable(Color newColor)
        {
            currentColor = newColor;
            //model.SwitchMove += OnSwitchMove;
        }


        /*
         * Method that disables robot player in current game
         */
        public void Disable()
        {
            currentColor = null;
            //model.SwitchMove -= OnSwitchMove;
        }


        /*
         * Method that makes random move into one of allowed cells
         * -----------------------------------------
         * allowedCells - cells where user currently can put chip into
         */
        private void MakeMove(SortedSet<Cell> allowedCells)
        {
            int randAllowedCellNumber = rand.Next(allowedCells.Count);

            Cell currentMoveCell = allowedCells.ToList()[randAllowedCellNumber];

            model.PutChip(currentMoveCell.X, currentMoveCell.Y);
        }


        /*
         * Method that makes move if it is current user turn
         */
        private void OnSwitchMove(object sender, SwitchMoveEventArgs eventArgs)
        {
            if (eventArgs.CurrentPlayerColor == currentColor)
            {
                MakeMove(eventArgs.AllowedCells);
            }
        }
    }
}
