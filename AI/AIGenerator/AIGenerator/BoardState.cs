using ReversiCore.Enums;

namespace AIGenerator
{
    class BoardState
    {
        public int FieldSize { get; private set; }
        public Color?[,] Field { get; set; }

        public BoardState()
        {
            FieldSize = 8;
            Field = new Color?[FieldSize, FieldSize];
        }
    }
}
