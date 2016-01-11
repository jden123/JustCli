using System;

namespace JustCli
{
    public class ConsoleOutput : IOutput
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteError(string message, Exception e)
        {
            Console.WriteLine("{0}: {1}", message, e.Message);
        }

        public void WriteInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}