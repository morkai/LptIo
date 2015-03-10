// Copyright (c) 2015, Łukasz Walukiewicz <lukasz@walukiewicz.eu>
// Licensed under the MIT License <http://lukasz.walukiewicz.eu/p/MIT>
// Part of the LptIo project <http://lukasz.walukiewicz.eu/p/LptIo>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace LptIo
{
    public class StdinWorker
    {
        private CancellationTokenSource tokenSource;

        private BlockingCollection<ILptAction> lptQueue;

        private bool x64;

        public StdinWorker(CancellationTokenSource tokenSource, BlockingCollection<ILptAction> lptQueue, bool x64)
        {
            this.tokenSource = tokenSource;
            this.lptQueue = lptQueue;
            this.x64 = x64;
        }

        public void Run()
        {
            while (!tokenSource.IsCancellationRequested && !lptQueue.IsAddingCompleted)
            {
                string[] actionArgs;

                try
                {
                    actionArgs = Console.ReadLine().Trim().Split(' ');
                }
                catch (Exception)
                {
                    break;
                }

                ILptAction lptAction = null;

                switch (actionArgs[0])
                {
                    case "WRITE":
                        lptAction = new WriteLptAction(x64);
                        break;

                    case "READ":
                        lptAction = new ReadLptAction(x64);
                        break;
                }

                if (lptAction == null)
                {
                    continue;
                }

                try
                {
                    lptAction.ReadArgs(actionArgs);

                    lptQueue.Add(lptAction);
                }
                catch (Exception) {}
            }
        }
    }
}
