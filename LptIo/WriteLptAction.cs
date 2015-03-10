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
    class WriteLptAction : ILptAction
    {
        [DllImport("inpout32.dll")]
        private static extern void DlPortWritePortUshort(short lptPort, ushort value);
        [DllImport("inpout32.dll")]
        private static extern ushort DlPortReadPortUshort(short lptPort);

        [DllImport("inpoutx64.dll", EntryPoint = "DlPortWritePortUshort")]
        private static extern void DlPortWritePortUshort_x64(short lptPort, ushort value);
        [DllImport("inpoutx64.dll", EntryPoint = "DlPortReadPortUshort")]
        private static extern ushort DlPortReadPortUshort_x64(short lptPort);

        private bool x64;

        private short lptPort = 0x378;

        private short bit = 0;

        private bool state = false;

        public WriteLptAction(bool x64)
        {
            this.x64 = x64;
        }

        public void ReadArgs(string[] args)
        {
            lptPort = Convert.ToInt16(args[1]);
            bit = Convert.ToInt16(args[2]);
            state = args[3] == "1";
        }

        public void Execute()
        {
            int value = (int)(x64 ? DlPortReadPortUshort_x64(lptPort) : DlPortReadPortUshort(lptPort));

            if (state)
            {
                value |= 1 << bit;
            }
            else
            {
                value &= ~(1 << bit);
            }

            if (x64)
            {
                DlPortWritePortUshort_x64(lptPort, (ushort)value);
            }
            else
            {
                DlPortWritePortUshort(lptPort, (ushort)value);
            }

            Console.WriteLine("WRITE {0} {1} {2} {3}", lptPort, bit, state ? 1 : 0, value);
        }
    }
}
