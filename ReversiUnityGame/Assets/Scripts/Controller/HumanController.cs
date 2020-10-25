using System;
using UnityEngine;
using Assets.Scripts.Model;
using ReversiCore;
using UnityEngine.EventSystems;

using ChipColor = ReversiCore.Enums.Color;
using ReversiRobot;
using UnityEngine.Events;
using ReversiCore.Enums;

public class HumanController : MonoBehaviour
{
    public UnityAction<GameMode> OnNewGameStarted;

    [SerializeField] 
    ReversiModelHolder holder;

    private ReversiModel model;
    private RandomUser robot;

    //convert coordinates and put chip
    private void PutChip(string cellName)
    {
        int x = Int32.Parse(cellName[0].ToString());
        int y = Int32.Parse(cellName[1].ToString());

        model.PutChip(x, y);
    }

    public void NewGameWithRobotAsWhite()
    {
        OnNewGameStarted?.Invoke(GameMode.HumanToRobot);

        robot.Enable(ChipColor.Black);
        model.NewGame();
    }

    public void NewGameWithRobotAsBlack()
    {
        OnNewGameStarted?.Invoke(GameMode.HumanToRobot);

        robot.Enable(ChipColor.White);
        model.NewGame();
    }

    public void NewGameWithSecondPlayer()
    {
        OnNewGameStarted?.Invoke(GameMode.HumanToHuman);

        robot.Disable();
        model.NewGame();
    }

    public void OnClicked()
    {
        //get clicked button by name
        PutChip(EventSystem.current.currentSelectedGameObject.name);
    }

    void Start()
    {
        model = holder.reversiModel;
        robot = holder.robotPlayer;
    }
}
