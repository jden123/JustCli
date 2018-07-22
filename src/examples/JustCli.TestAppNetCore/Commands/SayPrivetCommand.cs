﻿using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
    /// <summary>
    /// Show unicode support.
    /// </summary>
    /// <seealso cref="JustCli.ICommand" />
    [Command("sayprivet", "Prints 'Привет!'. Shows unicode support.")]
    class SayPrivetCommand : ICommand
    {
        [CommandOutput]
        public IOutput Output { get; set; }

        public Task<int> Execute()
        {
            Output.WriteInfo("Привет!");
            return ReturnCode.Success.ToAsync();
        }

    }
}
