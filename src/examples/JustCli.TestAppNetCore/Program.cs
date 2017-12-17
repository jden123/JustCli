using JustCli;
using System;

namespace JustCli.TestAppNetCore
{
   class Program
   {
      static int Main(string[] args)
      {
         return CommandLineParser.Default.ParseAndExecuteCommand(args);
      }
   }
}
