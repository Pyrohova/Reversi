using System.Collections.Generic;

namespace AIGenerator
{
    internal class ChangedChipsSearcher : Searcher
    {
        internal ChangedChipsSearcher(BoardState currentField, Color currentPlayerColor)
            : base(currentField, currentPlayerColor) { }


        /*
         * Method finds all the chips which color has to be changed 
         * after putting given chip onto the board field
         * -----------------------------------------
         * chip - chip that is put onto the board field
         */
        internal List<Chip> GetAllChangedChips(Chip chip)
        {
            List<Chip> ChangedChips = new List<Chip>();

            for (int stepX = -1; stepX <= 1; stepX++)
            {
                for (int stepY = -1; stepY <= 1; stepY++)
                {
                    int maxDistance = GetDistanceToFieldEdgeFromCell(chip.Cell, stepX, stepY);

                    int distance = 1;
                    int currentX = chip.Cell.X;
                    int currentY = chip.Cell.Y;

                    while (distance <= maxDistance)
                    {
                        currentX += stepX;
                        currentY += stepY;

                        if (field.Field[currentX, currentY] == Color.None)
                        {
                            break;
                        }

                        if (field.Field[currentX, currentY] == playerColor)
                        {
                            ChangedChips.AddRange(GetListOfChipsInRow(chip.Cell, stepX, stepY, distance - 1));
                            break;
                        }

                        distance++;
                    }
                }
            }

            return ChangedChips;
        }


        /*
         * Method returns a row of chips started from the given cell 
         * moving by given step values for given length of the row
         * -----------------------------------------
         * cell - cell that starts the row (excluded)
         * stepX - step that has to be taken along x-axis
         * stepY - step that has to be taken along y-axis
         * length - length of the row
         */
        private List<Chip> GetListOfChipsInRow(Cell cell, int stepX, int stepY, int length)
        {
            int currentX = cell.X;
            int currentY = cell.Y;

            List<Chip> chips = new List<Chip>();

            for (int i = 0; i < length; i++)
            {
                currentX += stepX;
                currentY += stepY;

                chips.Add(new Chip(playerColor, new Cell(currentX, currentY)));
            }

            return chips;
        }
    }
}
