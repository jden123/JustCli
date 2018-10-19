using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("asynccommand", "The async command.")]
    public class AsyncCommand : ICommandAsync
    {
        public Task<int> ExecuteAsync()
        {
            return Task.Factory.StartNew(() => { return ReturnCode.Success; });;
        }
    }
}