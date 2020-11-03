using ReversiCore;
using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIGenerator
{
    public class Generator
    {
        private BoardState currentBoardState;
        private ReversiModel model;
        private Cell blackHole;
        private Color currentColor; //color of this AI in current game
        private Color oppositeColor; //color of an opponent in current game
        private const int RUNS_Count = 20;
        private Random rand;
        public bool GameIsOver { get; private set; }

        public Generator(ReversiModel currentModel)
        {
            model = currentModel;
            rand = new Random();

            model.SetChips += (s, ea) => { SetNewChips(s, ea); };
            model.SwitchMove += OnSwitchMove;
            model.GameOver += OnGameOver;
        }


        /*
         * Starts new game
         * --------------------------------------------
         * currentBlackHole - black hole of current game
         * currentPlayerColor - color of this AI in current game
         */
        public void StartGame(Cell currentBlackHole, Color currentPlayerColor)
        {
            currentColor = currentPlayerColor;
            oppositeColor = (currentColor == Color.Black) ? Color.White : Color.Black;

            GameIsOver = false;
            blackHole = currentBlackHole;
            SetStartBoard();
            model.NewGame();
        }


        /*
         * Makes next move if it is possible
         */
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


        /*
         * Method that is called when SwitchOver event of the model is invoked
         */
        private void OnSwitchMove(object sender, SwitchMoveEventArgs eventArgs)
        {
            if (eventArgs.CurrentPlayerColor == currentColor)
            {
                GameIsOver = false;
            }
        }


        /*
         * Method that is called when GameOver event of the model is invoked
         */
        private void OnGameOver(object sender, GameOverEventArgs eventArgs)
        {
            GameIsOver = true;
        }


        /* Returns color of winner in randomly generated game
         * --------------------------------------------
         * boardState - board state from which game continues
         * currentPlayerColor - color of player that makes current move
         * passesCount - count of passes made in a row during the current game
         */
        private Color? GetRandomWinner(BoardState boardState, Color currentPlayerColor, int passesCount)
        {
            Color oppositePlayerColor = (currentPlayerColor == Color.White) ? Color.Black : Color.White;

            if (passesCount == 2)
            {
                return GetCurrentWinner(boardState);
            }

            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(boardState, currentPlayerColor);
            SortedSet<Cell> allowedCellsSet = cellsSearcher.GetAllAllowedCells();
            allowedCellsSet.Remove(blackHole);
            List<Cell> allowedCells = allowedCellsSet.ToList();

            if (allowedCells.Count == 0)
            {
                return GetRandomWinner(boardState, oppositePlayerColor, passesCount + 1);
            }

            int moveNumber = rand.Next(allowedCells.Count);

            MakeMoveIntoBoardState(boardState, allowedCells[moveNumber], currentPlayerColor);

            return GetRandomWinner(boardState, oppositePlayerColor, 0);
        }


        /* Returns color of winner on given board state (or returns null in case of a draw)
         * --------------------------------------------
         * boardState - board state to calculate winner on
         */
        private Color? GetCurrentWinner(BoardState boardState)
        {
            int blackCount = CountChipsForPlayer(boardState, Color.Black);
            int whiteCount = CountChipsForPlayer(boardState, Color.White);


            if (blackCount < whiteCount)
            {
                return Color.Black;
            }
            else if (blackCount > whiteCount)
            {
                return Color.White;
            }
            else
            {
                return null;
            }
        }


        /*
         * Makes move of given color into given cell of given board state
         * --------------------------------------------
         * boardState - board state to make move into
         * moveCell - cell to make move into
         * moveColor - color of chip to make move with
         */
        private void MakeMoveIntoBoardState(BoardState boardState, Cell moveCell, Color moveColor)
        {
            boardState.Field[moveCell.X, moveCell.Y] = moveColor;

            ChangedChipsSearcher changedChipsSearcher = new ChangedChipsSearcher(boardState, moveColor);
            List<Chip> changedChips = changedChipsSearcher.GetAllChangedChips(new Chip(moveColor, moveCell));

            foreach(Chip chip in changedChips)
            {
                boardState.Field[chip.Cell.X, chip.Cell.Y] = chip.Color;
            }
        }


        /*
         * Returns deep copy of given board state
         * --------------------------------------------
         * boardState - board state to copy
         */
        private BoardState GetBoardStateCopy(BoardState boardState)
        {
            BoardState boardStateCopy = new BoardState();

            for (int i = 0; i < boardState.FieldSize; i++)
            {
                for (int j = 0; j < boardState.FieldSize; j++)
                {
                    boardStateCopy.Field[i, j] = boardState.Field[i, j];
                }
            }

            return boardStateCopy;
        }


        /*
         * Returns count of chips of particular color on given board state
         * --------------------------------------------
         * board - given board state 
         * player - color of chips to count
         */
        private int CountChipsForPlayer(BoardState board, Color? player)
        {
            int count = 0;

            for (int i = 0; i < board.FieldSize; i++)
            {
                for (int j = 0; j < board.FieldSize; j++)
                {
                    if (board.Field[i, j] == player)
                    {
                        count++;
                    }
                }
            }

            return count;
        }


        /*
         * Returns the best cell to make next move into
         */
        private Cell GetCellToMakeMove()
        {
            AllowedCellsSearcher cellsSearcher = new AllowedCellsSearcher(currentBoardState, currentColor);
            SortedSet<Cell> allowedCellsSet = cellsSearcher.GetAllAllowedCells();
            allowedCellsSet.Remove(blackHole);
            List<Cell> allowedCells = allowedCellsSet.ToList();

            if (allowedCells.Count == 0)
            {
                GameIsOver = true;
                model.Pass(currentColor);
                Console.WriteLine("pass");
                return new Cell(0, 0);
            }

            int bestWinRate = 0;
            int bestWinRateInd = 0;

            for (int j = 0; j < allowedCells.Count; j++)
            {
                int currentWinCount = CountRandomWinsFromMove(allowedCells[j]);
                if (currentWinCount > bestWinRate)
                {
                    bestWinRate = currentWinCount;
                    bestWinRateInd = j;
                }
            }

            return allowedCells[bestWinRateInd];
        }


        /*
         * Returns count of random wins after making given first move into current board state
         * --------------------------------------------
         * move - cell to make first move into
         */
        private int CountRandomWinsFromMove(Cell move)
        {
            int winsCount = 0;

            BoardState nextBoardState = GetBoardStateCopy(currentBoardState);
            MakeMoveIntoBoardState(nextBoardState, move, currentColor);

            Task[] runTasks = new Task[RUNS_Count];
            object lockObj = new object();

            for (int i = 0; i < RUNS_Count; i++)
            {
                BoardState nextBoardStateCopy = GetBoardStateCopy(nextBoardState);
                runTasks[i] = new Task(() =>
                {
                    Color? winner = GetRandomWinner(nextBoardStateCopy, oppositeColor, 0);

                    if (winner == currentColor)
                    {
                        lock (lockObj)
                        {
                            winsCount++;
                        }
                    }
                }
                );
                runTasks[i].Start();
            }

            Task.WaitAll(runTasks);
            return winsCount;
        }


        /*
         * Sets chips on board (called when SetChips event of the model is invoked)
         */
        private void SetNewChips(object sender, SetChipsEventArgs e)
        {
            currentBoardState.Field[e.NewChip.Cell.X, e.NewChip.Cell.Y] = e.NewChip.Color;

            for (int i = 0; i < e.ChangedChips.Count; i++)
            {
                currentBoardState.Field[e.ChangedChips[i].Cell.X, e.ChangedChips[i].Cell.Y] = e.ChangedChips[i].Color;
            }
        }


        /*
         * Sets board into state of start of a game
         */
        private void SetStartBoard()
        {
            currentBoardState = new BoardState();
        }
    }
}
