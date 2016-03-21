using System;
using JustCli.Helpers;

namespace JustCli.Outputs
{
    public class ColoredConsoleOutput : IOutput
    {
        public void WriteInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteSuccess(string message)
        {
            WriteColoredMessage(message, ConsoleColor.Green);
        }

        public void WriteWarning(string message)
        {
            WriteColoredMessage(message, ConsoleColor.Yellow);
        }

        public void WriteError(string message)
        {
            WriteColoredMessage(message, ConsoleColor.Red);

            if (ConsoleHelper.IsConsoleOutputRedirected())
            {
                Console.Error.WriteLine(message);
            }
        }

        public void WriteError(string message, Exception e)
        {
            WriteColoredMessage(string.Format("{0}: {1}", message, e.Message), ConsoleColor.Red);
        }

        private static void WriteColoredMessage(string message, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}