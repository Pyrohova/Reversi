using System;

namespace ReversiCore
{
    public class CountChangedEventArgs : EventArgs
    {
        public int CountWhite { get; set; }
        public int CountBlack { get; set; }
    }
}
