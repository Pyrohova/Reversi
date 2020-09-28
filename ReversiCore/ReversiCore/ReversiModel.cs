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
        private SortedSet<Cell> currentAllowerCells;

        public event EventHandler<NewGameEventArgs> NewGameStarted;
        public event EventHandler<SetChipsEventArgs> SetChips;
        public event EventHandler<WrongMoveEventArgs> WrongMove;
        public event EventHandler<CountChangedEventArgs> CountChanged;
        public event EventHandler<GameOverEventArgs> GameOver;
        public Dictionary<Color, EventHandler<SwitchMoveEventArgs>> SwitchMove { get; set; }
        
        public ReversiModel()
        {
            turnHolder = new TurnHolder();
            board = new Board();
            currentAllowerCells = new SortedSet<Cell>();
            countHolder = new CountHolder();
        }

        public void NewGame(GameMode newGameMode)
        {
            board.Clear();
            turnHolder.Reset();
            
            NewGameStarted?.Invoke(this, new NewGameEventArgs{ NewGameMode = newGameMode });

            countHolder.Reset();

            CountChanged?.Invoke(this, new CountChangedEventArgs
            {
                CountWhite = countHolder.GetPlayerCount(Color.White),
                CountBlack = countHolder.GetPlayerCount(Color.Black),
            });

            CalculateAllowedCells();

            SwitchMove[turnHolder.CurrentTurnColor]?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowerCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
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
            
            SwitchMove[turnHolder.CurrentTurnColor]?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowerCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
        }

        private bool NewChipAllowed(Chip chip)
        {
            if (chip.Color != turnHolder.CurrentTurnColor || !currentAllowerCells.Contains(chip.Cell))
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

            currentAllowerCells = allowedCells;
        }

        private void EndGame()
        {
            GameOver?.Invoke(this, new GameOverEventArgs { WinnerColor = countHolder.GetWinner() });
        }
    }
}
