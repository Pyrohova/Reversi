using ReversiCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Model;
using ReversiCore.Enums;

using ChipColor = ReversiCore.Enums.Color;

namespace Assets.Scripts.View
{
    public class HumanViewer : MonoBehaviour
    {
        [SerializeField] ReversiModelHolder holder;
        private ReversiModel model;

        private const int boardSize = 8;

        [SerializeField] GameObject cellProto;
        [SerializeField] GameObject allowedCellProto;
        [SerializeField] GameObject blackChip;
        [SerializeField] GameObject whiteChip;
        [SerializeField] GameObject cells;
        [SerializeField] GameObject chips;
        [SerializeField] GameObject cellColliders;

        [SerializeField] Text whiteScore;
        [SerializeField] Text blackScore;
        [SerializeField] Text infoField;
        [SerializeField] Text currentTurn;

        private GameObject[,] boardCells;
        private GameObject[,] existedChips;
        private GameObject[,] allowedCells;

        private GameMode currentMode;

        private ChipColor playerColor;

        private DelayRobotMoveTimer delayRobotMoveTimer;
        private SwitchMoveEventArgs lastDelayedSwitchMoveEventArgs;
        private SetChipsEventArgs lastDelayedSetChipsEventArgs;


        public void ClearAll()
        {
            foreach (GameObject existed in existedChips)
            {
                Destroy(existed);
            }
            ClearAllowedCells();

            whiteScore.text = "0";
            blackScore.text = "0";
            infoField.text = "";
            currentTurn.text = "";
        }

        private void AddChip(Chip newChip)
        {
            GameObject chipToCreate;

            //choose color
            if (newChip.Color == ChipColor.Black)
                chipToCreate = blackChip;
            else
                chipToCreate = whiteChip;

            existedChips[newChip.Cell.X, newChip.Cell.Y] = Instantiate(chipToCreate, boardCells[newChip.Cell.X,
                newChip.Cell.Y].transform.position, chipToCreate.transform.rotation);
            existedChips[newChip.Cell.X, newChip.Cell.Y].transform.SetParent(chips.transform);
            existedChips[newChip.Cell.X, newChip.Cell.Y].name = newChip.Cell.X + "" + newChip.Cell.Y;

        }

        private void RemoveChip(Chip chip)
        {
            Destroy(existedChips[chip.Cell.X, chip.Cell.Y]);
        }

        private void GenerateBoard()
        {
            float startX = -473, startY = 530, step = 135;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    boardCells[i, j] = Instantiate(cellProto, new Vector2(startX, startY), cellProto.transform.rotation);
                    boardCells[i, j].name = i + "" + j;
                    boardCells[i, j].transform.SetParent(cells.transform);

                    allowedCells[i, j] = Instantiate(allowedCellProto, new Vector2(startX, startY), cellProto.transform.rotation);
                    allowedCells[i, j].name = i + "" + j;
                    allowedCells[i, j].transform.SetParent(cellColliders.transform);
                    allowedCells[i, j].SetActive(false);

                    startX += step;

                    if (j == boardSize - 1)
                        startX = -473;
                }

                startY -= step;
            }
        }


        private void SwitchTurn(IEnumerable<Cell> allowedCells, ChipColor currentPlayerColor)
        {
            if (currentMode == GameMode.HumanToRobot)
            {
                if (currentPlayerColor == ChipColor.Black)
                    currentTurn.text = "White";
                else
                    currentTurn.text = "Black";
            }
            else
            {
                currentTurn.text = currentPlayerColor.ToString();
            }

            //actual only  if player vs  robot
            //if it's not player's turn, make delay for robot
            if ((currentPlayerColor != playerColor) && (currentMode == GameMode.HumanToRobot))
            {
                return;
            }

            // create new
            foreach (Cell allowedCell in allowedCells)
            {
                AllowCell(allowedCell);
            }
        }

        private void SwitchMoveConsideringUserType(object sender, SwitchMoveEventArgs e)
        {
            if (currentMode == GameMode.HumanToHuman || e.CurrentPlayerColor != playerColor || !delayRobotMoveTimer.IsRunning)
            {
                SwitchTurn(e.AllowedCells, e.CurrentPlayerColor);
                return;
            }

            lastDelayedSwitchMoveEventArgs = e;
        }

        // enable collider if this cell is allowed
        private void AllowCell(Cell cell)
        {
            allowedCells[cell.X, cell.Y].SetActive(true);
        }

        private void ClearAllowedCells()
        {
            foreach (GameObject cell in allowedCells)
            {
                cell.SetActive(false);
            }
        }

        private void CountChanged(object sender, CountChangedEventArgs e)
        {
            whiteScore.text = e.CountWhite.ToString();
            blackScore.text = e.CountBlack.ToString();
        }

        private void WrongMove(object sender, WrongMoveEventArgs e)
        {
            infoField.text = "wrong move";
            Debug.Log("wrong move on " + e.WrongChip.Cell.X + "" + e.WrongChip.Cell.Y);
        }

        private void NewGameStarted(object sender, NewGameEventArgs e)
        {
            ClearAll();
            if (e.NewGameMode == GameMode.HumanToHuman)
                infoField.text = "new game with second player started";
            else
            {
                infoField.text = "new game with robot started";
                playerColor = (ChipColor)e.UserPlayerColor;
            }
            currentMode = e.NewGameMode;

        }

        private void GameOver(object sender, GameOverEventArgs e)
        {
            ClearAllowedCells();

            if (e.WinnerColor == null)
                infoField.text = "played a draw!";
            else
                infoField.text = e.WinnerColor.ToString() + " won!";

        }

        private void SetChipsConsideringUserType(object sender, SetChipsEventArgs e)
        {
            if (currentMode == GameMode.HumanToHuman || e.NewChip.Color == playerColor || e.ChangedChips.Count == 0)
            {
                SetChips(e.NewChip, e.ChangedChips);
                return;
            }

            lastDelayedSetChipsEventArgs = e;
            delayRobotMoveTimer.Start(1f);
        }

        private void SetChips(Chip newChip, IEnumerable<Chip> changedChips)
        {
            AddChip(newChip);

            foreach (Chip chip in changedChips)
            {
                RemoveChip(chip);
                AddChip(chip);
            }
        }


        private void SubscribeOnEvents()
        {
            model.NewGameStarted += NewGameStarted;
            model.WrongMove += WrongMove;
            model.SetChips += SetChipsConsideringUserType;
            model.SetChips += (s, ea) => { ClearAllowedCells(); };
            model.GameOver += GameOver;
            model.SwitchMove += SwitchMoveConsideringUserType;
            model.CountChanged += CountChanged;

        }


        void Start()
        {
            delayRobotMoveTimer = new DelayRobotMoveTimer();

            boardCells = new GameObject[boardSize, boardSize];
            existedChips = new GameObject[boardSize, boardSize];
            allowedCells = new GameObject[boardSize, boardSize];

            GenerateBoard();

            ClearAll();

            model = holder.reversiModel;
            SubscribeOnEvents();
        }

        void Update()
        {
            if (!delayRobotMoveTimer.IsRunning)
            {
                return;
            }

            delayRobotMoveTimer.Increase(Time.deltaTime);

            if (delayRobotMoveTimer.HasReachedMaxTime)
            {
                delayRobotMoveTimer.Stop();
                SetChips(lastDelayedSetChipsEventArgs.NewChip, lastDelayedSetChipsEventArgs.ChangedChips);
                SwitchTurn(lastDelayedSwitchMoveEventArgs.AllowedCells, lastDelayedSwitchMoveEventArgs.CurrentPlayerColor);
            }
        }
    }
}