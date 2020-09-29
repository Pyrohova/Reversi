using ReversiCore;
using ReversiCore.Enums;
using ReversiRobot;
using System;

namespace ReversiCoreTest
{
    class Program
    {
        static void Test1()
        {
            ReversiModel model = new ReversiModel();

            model.WrongMove += (s, ea) => 
            { 
                Console.WriteLine("Wrong move");

                foreach (Cell cell in model.currentAllowedCells)
                {
                    Console.WriteLine("{0} {1}", cell.X, cell.Y);
                }
            };
            model.SwitchMove += (s, ea) => { Console.WriteLine("Switch move {0}", ea.CurrentPlayerColor); };
            model.SwitchMove += (s, ea) =>
            {
                foreach (Cell cell in ea.AllowedCells)
                {
                    Console.WriteLine("{0} {1}", cell.X, cell.Y);
                }
                Console.WriteLine("-------------------");
                foreach (Cell cell in model.currentAllowedCells)
                {
                    Console.WriteLine("{0} {1}", cell.X, cell.Y);
                }
            };
            model.CountChanged += (s, ea) => { Console.WriteLine("count {0} {1}", ea.CountBlack, ea.CountWhite); };

            model.NewGame(GameMode.HumanToHuman);

            while (true)
            {
                int x = int.Parse(Console.ReadLine());
                int y = int.Parse(Console.ReadLine());
                model.PutChip(x, y);
            }
        }

        //Test robot mode (black)
        static void Test2()
        {
            ReversiModel model = new ReversiModel();
            RandomUser robot = new RandomUser(model);

            model.SwitchMove += (s, ea) => { Console.WriteLine("Switch move {0}", ea.CurrentPlayerColor); };
            model.SwitchMove += (s, ea) =>
            {
                Console.WriteLine(ea.AllowedCells.Count);
                foreach (Cell cell in ea.AllowedCells)
                {
                    Console.WriteLine("{0} {1}", cell.X, cell.Y);
                }
            };

            model.NewGame(GameMode.HumanToRobot, Color.Black);

        }

        //Test robot mode (color you want)
        static void Test3(Color userColor)
        {
            ReversiModel model = new ReversiModel();
            RandomUser robot = new RandomUser(model);

            model.NewGameStarted += (s, ea) => { Console.WriteLine(ea.UserPlayerColor); };
            model.SwitchMove += (s, ea) => { Console.WriteLine("Switch move {0}", ea.CurrentPlayerColor); };
            model.SwitchMove += (s, ea) =>
            {
                Console.WriteLine(ea.AllowedCells.Count);
                foreach (Cell cell in ea.AllowedCells)
                {
                    Console.WriteLine("{0} {1}", cell.X, cell.Y);
                }
            };

            model.NewGame(GameMode.HumanToRobot, userColor);
        }


        static void Main(string[] args)
        {
            Test1();
        }
    }
}
