using System;
using System.Collections.Generic;
using System.Linq;
using JustCli.Attributes;
using JustCli.Dto;

namespace JustCli
{
    public class AssemblyCommandRepository : ICommandRepository
    {
        private IOutput Output { get; set; }
        private readonly Dictionary<string, CommandInfo> commandNameToTypeMapping;

        public AssemblyCommandRepository(IOutput output = null)
        {
            Output = output;
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

        private Dictionary<string, CommandInfo> FindCommands()
        {
            var result = new Dictionary<string, CommandInfo>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes().Where(TypeIsCommand))
                {
                    var commandInfo = CommandMetaDataHelper.GetCommandInfo(type);
                    if (result.ContainsKey(commandInfo.Name))
                    {
                        if (Output != null)
                        {
                            Output.WriteWarning(string.Format("Command with name [{0}]({1}) already exists.", commandInfo.Name, type.FullName));
                        }
                    }
                    else
                    {
                        result[commandInfo.Name] = commandInfo;
                    }
                }
            }

            return result;
        }

        private static bool TypeIsCommand(Type type)
        {
            return (type.GetInterfaces().Contains(typeof(ICommand)) || 
                        type.GetInterfaces().Contains(typeof(ICommandAsync))) &&
                   !type.IsAbstract &&
                   !type.IsInterface &&
                   type.GetCustomAttributes(typeof(CommandAttribute), true).Length > 0;
        }
    }
}