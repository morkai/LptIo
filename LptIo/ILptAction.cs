// Copyright (c) 2015, Łukasz Walukiewicz <lukasz@walukiewicz.eu>
// Licensed under the MIT License <http://lukasz.walukiewicz.eu/p/MIT>
// Part of the LptIo project <http://lukasz.walukiewicz.eu/p/LptIo>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LptIo
{
    public interface ILptAction
    {
        void ReadArgs(string[] args);

        void Execute();
    }
}
