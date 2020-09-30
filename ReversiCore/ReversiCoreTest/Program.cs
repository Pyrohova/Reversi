using ReversiCore;
using ReversiCore.Enums;
using ReversiRobot;
using System;

namespace ReversiCoreTest
{
    /*
     * Here is some code for manual and dirty testing of the reversi core
     */

    class Program
    {
        //Simulates human to human game mode
        static void Test1()
        {
            ReversiModel model = new ReversiModel();

            model.WrongMove += (s, ea) => 
            { 
                Console.WriteLine("Wrong move");
            };
            model.SwitchMove += (s, ea) => { Console.WriteLine("Switch move {0}", ea.CurrentPlayerColor); };
            model.SwitchMove += (s, ea) =>
            {
                foreach (Cell cell in ea.AllowedCells)
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

            while (true)
            {
                int x = int.Parse(Console.ReadLine());
                int y = int.Parse(Console.ReadLine());
                model.PutChip(x, y);
            }
        }

        //Test robot mode (playing for color you want)
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

            while (true)
            {
                int x = int.Parse(Console.ReadLine());
                int y = int.Parse(Console.ReadLine());
                model.PutChip(x, y);
            }
        }


        static void Main(string[] args)
        {
            Test1();
        }
    }
}
