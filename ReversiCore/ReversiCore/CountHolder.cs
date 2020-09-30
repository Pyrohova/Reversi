using ReversiCore.Enums;
using System.Collections.Generic;

namespace ReversiCore
{
    //Responsible for current score of both players (Black and White)
    internal class CountHolder
    {
        private Dictionary<Color, int> counts; //Scores of both players (Black and White)

        internal CountHolder()
        {
            counts = new Dictionary<Color, int>();
            Reset();
        }


        /*
         * Puts initial values into players scores
         */
        internal void Reset()
        {
            counts[Color.White] = 2;
            counts[Color.Black] = 2;
        }


        /*
         * Methos increases score of the given player by given delta value
         * -----------------------------------------
         * playerColor - color of the player whose score has to be increased
         * delta - value which score has to be increased by
         */
        internal void Increase(Color playerColor, int delta)
        {
            counts[playerColor] += delta;
        }


        /*
         * Methos decreases score of the given player by given delta value
         * -----------------------------------------
         * playerColor - color of the player whose score has to be decreased
         * delta - value which score has to be decreased by
         */
        internal void Decrease(Color playerColor, int delta)
        {
            counts[playerColor] -= delta;
        }


        /*
         * Method returns score of the player of the given color
         * -----------------------------------------
         * playerColor - color of the player whose score has to be returned
         */
        internal int GetPlayerCount(Color playerColor)
        {
            return counts[playerColor];
        }


        /*
         * Method returns color of the player that currently wins 
         * (returns null when it's a draw)
         */
        internal Color? GetWinner()
        {
            if (counts[Color.White] == counts[Color.Black])
            {
                return null;
            }

            if (counts[Color.White] > counts[Color.Black])
            {
                return Color.White;
            }

            return Color.Black;
        }
    }
}
