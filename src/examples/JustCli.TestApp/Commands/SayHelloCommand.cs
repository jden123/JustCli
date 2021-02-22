using System;
using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
    [Command(
       "sayhello",
       "Prints a greeting.",
       @"Very useful command that print hello to the output with the word you provide.
This command is greatly described in this help text so you should be able to use it properly after you read it.
More examples at http://hello.world/greetings/examples")]
    class SayHelloCommand : ICommand
    {
        [CommandArgument("n", "name", Description = "The someone to greet.", DefaultValue = "World")]
        public string Name { get; set; }

        [CommandArgument("d", "date", Description = "Should show current UTC time.", DefaultValue = false)]
        public bool ShowCurrentUtcTime { get; set; }

        [CommandOutput]
        public IOutput Output { get; set; }

        public int Execute()
        {
            if (ShowCurrentUtcTime)
            {
                Output.WriteInfo(string.Format("Current UTC time: {0}", DateTime.UtcNow));
            }

            Output.WriteInfo(string.Format("Hello {0}!", Name));
            return ReturnCode.Success;
        }
    }
}
