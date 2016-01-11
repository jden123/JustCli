namespace JustCli.Commands
{
    public class CommandLineHelpCommand : ICommand
    {
        public ICommandRepository CommandRepository { get; set; }
        public IOutput Output { get; set; }

        public CommandLineHelpCommand(ICommandRepository commandRepository, IOutput output)
        {
            CommandRepository = commandRepository;
            Output = output;
        }

        public bool Execute()
        {
            var commandsInfo = CommandRepository.GetCommandsInfo();

            Output.WriteInfo("Command list:");
            foreach (var commandInfo in commandsInfo)
            {
                Output.WriteInfo(string.Format("{0} - {1}", commandInfo.Name, commandInfo.Description));
            }

            return true;
        }
    }
}