using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("DoSomethingWithDates", "The command for date default values testing.")]
    public class DoSomethingWithDatesCommand : ICommand
    {
        [CommandArgument("min", "mindate", DefaultValue = "minvalue")]
        public DateTime MinDate { get; private set; }

        [CommandArgument("max", "maxdate", DefaultValue = "maxvalue")]
        public DateTime MaxDate { get; private set; }

        [CommandArgument("d", "date", DefaultValue = "1983.10.03")]
        public DateTime Date { get; private set; }

        [CommandOutput]
        public IOutput Output { get; set; }

        public int Execute()
        {
            throw new NotImplementedException();
        }
    }
}