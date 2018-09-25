using System.Net.Http;
using System.Threading.Tasks;
using JustCli.Attributes;

namespace JustCli.TestAppAsync.Commands
{
   [Command("httpget", "Gets Url")]
   class HttpGetAsyncCommand : ICommandAsync
   {
      [CommandArgument("u", "Url", Description = "Url", DefaultValue = "http://c2.com")]
      public string Url{ get; set; }
      
      [CommandOutput]
      public IOutput Output { get; set; }
      
      public async Task<int> ExecuteAsync()
      {
         HttpClient http = new HttpClient();
         var res = await http.GetStringAsync(Url);
         Output.WriteInfo(res);
         return ReturnCode.Success;
      }
   }
}