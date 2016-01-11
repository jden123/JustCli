using System;
using System.IO;
using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
    [Command("fcd", "Prints file creation date time.")]
    class ShowFileCreationDateCommand : ICommand
    {
        // NOTE: it's required.
        [CommandArgument("f", "file", Description = "File path.")]
        public string FilePath { get; set; }

        public bool Execute()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("The file[{0}] does not exist. Please check the file path.", FilePath);
                return false;
            }

            Console.WriteLine("Utc file creation time is {0}", File.GetCreationTimeUtc(FilePath));
            return true;
        }
    }
}
