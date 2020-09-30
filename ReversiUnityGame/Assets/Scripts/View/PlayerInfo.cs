using UnityEngine;
using ReversiCore.Enums;
using System;
using UnityEngine.UI;

using ChipColor = ReversiCore.Enums.Color;

namespace Assets.Scripts.View
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] Text InfoField;
        [SerializeField] Text CurrentTurn;

        public void UpdateInfoField(string infoText)
        {
            InfoField.text = infoText;
        }

        public void UpdateCurrentTurnColor(ChipColor color)
        {
            CurrentTurn.text = color.ToString();
        }

        public void ClearAll()
        {
            InfoField.text = "";
            CurrentTurn.text = "";
        }


        private void Start()
        {
            ClearAll();
        }

    }
}
