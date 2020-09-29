using System;

namespace ReversiCore
{
    public class Cell : IComparable<Cell>, IEquatable<Cell>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(Cell other)
        {
            if (this.X == other.X && this.Y == other.Y)
            {
                return 0;
            }

            if (this.X < other.X)
            {
                return -1;
            }

            return 1;
        }

        public bool Equals(Cell other)
        {
            if (this.X == other.X && this.Y == other.Y)
            {
                return true;
            }

            return false;
        }
    }
}
