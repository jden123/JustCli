using System.Linq;
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
            _commandRepository.GetCommandType("ex").Returns(typeof(ThrowExceptionCommand));

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

            var doSomethingCommand = (DoSomethingCommand)command;
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

            var doSomethingCommand = (DoSomethingCommand)command;
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
        public void ParserShouldNotThrowErrorIfArgIsNotSet()
        {
            string[] args = new[] { "dosomething-ntimes" };
            Assert.DoesNotThrow(() => _commandLineParser.ParseCommand(args));
        }

        [Test]
        public void ParseAndExecuteCommandShouldNotThrowErrorIfArgIsNotSet()
        {
            string[] args = new[] { "dosomething-ntimes" };
            Assert.AreNotEqual(0, new CommandLineParser(_commandRepository).ParseAndExecuteCommand(args));
        }

        [Test]
        public void ParseAndExecuteCommandShouldReturnErrorCodeIfArgIsNotSet()
        {
            string[] args = new[] { "dosomething-ntimes" };
            Assert.AreNotEqual(0, new CommandLineParser(_commandRepository).ParseAndExecuteCommand(args));
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

        [Test]
        public void ParserShouldCatchCommandException()
        {
            string[] args = new[] { "ex" };
            Assert.DoesNotThrow(() => _commandLineParser.ParseAndExecuteCommand(args));
        }

        [Test]
        public void ParserShouldReturnFailureOnException()
        {
            string[] args = new[] { "ex" };
            Assert.AreEqual(ReturnCode.Failure, _commandLineParser.ParseAndExecuteCommand(args));
        }

        [Test]
        public void ParserShouldReturnFailureOnException11()
        {
            string[] args = new[] { "dosomething-ntimes", "-r", "d" };
            var memoryOutput = new MemoryOutput();
            var commandLineParser = new CommandLineParser(_commandRepository, memoryOutput);
            Assert.AreEqual(ReturnCode.Failure, commandLineParser.ParseAndExecuteCommand(args));
            Assert.True(memoryOutput.Content.Any(l => l.Contains("action")));
            Assert.True(memoryOutput.Content.Any(l => l.Contains("repeat")));
        }
    }
}
