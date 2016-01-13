using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("ex", "Throws exception.")]
    public class ThrowExceptionCommand : ICommand
    {
        public bool Execute()
        {
            throw new Exception("exception!");
        }
    }
}