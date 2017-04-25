using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PS1_l4_2
{
    class Program
    {
        private static readonly int taskAmount = 3;
        private static CancellationTokenSource[] tokenSource = new CancellationTokenSource[taskAmount];
        private static CancellationToken[] ct = new CancellationToken[taskAmount];
        private static Task[] t = new Task[taskAmount];
        static void Main(string[] args)
        {
            String x = null;
            for (int i = 0; i < taskAmount; i++)
            {
                int z = i;
                tokenSource[i] = new CancellationTokenSource();
                ct[i] = tokenSource[i].Token;
                t[i] = Task.Factory.StartNew(() => AlphabetWriter(i, ct[i]), ct[i]);
            }
            while (x != "end")
            {
                x = Console.ReadLine().ToLower();
                try
                {
                    InterpretCommand(x);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }
        static async Task AlphabetWriter(int number, CancellationToken ct)
        {
            try
            {
                for (char letter = 'A'; letter <= 'Z'; letter++)
                {
                    ct.ThrowIfCancellationRequested();
                    if (number + 1 > 9)
                    {
                        number = -1;
                    }
                    Console.WriteLine("{0}{1}", letter, number + 1);
                    await Task.Delay(1000);
                }
            }
            catch (OperationCanceledException e)
            {
                t[number].Dispose();
            }
        }
        static void InterpretCommand(String command)
        {
            String[] splittedCommand = command.Split(null);
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
        static void AbortTask(int nr)
        {
            try
            {
                if (t[nr - 1].IsCanceled || t[nr-1].IsFaulted)
                {
                    Console.WriteLine("Task {0} is already canceled", nr - 1);
                }
                else
                {
                    Console.WriteLine("Task {0} cancelled", nr);
                    tokenSource[nr - 1].Cancel();
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("No task with number {0}", nr);
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
