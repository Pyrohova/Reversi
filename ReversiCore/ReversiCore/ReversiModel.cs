using ReversiCore.Enums;
using ReversiCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversiCore
{
    public class ReversiModel : IReversiModel
    {
        private TurnHolder turnHolder;
        private Board board;
        private CountHolder countHolder;
        private SortedSet<Cell> currentAllowedCells;

        public event EventHandler<NewGameEventArgs> NewGameStarted;
        public event EventHandler<SetChipsEventArgs> SetChips;
        public event EventHandler<SwitchMoveEventArgs> SwitchMove;
        public event EventHandler<WrongMoveEventArgs> WrongMove;
        public event EventHandler<CountChangedEventArgs> CountChanged;
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<RobotColorSetEventArgs> RobotColorSet;
        public event EventHandler RobotDisabled;
        
        public ReversiModel()
        {
            turnHolder = new TurnHolder();
            board = new Board();
            currentAllowedCells = new SortedSet<Cell>();
            countHolder = new CountHolder();
        }

        public void NewGame(GameMode newGameMode, Color? userPlayerColor = null)
        {
            turnHolder.Reset();

            if (newGameMode == GameMode.HumanToRobot)
            {
                if (userPlayerColor == null)
                {
                    userPlayerColor = turnHolder.FirstTurnColor;
                }

                SetRobotColor(userPlayerColor);
            }
            else
            {
                RobotDisabled?.Invoke(this, new EventArgs());
            }
            
            NewGameStarted?.Invoke(this, new NewGameEventArgs{ NewGameMode = newGameMode, UserPlayerColor = userPlayerColor });

            SetStartBoardPosition();

            countHolder.Reset();

            CountChanged?.Invoke(this, new CountChangedEventArgs
            {
                CountWhite = countHolder.GetPlayerCount(Color.White),
                CountBlack = countHolder.GetPlayerCount(Color.Black),
            });

            CalculateAllowedCells();

            if (currentAllowedCells.Count == 0)
            {
                return;
            }

            SwitchMove?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowedCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
        }
        
        public void PutChip(int x, int y)
        {
            Chip newChip = new Chip(turnHolder.CurrentTurnColor, new Cell(x, y));

            if (!NewChipAllowed(newChip))
            {
                return;
            }

            board.AddChip(newChip);

            List<Chip> changedChips = board.GetChangedChips(newChip, turnHolder.CurrentTurnColor);
            countHolder.Increase(turnHolder.CurrentTurnColor, changedChips.Count + 1);
            countHolder.Decrease(turnHolder.OppositeTurnColor, changedChips.Count);

            SetChips?.Invoke(this, new SetChipsEventArgs { NewChip = newChip, ChangedChips = changedChips });
            CountChanged?.Invoke(this, new CountChangedEventArgs
            {
                CountWhite = countHolder.GetPlayerCount(Color.White),
                CountBlack = countHolder.GetPlayerCount(Color.Black),
            });

            turnHolder.Switch();

            CalculateAllowedCells();

            if (currentAllowedCells.Count == 0)
            {
                return;
            }
            
            SwitchMove?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowedCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
        }

        private void SwitchTurn()
        {
            //TODO put here duplicate from NewGame and PutChip
        }

        private void SetStartBoardPosition()
        {
            board.SetStartPosition();

            foreach(Chip chip in board.StartChips)
            {
                SetChips?.Invoke(this, new SetChipsEventArgs { NewChip = chip, ChangedChips = new List<Chip>() });
            }
        }

        private void SetRobotColor(Color? userPlayerColor)
        {
            Color robotColor = Color.White;

            if (userPlayerColor == Color.White)
            {
                robotColor = Color.Black;
            }

            RobotColorSet?.Invoke(this, new RobotColorSetEventArgs { RobotColor = robotColor });
        }

        private bool NewChipAllowed(Chip chip)
        {
            if (chip.Color != turnHolder.CurrentTurnColor || !CellIsAllowed(chip.Cell))
            {
                WrongMove?.Invoke(this, new WrongMoveEventArgs { WrongChip = chip });
                return false;
            }

            return true;
        }

        private bool CellIsAllowed(Cell cell)
        {
            foreach (Cell curAllowedCell in currentAllowedCells)
            {
                if (cell.Equals(curAllowedCell))
                {
                    return true;
                }
            }

            return false;
        }

        private void CalculateAllowedCells()
        {
            currentAllowedCells = board.GetAllowedCells(turnHolder.CurrentTurnColor);

            if (currentAllowedCells.Count == 0)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            GameOver?.Invoke(this, new GameOverEventArgs { WinnerColor = countHolder.GetWinner() });
        }
    }
}
