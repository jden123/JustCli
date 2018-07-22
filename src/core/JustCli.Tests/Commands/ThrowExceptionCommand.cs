using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
    [Command("ex", "Throws exception.")]
    public class ThrowExceptionCommand : ICommand
    {
        public async Task<int> Execute()
        {
            try
            {
                throw new Exception("testexception");
                    
            }
            catch (Exception)
            {
                return await ReturnCode.Failure.ToAsync();
            }
            
        }
    }
}