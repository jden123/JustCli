using System;

namespace JustCli.Helpers
{
    public static class ConsoleHelper
    {
        /// <summary>
        /// Determines whether output is redirected.
        /// http://stackoverflow.com/questions/743885/how-can-i-determine-whether-console-out-has-been-redirected-to-a-file
        /// http://suanfazu.com/t/how-can-i-determine-whether-console-out-has-been-redirected-to-a-file/10649
        /// Calling CursorVisible throws an exception when the console is redirected.
        /// .NET 4.5 has Console.IsConsoleOutputRedirected and Console.IsErrorRedirected.
        /// </summary>
        /// <returns></returns>
        public static bool IsConsoleOutputRedirected()
        {
            try
            {
                var cursorVisible = Console.CursorVisible;
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
