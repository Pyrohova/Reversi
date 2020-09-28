using ReversiCore.Enums;
using System;
using System.Collections.Generic;

namespace ReversiCore
{
    public class RobotColorSetEventArgs : EventArgs
    {
        public Color RobotColor { get; set; }
    }
}
