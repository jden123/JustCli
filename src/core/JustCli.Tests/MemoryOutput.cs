using System;
using System.Collections.Generic;

namespace JustCli.Tests
{
    public class MemoryOutput : IOutput
    {
        public List<string> Content = new List<string>();

        public void WriteWarning(string message)
        {
            Content.Add(message);
        }

        public void WriteError(string message)
        {
            Content.Add(message);
        }

        public void WriteError(string message, Exception e)
        {
            Content.Add(string.Format("{0}: {1}", message, e.Message));
        }

        public void WriteInfo(string message)
        {
            Content.Add(message);
        }

        public void WriteSuccess(string message)
        {
            Content.Add(message);
        }

        public void WriteLine(string message)
        {
            Content.Add(message);
        }
    }
}