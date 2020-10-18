using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using ReversiColor = ReversiCore.Enums.Color;

namespace AIGenerator
{
    class Program
    {
        static void MakeNextMoves(BoardState boardState, Color currentPlayerColor)
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(boardState, currentPlayerColor);
            SortedSet<Cell> allowedCells = cellsSearcher.GetAllAllowedCells();

            if (allowedCells.Count == 0)
            {
                //endOfGame
                return;
            }

            Color oppositeColor;

            if (currentPlayerColor == Color.White)
            {
                oppositeColor = Color.Black;
            }
            else
            {
                oppositeColor = Color.White;
            }

            foreach(Cell cell in allowedCells)
            {
                BoardState nextBoardState = GetBoardStateAfterMove(boardState, cell, currentPlayerColor);

                MakeNextMoves(nextBoardState, oppositeColor);
            }
        }

        static BoardState GetBoardStateAfterMove(BoardState prevBoardState, Cell moveCell, Color moveColor)
        {
            BoardState nextBoardState = new BoardState();
            prevBoardState.Field.CopyTo(nextBoardState.Field, 0);
            nextBoardState.Field[moveCell.X, moveCell.Y] = moveColor;

            ChangedChipsSearcher changedChipsSearcher = new ChangedChipsSearcher(prevBoardState, moveColor);
            List<Chip> changedChips = changedChipsSearcher.GetAllChangedChips(new Chip(moveColor, moveCell));

            foreach(Chip chip in changedChips)
            {
                nextBoardState.Field[chip.Cell.X, chip.Cell.Y] = chip.Color;
            }

            return nextBoardState;
        }

        static void Main(string[] args)
        {
            ReversiModel model = new ReversiModel();

            Random rand = new Random();
            ReversiColor currentColor = ReversiColor.Black;

            model.NewGame(GameMode.HumanToRobot, ReversiColor.Black);
        }
    }
}
