using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using ReversiColor = ReversiCore.Enums.Color;

namespace AIGenerator
{
    class Program
    {
        static int maxDepth = 2;

        static int startValueForBestMaxValue = -100;
        static int startValueForBestMinValue = 100;

        static int allowedCellsAmount;
        static float firstBestValue;
        static float secondBestValue;

        static BoardState currentBoardState;
        static ReversiModel model;



        static float MiniMax(BoardState boardState, Color currentPlayerColor, int depth, bool maximizingPlayer, float alpha, float beta)
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(boardState, currentPlayerColor);
            SortedSet<Cell> allowedCells = cellsSearcher.GetAllAllowedCells();

            if (allowedCells.Count == 0 || depth == 0)
            {
                //endOfGame
                //return;
                return StaticEvaluationFunction(boardState, true);
            }
            else
            {
                float eval = 0;
                Color oppositeColor = (currentPlayerColor == Color.White) ? Color.Black : Color.White;
                allowedCellsAmount = allowedCells.Count;

                if (maximizingPlayer)
                {
                    float maxValue = startValueForBestMaxValue;

                    // for each child of the current boardState
                    foreach (Cell cell in allowedCells)
                    {
                        // get child
                        BoardState nextBoardState = GetBoardStateAfterMove(boardState, cell, currentPlayerColor);

                        // set max value
                        eval = MiniMax(nextBoardState, oppositeColor, depth - 1, false, alpha, beta);
                        maxValue = Math.Max(maxValue, eval);

                        // set alpha
                        alpha = Math.Max(alpha, eval);
                        if (beta <= alpha)
                            break;
                    }

                    return maxValue;
                }
                else
                {
                    float minValue = startValueForBestMinValue;

                    // for each child of the current boardState
                    foreach (Cell cell in allowedCells)
                    {
                        // get child
                        BoardState nextBoardState = GetBoardStateAfterMove(boardState, cell, currentPlayerColor);

                        // set max value
                        eval = MiniMax(nextBoardState, oppositeColor, depth - 1, true, alpha, beta);
                        minValue = Math.Min(minValue, eval);

                        // set beta
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha)
                            break;
                    }
                    return minValue;
                }
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

        static int CalculateColorValue(BoardState boardState, Color color)
        {
            int result = 0;
            int size = boardState.Field.Length;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (boardState.Field[i, j] == color)
                        ++result;
                }
            }

            return result;
        }

        static float StaticEvaluationFunction(BoardState boardState, bool isLeaf = false)
        {
            if (isLeaf)
            {
                return CalculateColorValue(boardState, Color.Black) - CalculateColorValue(boardState, Color.White);
            } else
            {
                //Console.WriteLine("not a leaf");
                return (firstBestValue * (allowedCellsAmount - 1)) + (secondBestValue / allowedCellsAmount);
            }

        }

        static void MiniMaxRoot(Color currentPlayerColor)
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(currentBoardState, currentPlayerColor);
            SortedSet<Cell> allowedCells = cellsSearcher.GetAllAllowedCells();

            if (allowedCells.Count == 0)
            {
                //endOfGame
                return;
            }

            Cell moveCell = null;
            Color oppositeColor = (currentPlayerColor == Color.White) ? Color.Black : Color.White;

            float bestValue = startValueForBestMinValue;

            foreach (Cell cell in allowedCells)
            {
                BoardState nextBoardState = GetBoardStateAfterMove(currentBoardState, cell, currentPlayerColor);
                float eval = MiniMax(nextBoardState, currentPlayerColor, maxDepth, false, startValueForBestMaxValue, startValueForBestMinValue);

                if (bestValue > eval)
                {
                    bestValue = eval;
                    moveCell = cell;
                }
            }

            //apply the min cell
            model.PutChip(moveCell.X, moveCell.Y);

        }

        static void  SetNewChips(object sender, SetChipsEventArgs e)
        {
            currentBoardState.Field[e.NewChip.Cell.X, e.NewChip.Cell.Y] = e.NewChip.Color;

            for (int i = 0; i< e.ChangedChips.Count; i++)
            {
                currentBoardState.Field[e.ChangedChips[i].Cell.X, e.ChangedChips[i].Cell.Y] = e.ChangedChips[i].Color;
            }
        }

        static void SetStartBoard()
        {
            currentBoardState = new BoardState();
            currentBoardState.Field[3, 3] = Color.White;
            currentBoardState.Field[4, 4] = Color.White;
            currentBoardState.Field[3, 4] = Color.Black;
            currentBoardState.Field[4, 3] = Color.Black;
        }

        static void Main(string[] args)
        {
            model = new ReversiModel();

            //Random rand = new Random();
            Color currentColor = Color.Black;

            model.NewGame();
            model.NewGameStarted += (s, ea) => { SetStartBoard(); };
            model.SetChips += (s, ea) => { SetNewChips(s, ea); };
            model.SwitchMove += (s, ea) => { MiniMaxRoot(currentColor); };
        }
    }
}
