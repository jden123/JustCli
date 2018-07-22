using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
    [Command("sayhello", "Prints a greeting.")]
    class SayHelloCommand : ICommand
    {
        [CommandArgument("n", "name", Description = "The someone to greet.", DefaultValue = "World")]
        public string Name { get; set; }
        
        [CommandArgument("d", "date", Description = "Should show current UTC time.", DefaultValue = false)]
        public bool ShowCurrentUtcTime { get; set; }

        [CommandOutput]
        public IOutput Output { get; set; }

        public Task<int> Execute()
        {
            if (ShowCurrentUtcTime)
            {
                Output.WriteInfo(string.Format("Current UTC time: {0}", DateTime.UtcNow));
            }
            
            Output.WriteInfo(string.Format("Hello {0}!", Name));
            return ReturnCode.Success.ToAsync();
        }
    }
}
