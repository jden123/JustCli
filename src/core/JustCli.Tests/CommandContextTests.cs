using JustCli.Dto;
using NUnit.Framework;

namespace JustCli.Tests
{
    [TestFixture]
   public class CommandContextTests
    {
        private readonly string[] Args = new[]
        {
            "command", 
            "-s", "short", 
            "--long", "long value", 
            "-f"
        };

        [Test]
        public void ShouldReturnArgumentValueByShortName()
        {
            var commandContext = new CommandContext(Args);

            var argumentInfo = new ArgumentInfo() { ShortName = "s", ArgumentType = typeof(string) };
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual("short", argValue);
        }

        [Test]
        public void ShouldReturnArgumentValueByLongName()
        {
            var commandContext = new CommandContext(Args);

            var argumentInfo = new ArgumentInfo() { LongName = "long", ArgumentType = typeof(string) };
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual("long value", argValue);
        }

        [Test]
        public void ShouldReturnDefaultArgumentValue()
        {
            var commandContext = new CommandContext(Args);

            var argumentInfo = new ArgumentInfo() { LongName = "unset-arg", ArgumentType = typeof(string), DefaultValue = "unset" };
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual("unset", argValue);
        }

        [Test]
        public void ShouldReturnArgumentValueByShortNameIfSetBoth()
        {
            var commandContext = new CommandContext(Args);

            var argumentInfo = new ArgumentInfo() { ShortName = "s", LongName = "long", ArgumentType = typeof(string) };
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual("short", argValue);
        }

        [Test]
        public void ShouldReturnFlagArgumentValueByShortName()
        {
            var commandContext = new CommandContext(Args);

            var argumentInfo = new ArgumentInfo() { ShortName = "f", ArgumentType = typeof(bool) };
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual(true, argValue);
        }

        [Test]
        public void ShouldGetValueFromStringDefaultValueForNonPrimitiveTypes()
        {
            var commandContext = new CommandContext(Args);

            var argumentInfo = new ArgumentInfo() { ShortName = "npt", ArgumentType = typeof(decimal), DefaultValue = "0.123"};
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual(typeof(decimal), argValue.GetType());
            Assert.AreEqual(0.123, argValue);
        }
    }
}
