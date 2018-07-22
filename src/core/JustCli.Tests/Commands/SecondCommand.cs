using System;
using System.Threading.Tasks;

namespace JustCli.Tests.Commands
{
    public class SecondCommand : ICommand
    {
        public Task<int> Execute()
        {
            throw new NotImplementedException();
        }
    }
}