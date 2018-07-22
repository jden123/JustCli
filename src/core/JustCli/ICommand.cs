using System.Threading.Tasks;

namespace JustCli
{
    public interface ICommand
    {
       // int Execute();
        Task<int> Execute();
    }
}