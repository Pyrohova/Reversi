namespace ReversiCore
{
    public class Cell
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
