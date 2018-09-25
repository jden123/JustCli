using System.Threading.Tasks;

namespace JustCli.TestAppAsync
{
   internal class Program
   {
      public static async Task<int> Main(string[] args)
      {
         return await CommandLineParser.Default.ParseAndExecuteCommandAsync(args);
      }
   }
}