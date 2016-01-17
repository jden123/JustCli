using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("DoSomethingNTimesCommand", "Do something n times.")]
    public class DoSomethingNTimesCommand : ICommand
    {
        [CommandArgument("a", "action", Description = "Defines what should be done.")]
        public string Action { get; private set; }

        [CommandArgument("r", "repeat", Description = "Number of repeats.", DefaultValue = 1)]
        public int Repeat { get; private set; }

        public int Execute()
        {
            throw new NotImplementedException();
        }
    }
}