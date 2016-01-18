using JustCli.Tests.Commands;
using NUnit.Framework;

namespace JustCli.Tests
{
    [TestFixture]
    public class CommandMetaDataHelperTests
    {
        [Test]
        public void ShouldSetOtputIfThePropertyExists()
        {
            var commandWithOutput = new DoSomethingCommand();
            var output = new MemoryOutput();

            var result = CommandMetaDataHelper.SetOutputProperty(commandWithOutput, output);
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldNotSetOtputIfThePropertyDoesNotExist()
        {
            var commandWithoutOutput = new DoSomethingNTimesCommand();
            var output = new MemoryOutput();

            Assert.IsFalse(CommandMetaDataHelper.SetOutputProperty(commandWithoutOutput, output));
        }
    }
}
