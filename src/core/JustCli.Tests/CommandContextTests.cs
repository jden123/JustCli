using System.Collections.Generic;
using JustCli.Dto;
using NSubstitute;
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
        
        [Test]
        public void ShouldGetValueFromAdditionalSource()
        {
            var argValueSource = Substitute.For<IArgValueSource>();
            argValueSource.GetArgValues()
                .Returns(new Dictionary<string, string>()
                         {
                             {"fromAdditionalSource", "10,25"}
                         });

            var commandContext = new CommandContext(Args, new []{argValueSource});

            var argumentInfo = new ArgumentInfo() { LongName = "fromAdditionalSource", ArgumentType = typeof(decimal), DefaultValue = "0.123"};
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual(typeof(decimal), argValue.GetType());
            Assert.AreEqual(10.25, argValue);
        }
        
        [Test]
        // override args
        public void ShouldGetValueFromTheLatestAdditionalSource()
        {
            var argValueSource1 = Substitute.For<IArgValueSource>();
            argValueSource1.GetArgValues()
                .Returns(new Dictionary<string, string>()
                         {
                             {"fromAdditionalSource", "10,25"}
                         });
            
            var argValueSource2 = Substitute.For<IArgValueSource>();
            argValueSource2.GetArgValues()
                .Returns(new Dictionary<string, string>()
                         {
                             {"fromAdditionalSource", "20,25"}
                         });

            var commandContext = new CommandContext(Args, new []{argValueSource1, argValueSource2});

            var argumentInfo = new ArgumentInfo() { LongName = "fromAdditionalSource", ArgumentType = typeof(decimal), DefaultValue = "0.123"};
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual(typeof(decimal), argValue.GetType());
            Assert.AreEqual(20.25, argValue);
        }
        
        [Test]
        // override from commandline
        public void ShouldGetValueFromCommandlineIfExists()
        {
            var argValueSource1 = Substitute.For<IArgValueSource>();
            argValueSource1.GetArgValues()
                .Returns(new Dictionary<string, string>()
                         {
                             {"fromAdditionalSource", "10,25"}
                         });
            
            var argValueSource2 = Substitute.For<IArgValueSource>();
            argValueSource2.GetArgValues()
                .Returns(new Dictionary<string, string>()
                         {
                             {"fromAdditionalSource", "20,25"}
                         });

            var args = new[] {"command", "--fromAdditionalSource", "30,25"};
            var commandContext = new CommandContext(args, new []{argValueSource1, argValueSource2});

            var argumentInfo = new ArgumentInfo() { LongName = "fromAdditionalSource", ArgumentType = typeof(decimal), DefaultValue = "0.123"};
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual(typeof(decimal), argValue.GetType());
            Assert.AreEqual(30.25, argValue);
        }
        
        [Test]
        public void ShouldParseAdditionalSourcesIfNoArgumentsInCommandline()
        {
            var argValueSource1 = Substitute.For<IArgValueSource>();
            argValueSource1.GetArgValues()
                .Returns(new Dictionary<string, string>()
                         {
                             {"fromAdditionalSource", "10,25"}
                         });
            
            var argValueSource2 = Substitute.For<IArgValueSource>();
            argValueSource2.GetArgValues()
                .Returns(new Dictionary<string, string>()
                         {
                             {"fromAdditionalSource", "20,25"}
                         });

            var args = new[] { "command" };
            var commandContext = new CommandContext(args, new []{argValueSource1, argValueSource2});

            var argumentInfo = new ArgumentInfo() { LongName = "fromAdditionalSource", ArgumentType = typeof(decimal), DefaultValue = "0.123"};
            var argValue = commandContext.GetArgValue(argumentInfo);

            Assert.AreEqual(typeof(decimal), argValue.GetType());
            Assert.AreEqual(20.25, argValue);
        }
    }
}
