#if !NET40
using System.Threading.Tasks;

namespace JustCli
{
   public interface ICommandAsync
   {
      Task<int> ExecuteAsync();
   }
}
#endif