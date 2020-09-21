using ReversiCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiCore
{
    internal class TurnHolder
    {
        private readonly Color firstTurnColor;
        private Color _currentTurnColor;

        internal Color CurrentTurnColor 
        {
            get 
            {
                return _currentTurnColor;
            }

            private set
            {
                _currentTurnColor = value;

                if (value == Color.White)
                {
                    OppositeTurnColor = Color.Black;
                }
                else
                {
                    OppositeTurnColor = Color.White;
                }
            }
        }

        internal Color OppositeTurnColor { get; private set; }

        internal TurnHolder()
        {
            firstTurnColor = Color.Black;
            Reset();
        }

        internal void Reset()
        {
            CurrentTurnColor = firstTurnColor;
        }

        internal void Switch()
        {
            CurrentTurnColor = OppositeTurnColor;
        }
    }
}
