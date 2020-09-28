using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversiRobot
{
    public class RandomUser
    {
        private Random rand;
        private ReversiModel model;
        private Color currentColor;

        public RandomUser(ReversiModel reversiModel)
        {
            rand = new Random();

            model = reversiModel;

            model.RobotColorSet +=
                (sender, eventArgs) => ChangeColor(eventArgs.RobotColor);
        }

        internal void MakeMove(SortedSet<Cell> allowedCells)
        {
            int randAllowedCellNumber = rand.Next(allowedCells.Count);

            Cell currentMoveCell = allowedCells.ToList()[randAllowedCellNumber];

            model.PutChip(currentMoveCell.X, currentMoveCell.Y);
        }

        private void ChangeColor(Color newCurrentColor)
        {
            currentColor = newCurrentColor;

            model.SwitchMove[currentColor] +=
                (sender, eventArgs) => MakeMove(eventArgs.AllowedCells);
        }
    }
}
