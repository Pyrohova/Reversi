using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal abstract class Searcher
    {
        protected Field field;
        protected Color playerColor;

        internal Searcher(Field currentField, Color currentPlayerColor)
        {
            field = currentField;
            playerColor = currentPlayerColor;
        }

        protected int GetDistanceToFieldEdgeForCell(Cell cell, int stepX, int stepY)
        {
            int distanceX = GetDistanceToFieldEdgeForCoordinate(cell.X, stepX);
            int distanceY = GetDistanceToFieldEdgeForCoordinate(cell.Y, stepY);

            return Math.Min(distanceX, distanceY);
        }

        protected int GetDistanceToFieldEdgeForCoordinate(int coordinate, int step)
        {
            if (step == -1)
            {
                return coordinate;
            }

            if (step == 1)
            {
                return 7 - coordinate;
            }

            int infinityDistance = field.Size;
            return infinityDistance;
        }
    }
}
