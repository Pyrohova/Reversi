using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal class BoardManipulator
    {
        private Board board;

        internal BoardManipulator()
        {
            board = new Board();
            SetStartPosition();
        }

        internal void Clear()
        {
            board.FilledCells.Clear();
        }

        internal void SetStartPosition()
        {
            Clear();
            board.Field[3, 3] = Color.White;
            board.Field[4, 4] = Color.White;
            board.Field[3, 4] = Color.Black;
            board.Field[4, 3] = Color.Black;
        }

        internal IEnumerable<Cell> GetAllowedCells(Color currentPlayerColor)
        {
            AllowedCellsSearcher allowedCellsSearcher = new AllowedCellsSearcher(board);
            return allowedCellsSearcher.GetAllAllowedCells(currentPlayerColor);
        }
    }
}
