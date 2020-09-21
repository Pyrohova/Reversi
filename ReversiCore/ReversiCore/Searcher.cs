using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal abstract class Searcher
    {
        protected Board board;
        protected Color playerColor;

        internal Searcher(Board currentBoard, Color currentPlayerColor)
        {
            board = currentBoard;
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

            //"infinity"
            return board.Size;
        }
    }
}
