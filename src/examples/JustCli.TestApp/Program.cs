using System.Threading.Tasks;

namespace JustCli.TestApp
{
   class Program
   {
      static async Task<int> Main(string[] args)
      {
          return await CommandLineParser.Default.ParseAndExecuteCommand(args);          
      }
   }
}
