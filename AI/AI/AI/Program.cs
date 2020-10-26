using ReversiCore.Enums;
using System;
using AIGenerator;
using ReversiCore;

namespace AI
{
    class Program
    {
        private static ReversiModel model;
        private static Generator generator;
        private static Color playerColor;
        private static bool opponentPassed;

        private static AIGenerator.Cell ReadOpponentMove()
        {
            string coordStr = Console.ReadLine();

            if (coordStr == "pass")
            {
                opponentPassed = true;
                return new AIGenerator.Cell(-1, -1);
            }

            return ParseCell(coordStr);
        }

        private static AIGenerator.Cell ParseCell(string coordStr)
        {
            coordStr = coordStr.Trim();

            if (coordStr.Length > 2)
            {
                throw new ArgumentException("Incorrent coordinates.");
            }

            int coordY = coordStr[0] - 'A';
            if (coordY < 0 || coordY >= 8)
            {
                throw new ArgumentException("Incorrent coordinates.");
            }

            int coordX = coordStr[1] - '1';
            if (coordX < 0 || coordX >= 8)
            {
                throw new ArgumentException("Incorrent coordinates.");
            }

            return new AIGenerator.Cell(coordX, coordY);
        }

        private static AIGenerator.Cell ReadCell()
        {
            string coordStr = Console.ReadLine();

            return ParseCell(coordStr);
        }

        private static Color ReadColor()
        {
            string color = Console.ReadLine();

            if (color == "white")
            {
                return Color.White;
            }

            if (color == "black")
            {
                return Color.Black;
            }

            throw new ArgumentException($"{color} is not a valid color.");
        }

        static void Main()
        {
            model = new ReversiModel();
            generator = new Generator(model);
            opponentPassed = false;

            try
            {
                AIGenerator.Cell blackHole = ReadCell();
                playerColor = ReadColor();
                generator.StartGame(blackHole, playerColor);

                if (playerColor == Color.Black)
                {
                    generator.MakeMove();
                }

                while(!generator.GameIsOver)
                {
                    AIGenerator.Cell opponentMoveCell = ReadOpponentMove();

                    if (opponentPassed)
                    {
                        break;
                    }

                    model.PutChip(opponentMoveCell.X, opponentMoveCell.Y);

                    if (generator.GameIsOver)
                    {
                        break;
                    }

                    generator.MakeMove();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
