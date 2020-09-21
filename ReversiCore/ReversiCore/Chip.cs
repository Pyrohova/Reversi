using ReversiCore.Enums;

namespace ReversiCore
{
    public class Chip
    {
        public Color Color { get; internal set; }
        public Cell Cell { get; private set; }

        public Chip(Color color, Cell cell)
        {
            Color = color;
            Cell = cell;
        }
    }
}
