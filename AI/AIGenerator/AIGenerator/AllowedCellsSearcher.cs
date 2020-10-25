using ReversiCore.Enums;
using System.Collections.Generic;

namespace AIGenerator
{
    internal class AllowedCellsSearcher : Searcher
    {
        internal AllowedCellsSearcher(BoardState currentField, Color currentPlayerColor)
            : base(currentField, currentPlayerColor) { }


        /*
         * Method finds all the cells where current player can put their chips into
         * -----------------------------------------
         * currentPlayerColor - color of the current player
         */
        internal SortedSet<Cell> GetAllAllowedCells()
        {
            SortedSet<Cell> AllAllowedCells = new SortedSet<Cell>();

            for (int i = 0; i < field.FieldSize; i++)
            { 
                for (int j = 0; j < field.FieldSize; j++)
                {
                    if (field.Field[i, j] == playerColor)
                    {
                        IEnumerable<Cell> NewAllowedCells = GetAllowedCellsForCell(new Cell(i, j));
                        AllAllowedCells.UnionWith(NewAllowedCells);
                    }
                }
            }

            return AllAllowedCells;
        }


        /*
         * Method finds all the empty cells which can be reached 
         * from chip in the given cell by the rules
         * -----------------------------------------
         * cell - cell from which we try to reach other cells
         * -----------------------------------------
         * Example:
         *           ✖
         *        ⚪
         * ✖⚪⚫⚪⚪⚪✖
         */
        private SortedSet<Cell> GetAllowedCellsForCell(Cell cell)
        {
            SortedSet<Cell> AllowedCells = new SortedSet<Cell>();

            for (int stepX = -1; stepX <= 1; stepX++)
            {
                for (int stepY = -1; stepY <= 1; stepY++)
                {
                    int maxDistance = GetDistanceToFieldEdgeFromCell(cell, stepX, stepY);

                    int distance = 1;
                    int currentX = cell.X;
                    int currentY = cell.Y;

                    while (distance <= maxDistance)
                    {
                        currentX += stepX;
                        currentY += stepY;

                        if (field.Field[currentX, currentY] == playerColor)
                        {
                            break;
                        }

                        if (field.Field[currentX, currentY] == null)
                        {
                            if (distance > 1)
                            {
                                AllowedCells.Add(new Cell(currentX, currentY));
                            }

                            break;
                        }

                        distance++;
                    }
                }
            }

            return AllowedCells;
        }
    }
}
