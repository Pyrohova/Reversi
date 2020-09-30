using System;

namespace ReversiCore
{
    //Cell of the board field
    public class Cell : IComparable<Cell>
    {
        public int X { get; private set; } //x-coordinate of the cell
        public int Y { get; private set; } //y-coordinate of the cell

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }


        /*
         * Method compares current cell to the other one
         * -----------------------------------------
         * other - cell that we compare current cell to
         */
        public int CompareTo(Cell other)
        {
            if (this.X == other.X)
            {
                if (this.Y == other.Y)
                {
                    return 0;
                }

                if (this.Y > other.Y)
                {
                    return 1;
                }

                return -1;
            }

            if (this.X > other.X)
            {
                return 1;
            }

            return -1;
        }
    }
}
