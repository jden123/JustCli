using System;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.Tests.Commands
{
   [Command("DoSomethingWithGuids", "The command for Guid default values testing.")]
   public class DoSomethingWithGuidsCommand : ICommand
   {
      [CommandArgument("empty", "emptyguid", DefaultValue = "empty")]
      public Guid Empty { get; private set; }

      [CommandArgument("guid", "guid", DefaultValue = "e0f5747e-956f-4b23-9c54-dd5a6b2cd05f")]
      public Guid Guid { get; private set; }

      [CommandOutput]
      public IOutput Output { get; set; }

      public int Execute()
      {
         throw new NotImplementedException();
      }

        public Task<int> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}