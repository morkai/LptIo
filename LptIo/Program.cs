// Copyright (c) 2015, Łukasz Walukiewicz <lukasz@walukiewicz.eu>
// Licensed under the MIT License <http://lukasz.walukiewicz.eu/p/MIT>
// Part of the LptIo project <http://lukasz.walukiewicz.eu/p/LptIo>

using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

namespace LptIo
{
    class Program
    {
        static int ERR_DRIVER_NOT_OPEN = 0x07;
        static int ERR_DLL_NOT_FOUND = 0x08;
        static int ERR_THREAD = 0x09;

        [DllImport("inpout32.dll")]
        private static extern UInt32 IsInpOutDriverOpen();

        [DllImport("inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
        private static extern UInt32 IsInpOutDriverOpen_x64();

        private static bool x64 = false;

        static void Main(string[] args)
        {
            OpenPort();

            var tokenSource = new CancellationTokenSource();
            var lptQueue = new BlockingCollection<ILptAction>();

            var stdinWorker = new StdinWorker(tokenSource, lptQueue, x64);
            var stdinThread = new Thread(stdinWorker.Run);
            var lptWorker = new LptWorker(tokenSource, lptQueue);
            var lptThread = new Thread(lptWorker.Run);

            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => { tokenSource.Cancel(); };

            try
            {
                stdinThread.Start();
                lptThread.Start();
                stdinThread.Join();
                lptThread.Join();
            }
            catch (Exception x)
            {
                Exit(x, ERR_THREAD);
            }
        }

        public static void Exit(Exception x, int exitCode)
        {
            Console.Error.WriteLine(x.ToString());
            Environment.Exit(exitCode);
        }

        public static void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        private static void OpenPort()
        {
            try
            {
                uint result = 0;

                try
                {
                    result = IsInpOutDriverOpen();
                }
                catch (BadImageFormatException)
                {
                    try
                    {
                        result = IsInpOutDriverOpen_x64();

                        if (result != 0)
                        {
                            x64 = true;
                        }
                    }
                    catch (BadImageFormatException) { }
                }

                if (result == 0)
                {
                    Exit(ERR_DRIVER_NOT_OPEN);
                }
            }
            catch (DllNotFoundException x)
            {
                Exit(x, ERR_DLL_NOT_FOUND);
            }
        }
    }
}
