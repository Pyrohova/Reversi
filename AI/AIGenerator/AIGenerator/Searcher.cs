using System;

namespace AIGenerator
{
    internal abstract class Searcher
    {
        protected BoardState field;
        protected Color playerColor;

        internal Searcher(BoardState currentField, Color currentPlayerColor)
        {
            field = currentField;
            playerColor = currentPlayerColor;
        }


        /*
         * Method returns distance to board field edge from the given cell
         * moving by given steps
         * -----------------------------------------
         * cell - cell which distance to field edge calculates from
         * stepX - step that has to be taken along x-axis
         * stepY - step that has to be taken along y-axis
         */
        protected int GetDistanceToFieldEdgeFromCell(Cell cell, int stepX, int stepY)
        {
            int distanceX = GetDistanceToFieldEdgeForCoordinate(cell.X, stepX);
            int distanceY = GetDistanceToFieldEdgeForCoordinate(cell.Y, stepY);

            return Math.Min(distanceX, distanceY);
        }


        /*
         * Method returns distance to board field edge from the given coordinate
         * moving by given step
         * -----------------------------------------
         * coordinate - coordinate which distance to field edge calculates from
         * step - step that has to be taken along corresponding axis
         */
        protected int GetDistanceToFieldEdgeForCoordinate(int coordinate, int step)
        {
            if (step == -1)
            {
                return coordinate;
            }

            if (step == 1)
            {
                return field.FieldSize - 1 - coordinate;
            }

            int infinityDistance = field.FieldSize;
            return infinityDistance;
        }
    }
}
