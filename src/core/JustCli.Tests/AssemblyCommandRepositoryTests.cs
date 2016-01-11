using System.Linq;
using JustCli.Tests.Commands;
using NUnit.Framework;

namespace JustCli.Tests
{
    [TestFixture]
    public class AssemblyCommandRepositoryTests
    {
        [Test]
        public void AssemblyCommandRepositoryShouldUseCommandAttribute()
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandType = assemblyCommandRepository.GetCommandType("command1");

            Assert.IsNotNull(commandType);
            Assert.AreEqual(typeof(FirstCommand), commandType);
        }

        [Test]
        public void AssemblyCommandRepositoryShouldUseCommandClassNameIfNameIsNotSet()
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandType = assemblyCommandRepository.GetCommandType("SecondCommand");

            Assert.IsNotNull(commandType);
            Assert.AreEqual(typeof(SecondCommand), commandType);
        }

        [Test]
        public void AssemblyCommandRepositoryShouldBeCaseInsensetive()
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandType = assemblyCommandRepository.GetCommandType("secondcommand");

            Assert.IsNotNull(commandType);
            Assert.AreEqual(typeof(SecondCommand), commandType);
        }

        [Test]
        public void AssemblyCommandRepositoryShouldReturnAllCommandsInfo()
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandsInfo = assemblyCommandRepository.GetCommandsInfo();
            
            Assert.IsNotNull(commandsInfo);

            var command1 = commandsInfo.Single(c => c.Name == "command1");
            Assert.AreEqual("The first command.", command1.Description);
        }

        [TestCase("CommandLineHelpCommand")]
        [TestCase("CommandHelpCommand")]
        public void AssemblyCommandRepositoryShouldIgnoreInternalCommands(string commandName)
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandsInfo = assemblyCommandRepository.GetCommandsInfo();

            Assert.IsNull(commandsInfo.FirstOrDefault(c => c.Name == commandName.ToLower()));
        }
    }
}
