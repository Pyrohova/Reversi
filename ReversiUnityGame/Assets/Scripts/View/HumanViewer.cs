using ReversiCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Model;
using ReversiCore.Enums;

using ChipColor = ReversiCore.Enums.Color;
using System;

namespace Assets.Scripts.View
{
    public class HumanViewer : MonoBehaviour
    {
        [SerializeField] 
        private ReversiModelHolder holder;

        [SerializeField]
        private HumanController controller;

        private ReversiModel model;

        [SerializeField] 
        GameBoard gameBoard;

        [SerializeField] 
        PlayerInfo playerInfo;

        [SerializeField] 
        ScoreInfo scoreInfo;

        private GameMode currentMode;

        private ChipColor playerColor;

        private DelayRobotMoveTimer delayRobotMoveTimer = new DelayRobotMoveTimer();
        private DelayRobotMoveQueue delayRobotMoveQueue = new DelayRobotMoveQueue();

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


        private void SwitchMoveConsideringUserType(object sender, SwitchMoveEventArgs e)
        {
            /*
             * if it's HumanToHuman mode 
             * or it's HumanToRobot mode but robot turn 
             * or robot move is not currently delayed
             * => do not make delay
             * else make delay
             */
            if (currentMode == GameMode.HumanToHuman || e.CurrentPlayerColor != playerColor || !delayRobotMoveTimer.IsRunning)
            {
                SwitchTurn(e.AllowedCells, e.CurrentPlayerColor);
                return;
            }

            delayRobotMoveQueue.AddDelegate(() => { SwitchTurn(e.AllowedCells, e.CurrentPlayerColor); });
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

        private void NewGameStarted(object sender, EventArgs e)
        {
            delayRobotMoveTimer.Stop();

            //reset result
            ClearAll();
        }

        private void SetGameMode(GameMode gameMode)
        {
            //display info that new game started
            if (gameMode == GameMode.HumanToHuman)
                playerInfo.UpdateInfoField("new game with second player started");
            else
            {
                playerInfo.UpdateInfoField("new game with robot started");
            }

            //set new current mode
            currentMode = gameMode;
        }

        private void SetPlayerColor(ChipColor newPlayerColor)
        {
            playerColor = newPlayerColor;
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
            /*
             * if it is HumanToHuman mode 
             * or it is Human to Robot mode but human move
             * or current SetChips event configures start board position
             * => do not make delay
             * else make delay
             */
            if (currentMode == GameMode.HumanToHuman || e.NewChip.Color == playerColor || e.ChangedChips.Count == 0)
            {
                SetChips(e.NewChip, e.ChangedChips);
                return;
            }

            delayRobotMoveTimer.Restart(delayRobotMoveTime);
            delayRobotMoveQueue.Clear();
            delayRobotMoveQueue.AddDelegate(() => {SetChips(e.NewChip, e.ChangedChips); });
        }

        private void SetChips(Chip newChip, IEnumerable<Chip> changedChips)
        {
            // add new chip on the board and replace changed

            gameBoard.AddChip(newChip);
            gameBoard.ReplaceChipsColor(changedChips);
        }


        private void SubscribeOnEvents()
        {
            controller.OnGameModeChanged += SetGameMode;
            controller.OnPlayerColorChanged += SetPlayerColor;

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
                delayRobotMoveQueue.CallDelayedDelegates();
                delayRobotMoveQueue.Clear();
            }
        }
    }
}