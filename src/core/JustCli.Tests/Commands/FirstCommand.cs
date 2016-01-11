using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("command1", "The first command.")]
    public class FirstCommand : ICommand
    {
        public bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}