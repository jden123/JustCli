using System;
using System.Threading.Tasks;
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

        public Task<int> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}