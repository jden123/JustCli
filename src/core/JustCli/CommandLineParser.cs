using System;
using System.Linq;
using System.Threading.Tasks;
using JustCli.Commands;
using JustCli.Outputs;

namespace JustCli
{
    public class CommandLineParser
    {
        private static readonly string[] HelpCommandAliases = new[] { "?", "-?", "-h", "--help" }; 

        public ICommandRepository CommandRepository { get; set; }

        private static readonly Lazy<CommandLineParser> defaultParser =
           new Lazy<CommandLineParser>(
               () =>
                   {
                       var output = new ColoredConsoleOutput();
                       return new CommandLineParser(new AssemblyCommandRepository(output), output);
                   });

        public static CommandLineParser Default { get { return defaultParser.Value; } }

        private IOutput Output { get; set; }

        public bool ShowExceptionStackTrace { get; set; }

        private readonly List<IArgValueSource> _additionalArgValueSources = new List<IArgValueSource>();
        
        public CommandLineParser(ICommandRepository commandRepository, IOutput output = null)
        {
            CommandRepository = commandRepository;
            Output = output ?? new ColoredConsoleOutput();
            ShowExceptionStackTrace = false;
        }

        public void AddAdditionalArgValueSource(IArgValueSource argValueSource)
        {
            _additionalArgValueSources.Add(argValueSource);
        }
        
        public object ParseCommand(string[] args)
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
                Output.WriteError("Command does not exist.");
                return CreateDefaultCommand();
            }

            // NOTE: special case like cmd.exe do -?.
            if (args.Length > 1 && HelpCommandAliases.Contains(args[1]))
            {
                return new CommandHelpCommand(commandType, Output);
            }

            try
            {
                return CommandActivator.CreateCommand(commandType, args, Output, _additionalArgValueSources);
            }
            catch (Exception e)
            {
                Output.WriteError(string.Format("Cannot setup [{0}] command.", commandName));
                Output.WriteError(e.Message);
                return null;

                // NOTE: we can show command help. Do we need?
                //return new CommandHelpCommand(commandType, new ConsoleOutput());
            }
        }

        public int ParseAndExecuteCommand(string[] args)
        {
            var command = ParseCommand(args);

            // NOTE: the error code should send the command.
            if (command == null)
            {
                return ReturnCode.Failure;
            }

            try
            {
                if (command is ICommand syncCommand)
                {
                    return syncCommand.Execute();
                }
                
                return ReturnCode.Failure;
            }
            catch (Exception e)
            {
                Output.WriteError(e.Message);
                if (ShowExceptionStackTrace)
                {
                    Output.WriteError(e.StackTrace);
                }
                
                return ReturnCode.Failure;
            }
        }
        
#if !NET40
        public async Task<int> ParseAndExecuteCommandAsync(string[] args)
        {
            var command = ParseCommand(args);

            // NOTE: the error code should send the command.
            if (command == null)
            {
                return ReturnCode.Failure;
            }

            try
            {
                if (command is ICommandAsync asyncCommand)
                {
                    return await asyncCommand.ExecuteAsync();
                }
                
                if (command is ICommand syncCommand)
                {
                    return await Task.Factory.StartNew(() => syncCommand.Execute());
                }
                
                return ReturnCode.Failure;
            }
            catch (Exception e)
            {
                Output.WriteError(e.Message);
                if (ShowExceptionStackTrace)
                {
                    Output.WriteError(e.StackTrace);
                }
                
                return ReturnCode.Failure;
            }
        }
#endif
        
        private CommandLineHelpCommand CreateDefaultCommand()
        {
            return new CommandLineHelpCommand(CommandRepository, Output);
        }
    }
}