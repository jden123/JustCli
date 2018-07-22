using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("command1", "The first command.")]
    public class FirstCommand : ICommand
    {
        public Task<int> Execute()
        {
            throw new NotImplementedException();
        }
    }
}