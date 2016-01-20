using System;
using System.Collections.Generic;
using System.Linq;
using JustCli.Attributes;
using JustCli.Commands;
using JustCli.Dto;

namespace JustCli
{
    public class AssemblyCommandRepository : ICommandRepository
    {
        private readonly Dictionary<string, CommandInfo> commandNameToTypeMapping;

        public AssemblyCommandRepository()
        {
            commandNameToTypeMapping = FindCommands();
        }

        public Type GetCommandType(string commandName)
        {
            return commandNameToTypeMapping.ContainsKey(commandName.ToLower()) 
                ? commandNameToTypeMapping[commandName.ToLower()].Type 
                : null;
        }

        public List<CommandInfo> GetCommandsInfo()
        {
            return commandNameToTypeMapping.Select(commandPair => commandPair.Value).ToList();
        }

        public Dictionary<string, CommandInfo> FindCommands()
        {
            var result = new Dictionary<string, CommandInfo>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    // TODO:
                    if (type.GetInterfaces().Contains(typeof (ICommand)) && 
                        !type.IsAbstract &&
                        !type.IsInterface &&
                        type.GetCustomAttributes(typeof(CommandAttribute), true).Length > 0)
                    {
                        var commandInfo = CommandMetaDataHelper.GetCommandInfo(type);
                        if (result.ContainsKey(commandInfo.Name))
                        {
                            // warning
                        }
                        else
                        {
                            result[commandInfo.Name] = commandInfo;
                        }
                    }
                }
            }

            return result;
        }
    }
}