﻿using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    public class DoSomethingCommand : ICommand
    {
        [CommandArgument("a", "action", DefaultValue = "default")]
        public string Action { get; private set; }

        public int Execute()
        {
            throw new NotImplementedException();
        }
    }
}