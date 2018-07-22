using System.Threading.Tasks;

namespace JustCli.TestApp
{
   class Program
   {
      static async Task<int> Main(string[] args)
      {
          return CommandLineParser.Default.ParseAndExecuteCommand(args);
          //return await CommandLineParser.Default.ParseAndExecuteCommandAsync(args);
        }
   }
}
