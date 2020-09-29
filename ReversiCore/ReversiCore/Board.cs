using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal class Board
    {
        internal int Size { get; private set; }
        internal Color?[,] Field { get; private set; }

        public List<Chip> StartChips { get; private set; }

        internal Board()
        {
            Size = 8;
            Field = new Color?[Size, Size];

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
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Field[i, j] = null;
                }
            }
        }

        internal void SetStartPosition()
        {
            Clear();
            
            foreach(Chip chip in StartChips)
            {
                Field[chip.Cell.X, chip.Cell.Y] = chip.Color;
            }
        }

        internal SortedSet<Cell> GetAllowedCells(Color currentPlayerColor)
        {
            AllowedCellsSearcher allowedCellsSearcher = new AllowedCellsSearcher(this, currentPlayerColor); //Field, not this TODO
            return allowedCellsSearcher.GetAllAllowedCells(currentPlayerColor);
        }

        internal void AddChip(Chip chip)
        {
            Field[chip.Cell.X, chip.Cell.Y] = chip.Color;
        }

        internal List<Chip> GetChangedChips(Chip newChip, Color currentPlayerColor)
        {
            ChangedChipsSearcher changedChipsSearcher = new ChangedChipsSearcher(this, currentPlayerColor); //Field, not this TODO
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
                Field[chip.Cell.X, chip.Cell.Y] = color;
            }
        }
    }
}
