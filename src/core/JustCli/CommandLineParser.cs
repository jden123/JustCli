using System;
using System.Linq;
using JustCli.Commands;

namespace JustCli
{
    // TODO: queue instead of []. Command context + parser.
    // FEATURE: setup utility info(like author and etc)
    // FEATURE: use ILMerge to have one utility file(cmd.exe)
    // FEATURE: nuget package
    // FEATURE: indexed command context
    // FEATURE: Add command option. Command argument is positional argument. Option is get by name.
    // FEATURE: Command help method.
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

            return CommandActivator.CreateCommand(commandType, args);
        }

        public int ParseAndExecuteCommand(string[] args)
        {
            var command = Default.ParseCommand(args);
            return command.Execute() ? 0 : 1;
        }

        private CommandLineHelpCommand CreateDefaultCommand()
        {
            return new CommandLineHelpCommand(CommandRepository, new ConsoleOutput());
        }
    }
}