using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model;
using ReversiCore;
using ReversiCore.Enums;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HumanController : MonoBehaviour
{
    [SerializeField] ReversiModelHolder holder;
    private ReversiModel model;

    private ReversiCore.Enums.Color playerColor;


    public void PutChip(string cellName)
    {
        int x = Int32.Parse(cellName[0].ToString());
        int y = Int32.Parse(cellName[1].ToString());

        model.PutChip(x, y);

    }
    public void NewGameWithRobotAsWhite()
    {
        model.NewGame(GameMode.HumanToRobot, ReversiCore.Enums.Color.White);
    }

    public void NewGameWithRobotAsBlack()
    {
        model.NewGame(GameMode.HumanToRobot, ReversiCore.Enums.Color.Black);
    }

    public void NewGameWithSecondPlayer()
    {
        model.NewGame(GameMode.HumanToHuman);
    }

    public void OnClicked()
    {
        Debug.Log("called");
        PutChip(EventSystem.current.currentSelectedGameObject.name);
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
