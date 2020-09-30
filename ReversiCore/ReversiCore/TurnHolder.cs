using ReversiCore.Enums;

namespace ReversiCore
{
    //Responsible for current and opposite turns
    internal class TurnHolder
    {
        public Color FirstTurnColor { get; private set; } //Color of the player that makes a move first
        private Color _currentTurnColor;

        internal Color CurrentTurnColor //Color of the player who currently is making a move
        {
            get 
            {
                return _currentTurnColor;
            }

            private set
            {
                _currentTurnColor = value;

                //Change opposite color
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

        internal Color OppositeTurnColor { get; private set; } //Color of the player who currently isn't making a move

        internal TurnHolder()
        {
            FirstTurnColor = Color.Black;
            Reset();
        }


        /*
         * Method puts turn holder into state of the game start
         */
        internal void Reset()
        {
            CurrentTurnColor = FirstTurnColor;
        }


        /*
         * Method switches current turn
         */
        internal void Switch()
        {
            CurrentTurnColor = OppositeTurnColor;
        }
    }
}
