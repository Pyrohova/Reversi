using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace AIGenerator
{
    public class Generator
    {
        private const int MAX_DEPTH = 2;

        private BoardState currentBoardState;
        private ReversiModel model;
        private Cell blackHole;
        private Color currentColor;
        public bool GameIsOver { get; private set; }
        private Color? winnerColor;


        public Generator(ReversiModel currentModel)
        {
            model = currentModel;

            model.SetChips += (s, ea) => { SetNewChips(s, ea); };
            //model.SwitchMove += OnSwitchMove;
            model.GameOver += OnGameOver;
        }

        public void StartGame(Cell currentBlackHole, Color currentPlayerColor)
        {
            currentColor = currentPlayerColor;

            winnerColor = null;
            GameIsOver = false;
            blackHole = currentBlackHole;
            SetStartBoard();
            model.NewGame();
        }

        public void MakeMove()
        {
            if (GameIsOver)
            {
                if (winnerColor != currentColor)
                {
                    Console.WriteLine("pass");
                }
            }

            Cell moveCell = GetCellToMakeMove();

            if (GameIsOver)
            {
                return;
            }
            model.PutChip(moveCell.X, moveCell.Y);
            Console.WriteLine("{0}{1}", (char)('A' + moveCell.Y), (char)('1' + moveCell.X));
        }

        private void OnSwitchMove(object sender, SwitchMoveEventArgs eventArgs)
        {
            if (eventArgs.CurrentPlayerColor != currentColor)
            {
                return;
            }

            MakeMove();
        }

        private void OnGameOver(object sender, GameOverEventArgs eventArgs)
        {
            GameIsOver = true;
        }

        private float MiniMax(BoardState boardState, Color currentPlayerColor, int depth, bool maximizingPlayer, float alpha, float beta)
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(boardState, currentPlayerColor);
            SortedSet<Cell> allowedCells = cellsSearcher.GetAllAllowedCells();

            float staticEvaluation = StaticEvaluationFunction(boardState);

            bool theOnlyAllowedCellIsBlackHole = (allowedCells.Count == 1) && (allowedCells.Contains(blackHole));

            if (depth == 0 || allowedCells.Count == 0 || theOnlyAllowedCellIsBlackHole)
            {
                return staticEvaluation;
            }

            float eval;
            Color oppositeColor = (currentPlayerColor == Color.White) ? Color.Black : Color.White;

            int minMaxCoeff;

            if (maximizingPlayer)
            {
                minMaxCoeff = 1;
            }
            else
            {
                minMaxCoeff = -1;
            }

            float maxValue = 0;
            bool firstChildCalculated = false;

            // for each child of the current boardState
            foreach (Cell cell in allowedCells)
            {
                if (cell.CompareTo(blackHole) == 0)
                {
                    continue;
                }

                // get child
                BoardState nextBoardState = GetBoardStateAfterMove(boardState, cell, currentPlayerColor);

                eval = MiniMax(nextBoardState, oppositeColor, depth - 1, !maximizingPlayer, alpha, beta);
                eval *= minMaxCoeff;

                if (!firstChildCalculated)
                {
                    maxValue = eval;
                    firstChildCalculated = true;
                }
                else if (eval <= maxValue)
                {
                    continue;
                }

                maxValue = eval;

                if (maximizingPlayer)
                {
                    alpha = maxValue;
                }
                else
                {
                    beta = maxValue * -1;
                }

                if (beta <= alpha)
                    break;
            }

            return maxValue;
        }

        private BoardState GetBoardStateAfterMove(BoardState prevBoardState, Cell moveCell, Color moveColor)
        {
            BoardState nextBoardState = new BoardState();

            for (int i = 0; i < prevBoardState.FieldSize; i++)
            {
                for (int j = 0; j < prevBoardState.FieldSize; j++)
                {
                    nextBoardState.Field[i, j] = prevBoardState.Field[i, j];
                }
            }

            nextBoardState.Field[moveCell.X, moveCell.Y] = moveColor;

            ChangedChipsSearcher changedChipsSearcher = new ChangedChipsSearcher(prevBoardState, moveColor);
            List<Chip> changedChips = changedChipsSearcher.GetAllChangedChips(new Chip(moveColor, moveCell));

            foreach(Chip chip in changedChips)
            {
                nextBoardState.Field[chip.Cell.X, chip.Cell.Y] = chip.Color;
            }

            return nextBoardState;
        }

        private int CalculateColorValue(BoardState boardState, Color color)
        {
            int result = 0;

            for (int i = 0; i < boardState.FieldSize; i++)
            {
                for (int j = 0; j < boardState.FieldSize; j++)
                {
                    if (boardState.Field[i, j] == color)
                        ++result;
                }
            }

            return result;
        }

        private float StaticEvaluationFunction(BoardState boardState)
        {
            return CalculateColorValue(boardState, Color.Black) - CalculateColorValue(boardState, Color.White);
        }

        private Cell GetCellToMakeMove()
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(currentBoardState, currentColor);
            SortedSet<Cell> allowedCells = cellsSearcher.GetAllAllowedCells();

            bool theOnlyAllowedCellIsBlackHole = (allowedCells.Count == 1) && (allowedCells.Contains(blackHole));

            if (theOnlyAllowedCellIsBlackHole)
            {
                GameIsOver = true;
                Console.WriteLine("pass");
                return new Cell(0, 0);
            }

            Cell moveCell = null;

            float bestValue = 0;
            bool firstChildCalculated = false;

            bool ifMaximizeNextStep = (currentColor == Color.Black);

            foreach (Cell cell in allowedCells)
            {
                if (cell.CompareTo(blackHole) == 0)
                {
                    continue;
                }

                BoardState nextBoardState = GetBoardStateAfterMove(currentBoardState, cell, currentColor);

                float eval = MiniMax(nextBoardState, currentColor, MAX_DEPTH, ifMaximizeNextStep, int.MinValue, int.MaxValue);

                if (bestValue > eval || !firstChildCalculated)
                {
                    bestValue = eval;
                    moveCell = cell;
                }

                firstChildCalculated = true;
            }

            return moveCell;            
        }

        private void  SetNewChips(object sender, SetChipsEventArgs e)
        {
            currentBoardState.Field[e.NewChip.Cell.X, e.NewChip.Cell.Y] = e.NewChip.Color;

            for (int i = 0; i < e.ChangedChips.Count; i++)
            {
                currentBoardState.Field[e.ChangedChips[i].Cell.X, e.ChangedChips[i].Cell.Y] = e.ChangedChips[i].Color;
            }
        }

        private void SetStartBoard()
        {
            currentBoardState = new BoardState();
        }
    }
}
