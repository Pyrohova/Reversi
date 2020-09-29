using ReversiCore.Enums;

namespace ReversiCore
{
    internal class Field
    {
        internal int Size { get; private set; }
        internal Color?[,] PlacedChips { get; private set; }

        internal Field()
        {
            Size = 8;
            PlacedChips = new Color?[Size, Size];
        }
    }
}
