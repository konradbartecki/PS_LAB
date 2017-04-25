﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS1_l4_5
{
    public class AlphabetWriter
    {
        public AlphabetWriter(int number)
        {
            Number = number;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public AlphabetWriter()
        {
            CancellationTokenSource = new CancellationTokenSource();
        }

        public int Number { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public async Task Write()
        {
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                if (CancellationTokenSource.IsCancellationRequested)
                {
                    Console.Write("Writer {0} cancellation requested", Number);
                    return;
                }
                if (Number + 1 > 9)
                {
                    Number = -1;
                }
                Console.WriteLine("{0}{1}", letter, Number);
                await Task.Delay(1000);
            }
        }

        public void Cancel()
        {
            if(CancellationTokenSource?.IsCancellationRequested == false
                && CancellationTokenSource?.Token.CanBeCanceled == true)
                CancellationTokenSource.Cancel();
        }
    }
}
