using System;
using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
    [Command("time", "Prints date time.")]
    class ShowTimeCommand : ICommand
    {
        [CommandArgument("u", "utc", false, "true for utc time, otherwise, false.")]
        public bool ShowUtc { get; set; }
        
        [CommandOutput]
        public IOutput Output { get; set; }

        public int Execute()
        {
            Output.WriteInfo(ShowUtc
                ? string.Format("Utc time is {0}", DateTime.UtcNow)
                : string.Format("Local time is {0}", DateTime.Now));

            return ReturnCode.Success;
        }
    }
}
