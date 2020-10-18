namespace AIGenerator
{
    //Reversi game chip
    public class Chip
    {
        public Color Color { get; internal set; } //Color of the chip (White/Black)
        public Cell Cell { get; private set; } //Board field cell that contains current chip

        public Chip(Color color, Cell cell)
        {
            Color = color;
            Cell = cell;
        }
    }
}
