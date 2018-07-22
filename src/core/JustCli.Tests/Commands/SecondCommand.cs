using System;
using System.Threading.Tasks;

namespace JustCli.Tests.Commands
{
    public class SecondCommand : ICommand
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