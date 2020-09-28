using ReversiCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Model;
using ReversiCore.Enums;
using System;

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

        private ReversiCore.Enums.Color playerColor;

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
            //if cell border exist(what means cell is allowed) then add new chip
            //if (allowedCells[newChip.Cell.X, newChip.Cell.X] != null)
            //{
                GameObject chipToCreate;

                if (newChip.Color == ReversiCore.Enums.Color.Black)
                    chipToCreate = blackChip;
                else
                    chipToCreate = whiteChip;

                existedChips[newChip.Cell.X, newChip.Cell.Y] = Instantiate(chipToCreate, boardCells[newChip.Cell.X,
                    newChip.Cell.Y].transform.position, chipToCreate.transform.rotation);
                existedChips[newChip.Cell.X, newChip.Cell.Y].transform.SetParent(chips.transform);
                existedChips[newChip.Cell.X, newChip.Cell.Y].name = newChip.Cell.X + "" + newChip.Cell.Y;

            //}
            //else
            //{
            //    infoField.text = "wrong move, try again";
            //}

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
                    startX += step;
                    if (j == boardSize - 1)
                        startX = -473;
                }

                startY -= step;
            }
        }


        private void SwitchTurn(object sender, SwitchMoveEventArgs e)
        {
            Debug.Log(e.CurrentPlayerColor);
            // remove previous allowed cells
            ClearAllowedCells();
            // create new
            foreach (Cell allowedCell in e.AllowedCells)
            {
                Debug.Log(allowedCell.X + "" + allowedCell.Y);
                AllowCell(allowedCell);
            }

            //change turn
            playerColor = e.CurrentPlayerColor;
            currentTurn.text = e.CurrentPlayerColor.ToString();

        }

        // create cell collider if this cell is allowed
        private void AllowCell(Cell cell)
        {
            allowedCells[cell.X, cell.Y] = Instantiate(allowedCellProto, boardCells[cell.X,
                cell.Y].transform.position, allowedCellProto.transform.rotation);
            allowedCells[cell.X, cell.Y].transform.SetParent(cellColliders.transform);
            allowedCells[cell.X, cell.Y].transform.name = cell.X + "" + cell.Y;
        }

        private void ClearAllowedCells()
        {
            foreach (GameObject cell in allowedCells)
            {
                Destroy(cell);
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
            Debug.Log(e.WrongChip.Cell.X + "" + e.WrongChip.Cell.Y);
        }

        private void NewGameStarted(object sender, NewGameEventArgs e)
        {
            ClearAll();
            if (e.NewGameMode == GameMode.HumanToHuman)
                infoField.text = "new game with second player started";
            else
                infoField.text = "new game with robot started";
            currentMode = e.NewGameMode;
        }

        private void GameOver(object sender, GameOverEventArgs e)
        {
            // TO DO
            // disable cells wneh game is over so user can only press start new game
            if (e.WinnerColor == null)
                infoField.text = "played a draw!";
            else
                infoField.text = e.WinnerColor.ToString() + " won!";

        }

        private void SetChips(object sender, SetChipsEventArgs e)
        {
            Debug.Log(e.NewChip.Color.ToString());

            AddChip(e.NewChip);

            foreach (Chip chip in e.ChangedChips)
            {
                RemoveChip(chip);
                AddChip(chip);
            }
        }

        private void SubscribeOnEvents()
        {
            model.CountChanged += CountChanged;
            model.NewGameStarted += NewGameStarted;
            model.GameOver += GameOver;
            model.SetChips += SetChips;

            if (currentMode == GameMode.HumanToHuman)
            {
                model.SwitchMove[ReversiCore.Enums.Color.Black] += SwitchTurn;
                model.SwitchMove[ReversiCore.Enums.Color.White] += SwitchTurn;
            } else
            {
                model.SwitchMove[playerColor] += SwitchTurn;
                // subscribe on switch move [opposite color] by method where {startcorutine()}
            }

            model.WrongMove += WrongMove;

        }
        // Start is called before the first frame update
        void Start()
        {
            model = holder.reversiModel;
            boardCells = new GameObject[boardSize, boardSize];
            existedChips = new GameObject[boardSize, boardSize];
            allowedCells = new GameObject[boardSize, boardSize];

            GenerateBoard();

            //ClearAll();
            playerColor = ReversiCore.Enums.Color.Black;
            SubscribeOnEvents();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}