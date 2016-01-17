using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("ex", "Throws exception.")]
    public class ThrowExceptionCommand : ICommand
    {
        public int Execute()
        {
            throw new Exception("exception!");
        }
    }
}