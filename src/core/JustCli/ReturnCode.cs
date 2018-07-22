using System.Threading.Tasks;

namespace JustCli
{
    public  class ReturnCode
    {
        public const int Success = 0;
        public const int Failure = 1;

    }

    public static class IntExtension
    {
        public static Task<int> ToAsync(this int value)
        {
            return Task.Factory.StartNew(() => { return value; });
        }
    }
}
