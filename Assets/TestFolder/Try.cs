using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Try : MonoBehaviour
{
    private const int boardSize = 8;
    [SerializeField] GameObject cell;
    [SerializeField] GameObject blackChip;
    [SerializeField] GameObject whiteChip;
    [SerializeField] GameObject board;
    public GameObject[,] boardCells;

    void Start()
    {
        boardCells = new GameObject[boardSize, boardSize];
        GenerateBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GenerateBoard()
    {
        float startX = -473, startY  = 530, step = 135;
        for (int i = 0; i< boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                boardCells[i, j] = Instantiate(cell, new Vector2(startX, startY), cell.transform.rotation);
                boardCells[i, j].name = "cell" + i + j;
                boardCells[i, j].transform.SetParent(board.transform);
                startX += step;
                if (j == boardSize - 1)
                    startX = -473;
            }

            startY -= step;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("kek");
        Instantiate(whiteChip, cell.transform.position, whiteChip.transform.rotation);
    }

}
