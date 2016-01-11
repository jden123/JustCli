using System;
using System.Linq;
using JustCli.Commands;

namespace JustCli
{
    public class CommandLineParser
    {
        private static readonly string[] HelpCommandAliases = new[] { "?", "-?", "-h", "--help" }; 

        public ICommandRepository CommandRepository { get; set; }

        private static readonly Lazy<CommandLineParser> defaultParser = 
           new Lazy<CommandLineParser>(() => new CommandLineParser(new AssemblyCommandRepository()));

        public static CommandLineParser Default { get { return defaultParser.Value; } }

        public CommandLineParser(ICommandRepository commandRepository)
        {
            CommandRepository = commandRepository;
        }

        public ICommand ParseCommand(string[] args)
        {
            if (args.Length == 0)
            {
                return CreateDefaultCommand();
            }

            // TODO: parse context here.
            var commandName = args[0];

            if (HelpCommandAliases.Contains(commandName))
            {
                return CreateDefaultCommand();
            }

            var commandType = CommandRepository.GetCommandType(commandName);
            if (commandType == null)
            {
                Console.WriteLine("Command does not exist.");
                return CreateDefaultCommand();
            }

            // NOTE: special case like cmd.exe do -?.
            if (args.Length > 1 && HelpCommandAliases.Contains(args[1]))
            {
                return new CommandHelpCommand(commandType, new ConsoleOutput());
            }

            try
            {
                return CommandActivator.CreateCommand(commandType, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Cannot setup [{0}] command.", commandName));
                return null;

                // NOTE: we can show command help. Do we need?
                //return new CommandHelpCommand(commandType, new ConsoleOutput());
            }
        }

        public int ParseAndExecuteCommand(string[] args)
        {
            var command = ParseCommand(args);

            if (command == null)
            {
                return 1;
            }

            return command.Execute() ? 0 : 1;
        }

        private CommandLineHelpCommand CreateDefaultCommand()
        {
            return new CommandLineHelpCommand(CommandRepository, new ConsoleOutput());
        }
    }
}