using System;

namespace JustCli.Outputs
{
    public class ConsoleOutput : IOutput
    {
        public void WriteInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteSuccess(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteError(string message, Exception e)
        {
            Console.WriteLine("{0}: {1}", message, e.Message);
        }
    }
}