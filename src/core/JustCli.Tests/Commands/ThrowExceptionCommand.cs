using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("ex", "Throws exception.")]
    public class ThrowExceptionCommand : ICommand
    {
        public Task<int> Execute()
        {
            try
            {
                throw new Exception("testexception");
                    
            }
            catch (Exception)
            {
                return ReturnCode.Failure.ToAsync();
            }
            
        }
    }
}