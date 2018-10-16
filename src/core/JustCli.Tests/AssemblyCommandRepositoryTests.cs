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
        public void AssemblyCommandRepositoryShouldNotUseCommandWithoutCommandAttribute()
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandType = assemblyCommandRepository.GetCommandType("SecondCommand");

            Assert.IsNull(commandType);
        }

        [Test]
        public void AssemblyCommandRepositoryShouldBeCaseInsensetive()
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandType = assemblyCommandRepository.GetCommandType("COMmand1");

            Assert.IsNotNull(commandType);
            Assert.AreEqual(typeof(FirstCommand), commandType);
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
        
        [Test]
        public void AssemblyCommandRepositoryShouldFindAsyncCommand()
        {
            var assemblyCommandRepository = new AssemblyCommandRepository();

            var commandType = assemblyCommandRepository.GetCommandType("asynccommand");

            Assert.IsNotNull(commandType);
            Assert.AreEqual(typeof(AsyncCommand), commandType);
            Assert.IsNotNull(commandType.GetInterface("ICommandAsync"));
        }
    }
}
