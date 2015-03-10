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
using System.Runtime.InteropServices;

namespace LptIo
{
    public class LptWorker
    {
        private CancellationTokenSource tokenSource;

        private BlockingCollection<ILptAction> lptQueue;

        public LptWorker(CancellationTokenSource tokenSource, BlockingCollection<ILptAction> lptQueue)
        {
            this.tokenSource = tokenSource;
            this.lptQueue = lptQueue;
        }

        public void Run()
        {
            while (!tokenSource.IsCancellationRequested && !lptQueue.IsAddingCompleted)
            {
                try
                {
                    lptQueue.Take().Execute();
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception) {}
            }
        }
    }
}
