using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ReversiCore
{
    internal class AllowedCellsSearcher : Searcher
    {
        internal AllowedCellsSearcher(Board currentBoard, Color currentPlayerColor)
            : base(currentBoard, currentPlayerColor) { }

        internal SortedSet<Cell> GetAllAllowedCells(Color currentPlayerColor)
        {
            SortedSet<Cell> AllAllowedCells = new SortedSet<Cell>();

            for (int i = 0; i < board.Size; i++)
            { 
                for (int j = 0; j < board.Size; j++)
                {
                    if (board.Field[i, j] == currentPlayerColor)
                    {
                        IEnumerable<Cell> NewAllowedCells = GetAllowedCellsForCell(new Cell(i, j));
                        AllAllowedCells.UnionWith(NewAllowedCells);
                    }
                }
            }

            return AllAllowedCells;
        }

        private SortedSet<Cell> GetAllowedCellsForCell(Cell cell)
        {
            SortedSet<Cell> AllowedCells = new SortedSet<Cell>();

            for (int stepX = -1; stepX <= 1; stepX++)
            {
                for (int stepY = -1; stepY <= 1; stepY++)
                {
                    int maxDistance = GetDistanceToFieldEdgeForCell(cell, stepX, stepY);

                    int distance = 1;
                    int currentX = cell.X;
                    int currentY = cell.Y;

                    while (distance <= maxDistance)
                    {
                        currentX += stepX;
                        currentY += stepY;

                        if (board.Field[currentX, currentY] == playerColor)
                        {
                            break;
                        }

                        if (board.Field[currentX, currentY] == null && distance > 1)
                        {
                            AllowedCells.Add(new Cell(currentX, currentY));
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
