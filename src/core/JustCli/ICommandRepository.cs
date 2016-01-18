using System;
using System.Collections.Generic;
using JustCli.Dto;

namespace JustCli
{
    public interface ICommandRepository
    {
        Type GetCommandType(string commandName);
        List<CommandInfo> GetCommandsInfo();
    }
}