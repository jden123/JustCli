using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
    [Command("sayhello", "Prints a greeting.")]
    class SayHelloCommand : ICommand
    {
        [CommandArgument("n", "name", Description = "The someone to greet.", DefaultValue = "World")]
        public string Name { get; set; }

        [CommandOutput]
        public IOutput Output { get; set; }

        public int Execute()
        {
            Output.WriteInfo(string.Format("Hello {0}!", Name));
            return ReturnCode.Success;
        }
    }
}
