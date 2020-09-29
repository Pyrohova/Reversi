using ReversiCore;
using ReversiCore.Enums;
using System;

namespace ReversiCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ReversiModel model = new ReversiModel();

            model.WrongMove += (s, ea) => { Console.WriteLine("Wrong move"); };
            model.SwitchMove[Color.White] += (s, ea) => { Console.WriteLine("Switch move {0}", ea.CurrentPlayerColor); };
            model.SwitchMove[Color.Black] += (s, ea) => { Console.WriteLine("Switch move {0}", ea.CurrentPlayerColor); };
            model.SwitchMove[Color.White] += (s, ea) =>
            {
                foreach (Cell cell in ea.AllowedCells)
                {
                    Console.WriteLine("{0} {1}", cell.X, cell.Y);
                }
            };
            model.SwitchMove[Color.Black] += (s, ea) => 
                {
                    Console.WriteLine(ea.AllowedCells.Count);
                    foreach (Cell cell in ea.AllowedCells)
                    {
                        Console.WriteLine("{0} {1}", cell.X, cell.Y);
                    }
                };

            model.NewGame(GameMode.HumanToHuman);
            //model.PutChip(2, 3);
        }
    }
}
