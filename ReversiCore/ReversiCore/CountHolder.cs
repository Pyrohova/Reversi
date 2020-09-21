using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal class CountHolder
    {
        private Dictionary<Color, int> counts;

        internal CountHolder()
        {
            counts = new Dictionary<Color, int>();
            Reset();
        }

        internal void Reset()
        {
            counts[Color.White] = 0;
            counts[Color.Black] = 0;
        }

        internal void Increase(Color playerColor, int delta)
        {
            counts[playerColor] += delta;
        }

        internal void Decrease(Color playerColor, int delta)
        {
            counts[playerColor] -= delta;
        }

        internal int GetPlayerCount(Color playerColor)
        {
            return counts[playerColor];
        }

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
