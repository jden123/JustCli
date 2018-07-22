using System.IO;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
    [Command("fcd", "Prints file creation date time.")]
    class ShowFileCreationDateCommand : ICommand
    {
        // NOTE: it's required.
        [CommandArgument("f", "file", Description = "File path.")]
        public string FilePath { get; set; }
        
        [CommandOutput]
        public IOutput Output { get; set; }

        public Task<int> Execute()
        {
            if (!File.Exists(FilePath))
            {
                Output.WriteError(string.Format("The file[{0}] does not exist. Please check the file path.", FilePath));
                return ReturnCode.Failure.ToAsync();
            }

            Output.WriteSuccess(string.Format("Utc file creation time is {0}", File.GetCreationTimeUtc(FilePath)));
            return ReturnCode.Success.ToAsync();
        }
    }
}
