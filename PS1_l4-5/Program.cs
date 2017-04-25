using PS1_l4_5;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PS1_l4_5
{
    class Program
    {
        private static readonly int taskAmount = 3;
        private static List<AlphabetWriter> _alphabetWriters;
        static void Main(string[] args)
        {
            _alphabetWriters = new List<AlphabetWriter>();
            for (int i = 0; i < taskAmount; i++)
                _alphabetWriters.Add(new AlphabetWriter(i));
            _alphabetWriters.ForEach(x => Task.Run(async () => await x.Write()));
            ReadUserInputLoop();
        }

        static void ReadUserInputLoop()
        {
            string userInput = null;
            do
            {
                userInput = Console.ReadLine().ToLower();
                try
                {
                    InterpretCommand(userInput);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid command");
                }
            } while (userInput != "end");
        }
        static void InterpretCommand(String command)
        {
            string[] splittedCommand = command.Split(' ');
            List<int> thrNumber = RecognizeNumbers(splittedCommand[1]);
            if (splittedCommand[0] == "abort")
            {
                if (thrNumber.Count == 1)
                {
                    AbortTask(thrNumber[0]);
                }
                else if (thrNumber.Count == 2)
                {
                    for (int i = thrNumber[0]; i <= thrNumber[1]; i++)
                    {
                        AbortTask(i);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
        static void AbortTask(int number)
        {
            var target = _alphabetWriters.FirstOrDefault(x => x.Number == number);
            if (target == null)
                Console.WriteLine("Unable to find {0} with number {1}", nameof(AlphabetWriter), number);
            else
            {
                target.Cancel();
            }
        }
        static List<int> RecognizeNumbers(String numbers)
        {
            List<int> intList = new List<int>();
            int x;
            if (int.TryParse(numbers, out x))
            {
                intList.Add(x);
            }
            else
            {
                String[] s = numbers.Split('-');
                intList.Add(Int32.Parse(s[0]));
                intList.Add(Int32.Parse(s[1]));
            }
            return intList;
        }
    }
}
