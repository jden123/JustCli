using System;

namespace JustCli
{
    public interface IOutput
    {
        void WriteError(string message);
        void WriteError(string message, Exception e);
        void WriteInfo(string message);
        void WriteLine(string message);
    }
}