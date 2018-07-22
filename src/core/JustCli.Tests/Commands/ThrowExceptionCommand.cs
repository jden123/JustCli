using System;
using System.Threading.Tasks;
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

        public Task<int> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}