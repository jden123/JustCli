using System;
using System.Linq;
using JustCli.Commands;
using JustCli.Tests.Commands;
using NUnit.Framework;

namespace JustCli.Tests
{
    [TestFixture]
    public class CommandHelpCommandTests
    {
        [Test]
        public void CommandLineHelpCommandShouldReturnAllCommandInfo()
        {
            var memoryOutput = new MemoryOutput();
            var commandLineHelpCommand = new CommandHelpCommand(typeof(DoSomethingNTimesCommand), memoryOutput);

            commandLineHelpCommand.Execute();

            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("Do something n times.")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("-a")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("--action")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("Defines what should be done.")));
            
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("-r")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("--repeat")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("Number of repeats.")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.IndexOf("Default", StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
}
