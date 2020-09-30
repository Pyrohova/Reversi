using System;

namespace ReversiCore
{
    public class CountChangedEventArgs : EventArgs
    {
        public int CountWhite { get; set; } //Score of the white player
        public int CountBlack { get; set; } //Score of the black player
    }
}
