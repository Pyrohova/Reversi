using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model;
using ReversiCore;
using ReversiCore.Enums;

public class HumanController : MonoBehaviour
{
    [SerializeField] ReversiModelHolder holder;
    private ReversiModel model;

    private ReversiCore.Enums.Color playerColor;

    public void PutChip(string cellName)
    {
        Debug.Log("kek");
        int x = Int32.Parse(cellName[0].ToString());
        int y = Int32.Parse(cellName[1].ToString());

        model.PutChip(x, y);

    }

    public void NewGameWithRobot()
    {
        model.NewGame(GameMode.HumanToRobot);
    }

    public void NewGameWithSecondPlayer()
    {
        model.NewGame(GameMode.HumanToHuman);
    }
    // Start is called before the first frame update
    void Start()
    {
        model = holder.reversiModel;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
