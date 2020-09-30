using ReversiCore;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Model;
using ReversiCore.Enums;
using System;

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

        private float timer;
        private float timerWait = 5f;
        private bool timerIsActive;

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

                    allowedCells[i,j] = Instantiate(allowedCellProto, new Vector2(startX, startY), cellProto.transform.rotation);
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


        private void SwitchTurn(object sender, SwitchMoveEventArgs e)
        {
            if (currentMode == GameMode.HumanToRobot)
            {
                if (e.CurrentPlayerColor == ChipColor.Black)
                    currentTurn.text = "White";
                else
                    currentTurn.text = "Black";
            } else
            {
                currentTurn.text = e.CurrentPlayerColor.ToString();
            }

            //actual only  if player vs  robot
            //if it's not player's turn, make delay for robot
            if ( (e.CurrentPlayerColor != playerColor) && (currentMode == GameMode.HumanToRobot) )
            {
                //DelayRobotTurn();
                timerIsActive = true;
                return;
            }

            // remove previous allowed cells
            ClearAllowedCells();

            // create new
            foreach (Cell allowedCell in e.AllowedCells)
            {
                AllowCell(allowedCell);
            }

        }

        private void DelayRobotTurn()
        {
            Debug.Log("called");
            timer = timerWait;
            while(timer > 0)
            {
                Debug.Log(timer);
                //artificial delay
            }

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

        private void SetChips(object sender, SetChipsEventArgs e)
        {
            AddChip(e.NewChip);

            foreach (Chip chip in e.ChangedChips)
            {
                RemoveChip(chip);
                AddChip(chip);
            }
        }


        private void SubscribeOnEvents()
        {
            model.NewGameStarted += NewGameStarted;
            model.WrongMove += WrongMove;
            model.SetChips += SetChips;
            model.GameOver += GameOver;
            model.SwitchMove += SwitchTurn;
            model.CountChanged += CountChanged;


        }

        private void CheckTimer()
        {
            if (timerIsActive)
            {
                timer = timerWait;
                if (timer < 0)
                    timerIsActive = false;
            }
        }

        void Start()
        {
            boardCells = new GameObject[boardSize, boardSize];
            existedChips = new GameObject[boardSize, boardSize];
            allowedCells = new GameObject[boardSize, boardSize];

            GenerateBoard();

            ClearAll();

            model = holder.reversiModel;
            SubscribeOnEvents();

            timer = 100f;
            timerIsActive = false;
        }

        void Update()
        {
            CheckTimer();
            timer -= Time.deltaTime;
            //if (timer < -100f)
            //    timer = 100f;
        }
    }
}