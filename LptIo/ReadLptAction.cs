// Copyright (c) 2015, Łukasz Walukiewicz <lukasz@walukiewicz.eu>
// Licensed under the MIT License <http://lukasz.walukiewicz.eu/p/MIT>
// Part of the LptIo project <http://lukasz.walukiewicz.eu/p/LptIo>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LptIo
{
    class ReadLptAction : ILptAction
    {
        [DllImport("inpout32.dll")]
        private static extern ushort DlPortReadPortUshort(int lptPort);

        [DllImport("inpoutx64.dll", EntryPoint = "DlPortReadPortUshort")]
        private static extern ushort DlPortReadPortUshort_x64(int lptPort);

        private bool x64;

        private short lptPort = 0x378;

        private short bit = 0;

        public ReadLptAction(bool x64)
        {
            this.x64 = x64;
        }

        public void ReadArgs(string[] args)
        {
            lptPort = Convert.ToInt16(args[1]);
            bit = Convert.ToInt16(args[2]);
        }

        public void Execute()
        {
            ushort value = x64 ? DlPortReadPortUshort_x64(lptPort) : DlPortReadPortUshort(lptPort);
            bool state = (value & (1 << bit)) != 0;
            
            Console.WriteLine("READ {0} {1} {2} {3}", lptPort, bit, value, state ? 1 : 0);
        }
    }
}
