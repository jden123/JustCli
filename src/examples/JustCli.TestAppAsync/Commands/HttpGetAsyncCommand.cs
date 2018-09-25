using JustCli.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustCli.TestApp.Commands
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