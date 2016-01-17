using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("command1", "The first command.")]
    public class FirstCommand : ICommand
    {
        public int Execute()
        {
            throw new NotImplementedException();
        }
    }
}