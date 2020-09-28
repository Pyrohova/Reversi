using ReversiCore.Enums;
using ReversiCore.Interfaces;
using System;
using System.Collections.Generic;

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
        public event EventHandler<WrongMoveEventArgs> WrongMove;
        public event EventHandler<CountChangedEventArgs> CountChanged;
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<RobotColorSetEventArgs> RobotColorSet;
        public Dictionary<Color, EventHandler<SwitchMoveEventArgs>> SwitchMove { get; set; }
        
        public ReversiModel()
        {
            turnHolder = new TurnHolder();
            board = new Board();
            currentAllowedCells = new SortedSet<Cell>();
            countHolder = new CountHolder();

            SwitchMove = new Dictionary<Color, EventHandler<SwitchMoveEventArgs>>();
            SwitchMove[Color.White] = null;
            SwitchMove[Color.Black] = null;
        }

        public void NewGame(GameMode newGameMode, Color? userPlayerColor = null)
        {
            board.Clear();
            turnHolder.Reset();

            if (newGameMode == GameMode.HumanToRobot)
            {
                SetRobotColor(userPlayerColor);
            }
            
            NewGameStarted?.Invoke(this, new NewGameEventArgs{ NewGameMode = newGameMode });

            countHolder.Reset();

            CountChanged?.Invoke(this, new CountChangedEventArgs
            {
                CountWhite = countHolder.GetPlayerCount(Color.White),
                CountBlack = countHolder.GetPlayerCount(Color.Black),
            });

            CalculateAllowedCells();

            SwitchMove[turnHolder.CurrentTurnColor]?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowedCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
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
            
            SwitchMove[turnHolder.CurrentTurnColor]?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowedCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
        }

        private void SetStartPosition()
        {
            //TODO убрать дублирование с Board
            List<Chip> startChips = new List<Chip>()
            {
                new Chip(Color.White, new Cell(3, 3)),
                new Chip(Color.White, new Cell(4, 4)),
                new Chip(Color.Black, new Cell(3, 4)),
                new Chip(Color.Black, new Cell(4, 3))
            };

            foreach(Chip chip in startChips)
            {
                SetChips?.Invoke(this, new SetChipsEventArgs { NewChip = chip, ChangedChips = new List<Chip>() });
            }
        }

        private void SetRobotColor(Color? userPlayerColor)
        {
            if (userPlayerColor == null)
            {
                //TODO
            }

            Color robotColor = Color.White;

            if (userPlayerColor == Color.White)
            {
                robotColor = Color.Black;
            }

            RobotColorSet?.Invoke(this, new RobotColorSetEventArgs { RobotColor = robotColor });
        }

        private bool NewChipAllowed(Chip chip)
        {
            if (chip.Color != turnHolder.CurrentTurnColor || !currentAllowedCells.Contains(chip.Cell))
            {
                WrongMove?.Invoke(this, new WrongMoveEventArgs { WrongChip = chip });
                return false;
            }

            return true;
        }

        private void CalculateAllowedCells()
        {
            SortedSet<Cell> allowedCells = board.GetAllowedCells(turnHolder.CurrentTurnColor);

            if (allowedCells.Count == 0)
            {
                EndGame();
                return;
            }

            currentAllowedCells = allowedCells;
        }

        private void EndGame()
        {
            GameOver?.Invoke(this, new GameOverEventArgs { WinnerColor = countHolder.GetWinner() });
        }
    }
}
