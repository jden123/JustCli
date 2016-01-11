using System.Collections.Generic;
using System.Linq;
using JustCli.Commands;
using NSubstitute;
using NUnit.Framework;

namespace JustCli.Tests
{
    [TestFixture]
    public class CommandLineHelpCommandTests
    {
        private ICommandRepository _commandRepository;
        private CommandLineParser _commandLineParser;

        public CommandLineHelpCommandTests()
        {
            _commandRepository = Substitute.For<ICommandRepository>();
            _commandRepository.GetCommandsInfo()
                .Returns(new List<CommandInfo>()
                {
                    new CommandInfo() {Name = "command1", Description = "The first command."},
                    new CommandInfo() {Name = "command2"},
                });

            _commandLineParser = new CommandLineParser(_commandRepository);
        }


        [Test]
        public void CommandLineHelpCommandShouldReturnAllCommandInfo()
        {
            var memoryOutput = new MemoryOutput();
            var commandLineHelpCommand = new CommandLineHelpCommand(_commandRepository, memoryOutput);

            commandLineHelpCommand.Execute();

            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("command1")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("command2")));
        }

    }
}
