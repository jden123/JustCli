using System;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
   [Command("DoSomethingWithEnumCommand", "The command for enum values testing.")]
   public class DoSomethingWithEnumCommand : ICommand
   {
      [CommandArgument("d", "default", DefaultValue = TestEnum.Value2)]
      public TestEnum Empty { get; private set; }

      [CommandOutput]
      public IOutput Output { get; set; }

      public int Execute()
      {
         throw new NotImplementedException();
      }
   }
}
