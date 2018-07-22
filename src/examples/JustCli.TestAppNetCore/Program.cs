using JustCli;
using System;
using System.Threading.Tasks;

namespace JustCli.TestAppNetCore
{
   class Program
   {
      static async Task<int> Main(string[] args)
      {
         return await CommandLineParser.Default.ParseAndExecuteCommand(args);
      }
   }
}
