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
        public void CommandHelpCommandShouldReturnAllCommandInfo()
        {
            var memoryOutput = new MemoryOutput();
            var commandHelpCommand = new CommandHelpCommand(typeof(DoSomethingNTimesCommand), memoryOutput);

            commandHelpCommand.Execute();

            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("Do something n times.")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("  That's long description line 1.")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("  That's long description line 2.")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("-a")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("--action")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("Defines what should be done.")));

            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("-r")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("--repeat")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.Contains("Number of repeats.")));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.IndexOf("Default", StringComparison.OrdinalIgnoreCase) >= 0));
        }

        [Test]
        public void CommandHelpCommandShouldIncludePropertyType()
        {
            var memoryOutput = new MemoryOutput();
            var commandHelpCommand = new CommandHelpCommand(typeof(DoSomethingNTimesCommand), memoryOutput);

            commandHelpCommand.Execute();

            Assert.IsTrue(memoryOutput.Content.Any(l => l.IndexOf("string", StringComparison.OrdinalIgnoreCase) >= 0));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.IndexOf("int", StringComparison.OrdinalIgnoreCase) >= 0));
        }

        [Test]
        public void CommandHelpCommandShouldIncludeEnumValues()
        {
            var memoryOutput = new MemoryOutput();
            var commandHelpCommand = new CommandHelpCommand(typeof(DoSomethingWithEnumCommand), memoryOutput);

            commandHelpCommand.Execute();

            Assert.IsTrue(memoryOutput.Content.Any(l => l.IndexOf("[values: Value1,Value2,Value3]", StringComparison.OrdinalIgnoreCase) >= 0));
            Assert.IsTrue(memoryOutput.Content.Any(l => l.IndexOf("[default: Value2]", StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
}
