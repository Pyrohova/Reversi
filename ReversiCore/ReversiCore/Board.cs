using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal class Board
    {
        internal Field BoardField { get; private set; }

        public List<Chip> StartChips { get; private set; }

        internal Board()
        {
            BoardField = new Field();

            StartChips = new List<Chip>()
            {
                new Chip(Color.White, new Cell(3, 3)),
                new Chip(Color.White, new Cell(4, 4)),
                new Chip(Color.Black, new Cell(3, 4)),
                new Chip(Color.Black, new Cell(4, 3))
            };

            Clear();
        }

        private void Clear()
        {
            for (int i = 0; i < BoardField.Size; i++)
            {
                for (int j = 0; j < BoardField.Size; j++)
                {
                    BoardField.PlacedChips[i, j] = null;
                }
            }
        }

        internal void SetStartPosition()
        {
            Clear();
            
            foreach(Chip chip in StartChips)
            {
                BoardField.PlacedChips[chip.Cell.X, chip.Cell.Y] = chip.Color;
            }
        }

        internal SortedSet<Cell> GetAllowedCells(Color currentPlayerColor)
        {
            AllowedCellsSearcher allowedCellsSearcher = new AllowedCellsSearcher(BoardField, currentPlayerColor);
            return allowedCellsSearcher.GetAllAllowedCells(currentPlayerColor);
        }

        internal void AddChip(Chip chip)
        {
            BoardField.PlacedChips[chip.Cell.X, chip.Cell.Y] = chip.Color;
        }

        internal List<Chip> GetChangedChips(Chip newChip, Color currentPlayerColor)
        {
            ChangedChipsSearcher changedChipsSearcher = new ChangedChipsSearcher(BoardField, currentPlayerColor);
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
                BoardField.PlacedChips[chip.Cell.X, chip.Cell.Y] = color;
            }
        }
    }
}
