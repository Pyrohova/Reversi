using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ReversiRobot
{
    public class RandomUser
    {
        private Random rand;
        private ReversiModel model;
        private Color? currentColor;

        public RandomUser(ReversiModel reversiModel)
        {
            rand = new Random();

            Disable();

            model = reversiModel;

            model.RobotColorSet +=
                (sender, eventArgs) => { currentColor = eventArgs.RobotColor; };

            model.RobotDisabled += (s, eventArgs) => { Disable(); };

            model.SwitchMove += OnSwitchMove;
        }

        internal void MakeMove(SortedSet<Cell> allowedCells)
        {
            int randAllowedCellNumber = rand.Next(allowedCells.Count);

            Cell currentMoveCell = allowedCells.ToList()[randAllowedCellNumber];

            model.PutChip(currentMoveCell.X, currentMoveCell.Y);
        }

        private void OnSwitchMove(object sender, SwitchMoveEventArgs eventArgs)
        {
            if (eventArgs.CurrentPlayerColor == currentColor)
            {
                Thread.Sleep(2000);
                MakeMove(eventArgs.AllowedCells);
            }
        }

        private void Disable()
        {
            currentColor = null;
        }
    }
}
