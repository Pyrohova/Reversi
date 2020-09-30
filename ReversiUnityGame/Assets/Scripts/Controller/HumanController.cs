using System;
using UnityEngine;
using Assets.Scripts.Model;
using ReversiCore;
using ReversiCore.Enums;
using UnityEngine.EventSystems;

using ChipColor = ReversiCore.Enums.Color;

public class HumanController : MonoBehaviour
{
    [SerializeField] ReversiModelHolder holder;
    private ReversiModel model;

    //convert coordinates and put chip
    private void PutChip(string cellName)
    {
        int x = Int32.Parse(cellName[0].ToString());
        int y = Int32.Parse(cellName[1].ToString());

        model.PutChip(x, y);
    }

    public void NewGameWithRobotAsWhite()
    {
        model.NewGame(GameMode.HumanToRobot, ChipColor.White);
    }

    public void NewGameWithRobotAsBlack()
    {
        model.NewGame(GameMode.HumanToRobot, ChipColor.Black);
    }

    public void NewGameWithSecondPlayer()
    {
        model.NewGame(GameMode.HumanToHuman);
    }

    public void OnClicked()
    {
        //get clicked button by name
        PutChip(EventSystem.current.currentSelectedGameObject.name);
    }

    void Start()
    {
        model = holder.reversiModel;
    }
}
