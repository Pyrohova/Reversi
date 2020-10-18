using ReversiCore.Enums;
using ReversiColor = ReversiCore.Enums.Color;

namespace AIGenerator
{
    //Field of the game board
    internal class Field
    {
        internal int Size { get; private set; } //Size of the field (Size == width == height)
        internal ReversiColor?[,] PlacedChips { get; private set; } //Colors of the chips that has been placed onto field

        internal Field()
        {
            Size = 8;
            PlacedChips = new ReversiColor?[Size, Size];
        }
    }
}
