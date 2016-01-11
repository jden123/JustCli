using System;
using System.Collections.Generic;

namespace JustCli
{
    public interface ICommandRepository
    {
        Type GetCommandType(string commandName);
        List<CommandInfo> GetCommandsInfo();
    }
}