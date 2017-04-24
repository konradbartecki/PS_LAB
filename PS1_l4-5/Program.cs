using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;

namespace PS1_l4_2
{
    class Program
    {
        private static readonly int taskAmount = 10;
        private static Task[] t = new Task[taskAmount];
        static void Main(string[] args)
        {
            String x = null;
            for (int i = 0; i < taskAmount; i++)
            {
                int z = i;
                t[i] = new Task(() => AlphabetWriter(z));
            }
            while (x != "end")
            {
                x = Console.ReadLine();
                try
                {
                    InterpretCommand(x);
                    for (int i = 0; i < taskAmount; i++)
                    {
                        int z = i;
                    }
                }
                catch (ThreadStateException e)
                {
                    Console.WriteLine("Thread is already running!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }
        static async Task AlphabetWriter(int number)
        {
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                if (number + 1 > 9)
                {
                    number = -1;
                }
                Console.WriteLine("{0}{1}", letter, number + 1);
                await Task.Delay(1000);
            }
        }
        static void InterpretCommand(String command)
        {
            String[] splittedCommand = command.Split(null);
            List<int> thrNumber = RecognizeNumbers(splittedCommand[1]);
            if (splittedCommand[0] == "stop")
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
            else if (splittedCommand[0] == "start")
            {
                if (thrNumber.Count == 1)
                {
                    StartTask(thrNumber[0]);
                }
                else if (thrNumber.Count == 2)
                {

                    for (int i = thrNumber[0]; i <= thrNumber[1]; i++)
                    {
                        StartTask(i);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
        static void AbortTask(int nr)
        {
            nr--;
            if(t[nr].Status != TaskStatus.Canceled)
            {
                
            }
            else
            {
                Console.WriteLine("Can't abort, already aborted or not yet started.");
            }
        }
        static void StartTask(int nr)
        {
            nr--;
            if (t[nr].Status == TaskStatus.Created)
            {
                t[nr].Start();
            }
            else
            {
                Console.WriteLine("Can't start, already running or finished.");
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
