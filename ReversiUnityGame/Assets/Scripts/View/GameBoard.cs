using ReversiCore;
using System.Collections.Generic;
using UnityEngine;

using ChipColor = ReversiCore.Enums.Color;

namespace Assets.Scripts.View
{

    public class GameBoard : MonoBehaviour
    {
        private const int boardSize = 8;

        [SerializeField] GameObject cellProto; //prototype of board cell
        [SerializeField] GameObject allowedCellProto; //prototype of board cell collider(collider for allowed cells)
        [SerializeField] GameObject blackChip; //prototype of black chip
        [SerializeField] GameObject whiteChip; // prototype of white chip
        [SerializeField] GameObject cells;  //parent of all cells that would be generated
        [SerializeField] GameObject chips;  // parent of all chips that would be added
        [SerializeField] GameObject cellColliders; //parent of all board cell colliders (colliders for allowed cells)

        private GameObject[,] boardCells;
        private GameObject[,] existedChips;
        private GameObject[,] allowedCells;

        public void ClearAll()
        {
            foreach (GameObject existed in existedChips)
            {
                Destroy(existed);
            }
            ClearAllowedCells();
        }

        public void AddChip(Chip newChip)
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

        public void RemoveChip(Chip chip)
        {
            Destroy(existedChips[chip.Cell.X, chip.Cell.Y]);
        }

        // enable to press on allowed colliders
        public void AllowCells(IEnumerable<Cell> allowedCells)
        {
            foreach (Cell allowedCell in allowedCells)
            {
                this.allowedCells[allowedCell.X, allowedCell.Y].SetActive(true);
            }
        }

        public void ClearAllowedCells()
        {
            //disable to press on all colliders
            foreach (GameObject cell in allowedCells)
            {
                cell.SetActive(false);
            }
        }

        //respawn every chip of the opposite color 
        public void ReplaceChipsColor(IEnumerable<Chip> changedChips)
        {
            foreach (Chip chip in changedChips)
            {
                RemoveChip(chip);
                AddChip(chip);
            }
        }

        public void GenerateBoard()
        {
            float startX = -473, startY = 530, step = 135;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    // generate board cells
                    boardCells[i, j] = Instantiate(cellProto, new Vector2(startX, startY), cellProto.transform.rotation);
                    boardCells[i, j].name = i + "" + j;
                    boardCells[i, j].transform.SetParent(cells.transform);

                    //generate colliders
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

        void Start()
        {
            boardCells = new GameObject[boardSize, boardSize];
            existedChips = new GameObject[boardSize, boardSize];
            allowedCells = new GameObject[boardSize, boardSize];

            GenerateBoard();
        }

    }
}
