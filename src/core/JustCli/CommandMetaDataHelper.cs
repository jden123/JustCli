using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JustCli.Attributes;

namespace JustCli
{
    public static class CommandMetaDataHelper
    {
        public static CommandInfo GetCommandInfo(Type type)
        {
            var commandInfo = new CommandInfo();

            var commandAttribute = (CommandAttribute)type.GetCustomAttributes(typeof (CommandAttribute), true).FirstOrDefault();
            if (commandAttribute == null)
            {
                commandInfo.Name = type.Name.ToLower();
            }
            else
            {
                commandInfo.Name = commandAttribute.CommandName.ToLower();
                commandInfo.Description = commandAttribute.CommandDescription;
            }

            commandInfo.Type = type;

            return commandInfo;
        }

        public static List<CommandArgumentAttribute> GetCommandArgumentInfos(Type commandType)
        {
            var commandArgumentPropertyInfos = commandType.GetProperties()
                .Select(p => p.GetCustomAttributes(typeof (CommandArgumentAttribute), true).FirstOrDefault())
                .Where(a => a != null)
                .Cast<CommandArgumentAttribute>()
                .ToList();

            return commandArgumentPropertyInfos;
        }

        public static IEnumerable<PropertyInfo> GetCommandArgumentPropertyInfos(Type commandType)
        {
            var commandArgumentPropertyInfos = commandType
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof (CommandArgumentAttribute), true).Length > 0);
            return commandArgumentPropertyInfos;
        }

        public static ArgumentInfo GetArgumentInfo(PropertyInfo commandArgumentPropertyInfo)
        {
            var commandArgumentAttribute = (CommandArgumentAttribute)commandArgumentPropertyInfo
                .GetCustomAttributes(typeof (CommandArgumentAttribute), true)
                .First();

            return new ArgumentInfo()
            {
                ShortName = commandArgumentAttribute.ShortName,
                LongName = commandArgumentAttribute.LongName,
                DescriptionName = commandArgumentAttribute.Description,
                DefaultValue = commandArgumentAttribute.DefaultValue,
            };
        }
    }
}