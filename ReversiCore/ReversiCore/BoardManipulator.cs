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
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    board.Field[i, j] = null;
                }
            }
        }

        internal void SetStartPosition()
        {
            Clear();
            board.Field[3, 3] = Color.White;
            board.Field[4, 4] = Color.White;
            board.Field[3, 4] = Color.Black;
            board.Field[4, 3] = Color.Black;
        }

        internal SortedSet<Cell> GetAllowedCells(Color currentPlayerColor)
        {
            AllowedCellsSearcher allowedCellsSearcher = new AllowedCellsSearcher(board, currentPlayerColor);
            return allowedCellsSearcher.GetAllAllowedCells(currentPlayerColor);
        }

        internal void AddChip(Chip chip)
        {
            board.Field[chip.Cell.X, chip.Cell.Y] = chip.Color;
        }

        internal List<Chip> GetChangedChips(Chip newChip, Color currentPlayerColor)
        {
            ChangedChipsSearcher changedChipsSearcher = new ChangedChipsSearcher(board, currentPlayerColor);
            List<Chip> changedChips = changedChipsSearcher.GetAllChangedChips(newChip);
            RepaintChips(changedChips, currentPlayerColor);
            return changedChips;
        }

        private void RepaintChips(List<Chip> changedChips, Color color)
        {
            for (int i = 0; i < changedChips.Count; i++)
            {
                Chip chip = changedChips[i];
                chip.Color = color;
                board.Field[chip.Cell.X, chip.Cell.Y] = color;
            }
        }
    }
}
