using System.Threading.Tasks;

namespace JustCli
{
   public interface ICommandAsync
   {
      Task<int> ExecuteAsync();
   }
}