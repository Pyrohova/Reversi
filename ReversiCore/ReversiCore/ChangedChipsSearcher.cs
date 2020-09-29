using ReversiCore.Enums;
using System.Collections.Generic;

namespace ReversiCore
{
    internal class ChangedChipsSearcher : Searcher
    {
        internal ChangedChipsSearcher(Board currentBoard, Color currentPlayerColor)
            : base(currentBoard, currentPlayerColor) { }

        internal List<Chip> GetAllChangedChips(Chip chip)
        {
            List<Chip> ChangedChips = new List<Chip>();

            for (int stepX = -1; stepX <= 1; stepX++)
            {
                for (int stepY = -1; stepY <= 1; stepY++)
                {
                    int maxDistance = GetDistanceToFieldEdgeForCell(chip.Cell, stepX, stepY);

                    int distance = 1;
                    int currentX = chip.Cell.X;
                    int currentY = chip.Cell.Y;

                    while (distance <= maxDistance)
                    {
                        currentX += stepX;
                        currentY += stepY;

                        if (board.Field[currentX, currentY] == null)
                        {
                            break;
                        }

                        if (board.Field[currentX, currentY] == playerColor)
                        {
                            ChangedChips.AddRange(GetListOfChipsInRow(chip.Cell, stepX, stepY, distance));
                            break;
                        }

                        distance++;
                    }
                }
            }

            return ChangedChips;
        }

        private List<Chip> GetListOfChipsInRow(Cell cell, int stepX, int stepY, int length)
        {
            int currentX = cell.X;
            int currentY = cell.Y;

            List<Chip> chips = new List<Chip>();

            for (int i = 0; i < length - 1; i++)
            {
                currentX += stepX;
                currentY += stepY;

                chips.Add(new Chip(playerColor, new Cell(currentX, currentY)));
            }

            return chips;
        }
    }
}
