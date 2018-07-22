using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    public class DoSomethingAsyncCommand : ICommand
    {
        [CommandArgument("a", "action", DefaultValue = "default")]
        public string Action { get; private set; }

        [CommandOutput]
        public IOutput Output { get; set; }

        public int Execute()
        {
            throw new NotImplementedException();
        }

        public async Task<int> ExecuteAsync()
        {
            return await Task.Factory.StartNew(
                () => (ReturnCode.Success));
        }
    }
}