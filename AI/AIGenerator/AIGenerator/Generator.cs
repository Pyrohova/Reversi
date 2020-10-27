using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace AIGenerator
{
    public class Generator
    {
        private const int MAX_DEPTH = 5;

        private BoardState currentBoardState;
        private ReversiModel model;
        private Cell blackHole;
        private Color currentColor;
        private float[,] evalMatrix = new float[8, 8]{
                        {20, -3, 11, 8,  8, 11, -3, 20},
                        {-3, -7, -4, 1,  1, -4, -7, -3},
                        { 11, -4, 2,  2,  2,  2, -4, 11},
                        { 8,  1, 2, -3, -3,  2,  1,  8},
                        { 8,  1, 2, -3, -3,  2,  1,  8},
                        { 11, -4, 2,  2,  2,  2, -4, 11},
                        { -3, -7, -4, 1,  1, -4, -7, -3},
                        { 20, -3, 11, 8,  8, 11, -3, 20}
            };
        public bool GameIsOver { get; private set; }


        public Generator(ReversiModel currentModel)
        {
            model = currentModel;

            model.SetChips += (s, ea) => { SetNewChips(s, ea); };
            model.SwitchMove += OnSwitchMove;
            model.GameOver += OnGameOver;
            //model.WrongMove += (s, ea) => { Console.WriteLine("===================== {0}", ea.WrongChip.Color); };
        }

        public void StartGame(Cell currentBlackHole, Color currentPlayerColor)
        {
            currentColor = currentPlayerColor;

            GameIsOver = false;
            blackHole = currentBlackHole;
            SetStartBoard();
            model.NewGame();
        }

        public void MakeMove()
        {
            GameIsOver = false;

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
            //Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^ {0}", eventArgs.CurrentPlayerColor);
            if (eventArgs.CurrentPlayerColor == currentColor)
            {
                GameIsOver = false;
            }
        }

        private void OnGameOver(object sender, GameOverEventArgs eventArgs)
        {
            GameIsOver = true;
        }

        private float MiniMax(BoardState boardState, Color currentPlayerColor, int depth, bool maximizingPlayer, float alpha, float beta)
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(boardState, currentPlayerColor);
            SortedSet<Cell> allowedCells = cellsSearcher.GetAllAllowedCells();

            Color oppositeColor = (currentPlayerColor == Color.White) ? Color.Black : Color.White;

            if (depth == 0)
            {
                return StaticEvaluationFunction(boardState);
            }

            bool theOnlyAllowedCellIsBlackHole = (allowedCells.Count == 1) && (allowedCells.Contains(blackHole));

            if (theOnlyAllowedCellIsBlackHole || allowedCells.Count == 0)
            {
                return MiniMax(boardState, oppositeColor, depth - 1, !maximizingPlayer, alpha, beta);
            }

            float eval;

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

        private float StaticEvaluationFunction(BoardState boardState)
        {
            int blackCounter = 0;
            int whiteCounter = 0;

            float blackScore = 0;
            float whiteScore = 0;

            for (int i = 0; i < boardState.FieldSize; i++)
            {
                for (int j = 0; j < boardState.FieldSize; j++)
                {
                    if (boardState.Field[i, j] == Color.Black)
                    {
                        ++blackCounter;
                        blackScore += evalMatrix[i, j];
                    }
                    else if (boardState.Field[i, j] == Color.White)
                    {
                        ++whiteCounter;
                        whiteScore += evalMatrix[i, j];
                    }
                }
            }
            return (blackScore / blackCounter) - (whiteScore / whiteCounter);
            /*float distanceTotal = 0;

            for (var i = 0; i < 8; i++)
                for (var j = 0; j < 8; j++)
                    if (boardState.Field[i, j] == Color.Black)
                        distanceTotal += DistSquared(i - 4.5f, j - 4.5f);
                    else if (boardState.Field[i, j] == Color.White)
                        distanceTotal -= DistSquared(i - 4.5f, j - 4.5f);

            return distanceTotal;*/
        }

        private static float DistSquared(float x, float y)
        {
            return x * x + y * y;
        }

        private Cell GetCellToMakeMove()
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(currentBoardState, currentColor);
            SortedSet<Cell> allowedCells = cellsSearcher.GetAllAllowedCells();

            /*foreach(Cell cell in allowedCells)
            {
                Console.WriteLine("{0} {1}", cell.X, cell.Y);
            }
            Console.WriteLine();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (currentBoardState.Field[i, j] != null)
                        Console.Write("{0} ", currentBoardState.Field[i, j].ToString()[0]);
                    else
                        Console.Write("_ ");
                }
                Console.WriteLine();
            }*/

            bool ifMaximizeNextStep = (currentColor == Color.White);

            bool theOnlyAllowedCellIsBlackHole = (allowedCells.Count == 1) && (allowedCells.Contains(blackHole));

            if (allowedCells.Count == 0 || theOnlyAllowedCellIsBlackHole)
            {
                //if (MiniMax(currentBoardState, currentColor, MAX_DEPTH, ifMaximizeNextStep, int.MinValue, int.MaxValue) == float.MinValue)
                //{
                    GameIsOver = true;
                    model.Pass(currentColor);
                    Console.WriteLine("pass");
                    return new Cell(0, 0);
                //}
            }

            Cell moveCell = null;

            float bestValue = 0;
            bool firstChildCalculated = false;

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
