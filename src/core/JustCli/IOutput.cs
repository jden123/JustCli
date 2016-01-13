using System;

namespace JustCli
{
    public interface IOutput
    {
        void WriteInfo(string message);
        void WriteSuccess(string message);
        void WriteWarning(string message);
        void WriteError(string message);
        void WriteError(string message, Exception e);
    }
}