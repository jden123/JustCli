﻿using System.Collections.Generic;
using System.Linq;
using JustCli.Commands;
using JustCli.Dto;
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

        [Test]
        public void CommandLineHelpCommandShouldShowNoCommandsMessageIfThereAreNoCommands()
        {
            var emptyCommandRepository = Substitute.For<ICommandRepository>();
            emptyCommandRepository.GetCommandsInfo().Returns(new List<CommandInfo>());
           
            var memoryOutput = new MemoryOutput();
            var commandLineHelpCommand = new CommandLineHelpCommand(emptyCommandRepository, memoryOutput);

            commandLineHelpCommand.Execute();

            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("There are no commands.")));
        }

        [Test]
        public void CommandLineHelpCommandShouldUseOrder()
        {
            var commandRepository = Substitute.For<ICommandRepository>();
            commandRepository.GetCommandsInfo()
                .Returns(new List<CommandInfo>()
                {
                    new CommandInfo() {Name = "command1", Description = "The first command.", Order = 2},
                    new CommandInfo() {Name = "command2", Order = 1},
                });

            var memoryOutput = new MemoryOutput();
            var commandLineHelpCommand = new CommandLineHelpCommand(commandRepository, memoryOutput);

            commandLineHelpCommand.Execute();

            var command1position = memoryOutput.Content.IndexOf(memoryOutput.Content.First(l => l.Contains("command1")));
            var command2position = memoryOutput.Content.IndexOf(memoryOutput.Content.First(l => l.Contains("command2")));

            Assert.IsTrue(command1position > command2position);
        }
    }
}
