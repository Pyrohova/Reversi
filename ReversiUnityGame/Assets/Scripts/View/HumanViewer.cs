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

        [SerializeField] GameBoard gameBoard;

        [SerializeField] PlayerInfo playerInfo;
        [SerializeField] ScoreInfo scoreInfo;

        private GameMode currentMode;

        private ChipColor playerColor;

        private DelayRobotMoveTimer delayRobotMoveTimer;
        private SwitchMoveEventArgs lastDelayedSwitchMoveEventArgs;
        private SetChipsEventArgs lastDelayedSetChipsEventArgs;

        private float delayRobotMoveTime = 1f; // how many time player will wait for robot's answer

        public void ClearAll()
        {
            gameBoard.ClearAll();

            playerInfo.ClearAll();
            scoreInfo.ClearAll();
        }


        private void SwitchTurn(IEnumerable<Cell> allowedCells, ChipColor currentPlayerColor)
        {
            // display info whose is current turn
            playerInfo.UpdateCurrentTurnColor(currentPlayerColor);

            //if it's not your turn and it's human vs robot mode => so robot should put chip now
            if ((currentPlayerColor != playerColor) && (currentMode == GameMode.HumanToRobot))
            {
                //skip next step, player should not see allowed cells for robot 
                return;
            }

            // make enable new colliders for player
            gameBoard.AllowCells(allowedCells);
        }

        //
        private void SwitchMoveConsideringUserType(object sender, SwitchMoveEventArgs e)
        {
            //if it's pvp mode or it's not your turn or the robot is not delayed now => make turn for player
            if (currentMode == GameMode.HumanToHuman || e.CurrentPlayerColor != playerColor || !delayRobotMoveTimer.IsRunning)
            {
                SwitchTurn(e.AllowedCells, e.CurrentPlayerColor);
                return;
            }

            lastDelayedSwitchMoveEventArgs = e;
        }


        private void CountChanged(object sender, CountChangedEventArgs e)
        {
            //update score
            scoreInfo.UpdateScore(e.CountWhite, e.CountBlack);
        }

        private void WrongMove(object sender, WrongMoveEventArgs e)
        {
            playerInfo.UpdateInfoField("wrong move");
        }

        private void NewGameStarted(object sender, NewGameEventArgs e)
        {
            //reset result
            ClearAll();

            //display info that new game started
            if (e.NewGameMode == GameMode.HumanToHuman)
                playerInfo.UpdateInfoField("new game with second player started");
            else
            {
                playerInfo.UpdateInfoField("new game with robot started");
                playerColor = (ChipColor)e.UserPlayerColor;
            }
            //set new current mode
            currentMode = e.NewGameMode;

        }

        private void GameOver(object sender, GameOverEventArgs e)
        {
            // display info who is a winner

            if (e.WinnerColor == null)
                playerInfo.UpdateInfoField("played a draw!");
            else
                playerInfo.UpdateInfoField(e.WinnerColor + " won!");
        }

        private void SetChipsConsideringUserType(object sender, SetChipsEventArgs e)
        {
            if (currentMode == GameMode.HumanToHuman || e.NewChip.Color == playerColor || e.ChangedChips.Count == 0)
            {
                SetChips(e.NewChip, e.ChangedChips);
                return;
            }

            lastDelayedSetChipsEventArgs = e;
            delayRobotMoveTimer.Start(delayRobotMoveTime);
        }

        private void SetChips(Chip newChip, IEnumerable<Chip> changedChips)
        {
            // add new chip on the board and replace changed

            gameBoard.AddChip(newChip);
            gameBoard.ReplaceChipsColor(changedChips);
        }


        private void SubscribeOnEvents()
        {
            model.NewGameStarted += NewGameStarted;
            model.WrongMove += WrongMove;
            model.SetChips += SetChipsConsideringUserType;
            model.SetChips += (s, ea) => { gameBoard.ClearAllowedCells(); };
            model.GameOver += GameOver;
            model.SwitchMove += SwitchMoveConsideringUserType;
            model.CountChanged += CountChanged;

        }


        void Start()
        {
            delayRobotMoveTimer = new DelayRobotMoveTimer();

            GameBoard gameBoard = new GameBoard();

            PlayerInfo playerInfo = new PlayerInfo();
            ScoreInfo scoreInfo = new ScoreInfo();

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