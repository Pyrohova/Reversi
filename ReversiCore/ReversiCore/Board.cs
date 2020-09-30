using ReversiCore.Enums;
using System.Collections.Generic;

namespace ReversiCore
{
    //Game board
    internal class Board
    {
        internal Field BoardField { get; private set; } //Field of the board that contains chips
        internal List<Chip> StartChips { get; private set; } //List of chips that have to be put onto board field when game is started

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


        /*
         * Method clears board field
         */
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


        /*
         * Method sets chips on board field into start position
         */
        internal void SetStartPosition()
        {
            Clear();
            
            foreach(Chip chip in StartChips)
            {
                BoardField.PlacedChips[chip.Cell.X, chip.Cell.Y] = chip.Color;
            }
        }


        /*
         * Method returns all the cells where current player can put their chips into
         * -----------------------------------------
         * currentPlayerColor - color of the current player
         */
        internal SortedSet<Cell> GetAllowedCells(Color currentPlayerColor)
        {
            AllowedCellsSearcher allowedCellsSearcher = new AllowedCellsSearcher(BoardField, currentPlayerColor);
            return allowedCellsSearcher.GetAllAllowedCells(currentPlayerColor);
        }


        /*
         * Method adds given chip onto board field
         * -----------------------------------------
         * chip - chip that has to be added onto board field
         */
        internal void AddChip(Chip chip)
        {
            BoardField.PlacedChips[chip.Cell.X, chip.Cell.Y] = chip.Color;
        }


        /*
         * Method changes color of all the chips which color has to be changed
         * after putting given chip onto the board field
         * and returns them
         * -----------------------------------------
         * newChip - chip that is put onto the board field
         * currentPlayerColor - color of the current player
         */
        internal List<Chip> GetChangedChips(Chip newChip, Color currentPlayerColor)
        {
            ChangedChipsSearcher changedChipsSearcher = new ChangedChipsSearcher(BoardField, currentPlayerColor);
            List<Chip> changedChips = changedChipsSearcher.GetAllChangedChips(newChip);
            RepaintChips(changedChips, currentPlayerColor);
            return changedChips;
        }


        /*
         * Method repeains list of given chips into given color
         * -----------------------------------------
         * changedChips - list of chips that has to be repainted
         * color - color which chips are have to be repainted into
         */
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
