using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("DoSomethingAsync", "Async Task")]
    public class DoSomethingAsyncCommand : ICommand
    {
        [CommandOutput]
        public IOutput Output { get; set; }

        public async Task<int> Execute()
        {
            return await ReturnCode.Success.ToAsync();
        }

    }
}