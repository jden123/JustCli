using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    public class DoSomethingCommand : ICommand
    {
        [CommandArgument("a", "action", DefaultValue = "default")]
        public string Action { get; private set; }

        [CommandOutput]
        public IOutput Output { get; set; }

        public int Execute()
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}