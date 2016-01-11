using System;
using JustCli.Commands;
using JustCli.Tests.Commands;
using NSubstitute;
using NUnit.Framework;

namespace JustCli.Tests
{
    [TestFixture]
    public class CommandLineParserTests
    {
        private ICommandRepository _commandRepository;
        private CommandLineParser _commandLineParser;

        public CommandLineParserTests()
        {
            _commandRepository = Substitute.For<ICommandRepository>();
            _commandRepository.GetCommandType("dosomething").Returns(typeof(DoSomethingCommand));
            _commandRepository.GetCommandType("dosomething-ntimes").Returns(typeof(DoSomethingNTimesCommand));

            _commandLineParser = new CommandLineParser(_commandRepository);
        }

        [Test]
        public void ParserShouldReturnCommandLineHelpCommandForEmptyCommandLine()
        {
            string[] args = new string[0];

            var command = _commandLineParser.ParseCommand(args);

            Assert.IsNotNull(command);
            Assert.IsInstanceOf<CommandLineHelpCommand>(command);
        }

        [Test]
        public void ParserShouldReturnCommandLineHelpCommandForUnknownCommand()
        {
            string[] args = new[] { "unknown" };

            var command = _commandLineParser.ParseCommand(args);

            Assert.IsNotNull(command);
            Assert.IsInstanceOf<CommandLineHelpCommand>(command);
        }

        [TestCase("?")]
        [TestCase("-?")]
        [TestCase("-h")]
        [TestCase("--help")]
        public void ParserShouldReturnCommandLineHelpCommandForHelpCommandLine(string helpArg)
        {
            string[] args = new[] { helpArg };

            var command = _commandLineParser.ParseCommand(args);

            Assert.IsInstanceOf<CommandLineHelpCommand>(command);
        }

        [Test]
        public void ParserShouldReturnCommandForKnownCommandWithDefaultValue()
        {
            string[] args = new[] { "dosomething" };

            var command = _commandLineParser.ParseCommand(args);

            Assert.IsNotNull(command);
            Assert.IsInstanceOf<ICommand>(command);

            var doSomethingCommand = (DoSomethingCommand)command;
            Assert.AreEqual("default", doSomethingCommand.Action);
        }

        [Test]
        public void ParserShouldParseArgsShortName()
        {
            string[] args = new[] { "dosomething", "-a", "something" };

            var command = _commandLineParser.ParseCommand(args);

            Assert.IsNotNull(command);
            Assert.IsInstanceOf<ICommand>(command);

            Assert.IsInstanceOf<DoSomethingCommand>(command);

            var doSomethingCommand = (DoSomethingCommand) command;
            Assert.AreEqual("something", doSomethingCommand.Action);
        }

        [Test]
        public void ParserShouldParseArgsLongName()
        {
            string[] args = new[] { "dosomething", "--action", "something" };

            var command = _commandLineParser.ParseCommand(args);

            Assert.IsNotNull(command);
            Assert.IsInstanceOf<ICommand>(command);

            Assert.IsInstanceOf<DoSomethingCommand>(command);

            var doSomethingCommand = (DoSomethingCommand) command;
            Assert.AreEqual("something", doSomethingCommand.Action);
        }

        [Test]
        public void ParserShouldParseIntegerArg()
        {
            string[] args = new[] { "dosomething-ntimes", "--action", "something", "-r", "5" };

            var command = _commandLineParser.ParseCommand(args);

            Assert.IsNotNull(command);
            Assert.IsInstanceOf<ICommand>(command);

            Assert.IsInstanceOf<DoSomethingNTimesCommand>(command);

            var doSomethingNTimesCommand = (DoSomethingNTimesCommand)command;
            Assert.AreEqual("something", doSomethingNTimesCommand.Action);
            Assert.AreEqual(5, doSomethingNTimesCommand.Repeat);
        }

        [Test]
        public void ParserShouldThrowErrorIfArgIsNotSet()
        {
            string[] args = new[] { "dosomething-ntimes" };
            Assert.Throws<Exception>(() => _commandLineParser.ParseCommand(args));
        }

        [TestCase("?")]
        [TestCase("-?")]
        [TestCase("-h")]
        [TestCase("--help")]
        public void ParserShouldReturnCommandHelpCommandForHelpCommand(string helpArg)
        {
           string[] args = new[] { "dosomething-ntimes", helpArg };

           var command = _commandLineParser.ParseCommand(args);

           Assert.IsInstanceOf<CommandHelpCommand>(command);
        }
    }
}
