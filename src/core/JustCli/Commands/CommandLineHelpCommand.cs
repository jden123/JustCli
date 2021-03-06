using System.Linq;

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

        public int Execute()
        {
            var commandsInfo = CommandRepository.GetCommandsInfo();

            if (commandsInfo == null || commandsInfo.Count == 0)
            {
                Output.WriteInfo("There are no commands.");
                return ReturnCode.Success;
            }

            Output.WriteInfo("Command list:");
            foreach (var commandInfo in commandsInfo.OrderBy(i => i.Order).ThenBy(i => i.Name))
            {
                Output.WriteInfo(string.Format("{0} - {1}", commandInfo.Name, commandInfo.Description));
            }

            return ReturnCode.Success;
        }
    }
}