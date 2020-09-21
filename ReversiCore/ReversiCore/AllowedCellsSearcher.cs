using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal class AllowedCellsSearcher
    {
        private Board board;

        internal AllowedCellsSearcher(Board currentBoard)
        {
            board = currentBoard;
        }

        internal IEnumerable<Cell> GetAllAllowedCells(Color currentPlayerColor)
        {
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board.Field[i, j] == currentPlayerColor)
                    {
                        GetAllowedCellsForCell(new Cell(i, j));
                    }
                }
            }
        }

        internal IEnumerable<Cell> GetAllowedCellsForChip(Chip currentChip)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (board.Field[i, j] == currentPlayerColor)
                    {

                    }
                }
            }
        }
    }
}
