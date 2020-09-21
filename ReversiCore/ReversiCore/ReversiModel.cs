using ReversiCore.Enums;
using ReversiCore.Interfaces;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class ReversiModel : IReversiModel
    {
        private TurnHolder turnHolder;
        private BoardManipulator boardManipulator;
        private CountHolder countHolder;
        private SortedSet<Cell> currentAllowerCells;

        public event EventHandler<NewGameEventArgs> NewGameStarted;
        public event EventHandler<SetChipsEventArgs> SetChips;
        public event EventHandler<WrongMoveEventArgs> WrongMove;
        public event EventHandler<CountChangedEventArgs> CountChanged;
        public event EventHandler<SwitchMoveEventArgs> SwitchMove;
        public event EventHandler<GameOverEventArgs> GameOver;

        public ReversiModel()
        {
            turnHolder = new TurnHolder();
            boardManipulator = new BoardManipulator();
            currentAllowerCells = new SortedSet<Cell>();
            countHolder = new CountHolder();
        }

        public void NewGame(GameMode newGameMode)
        {
            boardManipulator.Clear();
            turnHolder.Reset();
            
            NewGameStarted?.Invoke(this, new NewGameEventArgs{ NewGameMode = newGameMode });

            countHolder.Reset();

            CountChanged?.Invoke(this, new CountChangedEventArgs
            {
                CountWhite = countHolder.GetPlayerCount(Color.White),
                CountBlack = countHolder.GetPlayerCount(Color.Black),
            });

            CalculateAllowedCells();

            SwitchMove?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowerCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
        }
        
        public void PutChip(int x, int y)
        {
            Chip newChip = new Chip(turnHolder.CurrentTurnColor, new Cell(x, y));
            CheckNewChip(newChip);

            boardManipulator.AddChip(newChip);

            List<Chip> changedChips = boardManipulator.GetChangedChips(newChip, turnHolder.CurrentTurnColor);
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
            
            SwitchMove?.Invoke(this, new SwitchMoveEventArgs { AllowedCells = currentAllowerCells, CurrentPlayerColor = turnHolder.CurrentTurnColor });
        }

        private void CheckNewChip(Chip chip)
        {
            if (chip.Color != turnHolder.CurrentTurnColor || !currentAllowerCells.Contains(chip.Cell))
            {
                WrongMove?.Invoke(this, new WrongMoveEventArgs { WrongChip = chip });
                return;
            }
        }

        private void CalculateAllowedCells()
        {
            SortedSet<Cell> allowedCells = boardManipulator.GetAllowedCells(turnHolder.CurrentTurnColor);

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
