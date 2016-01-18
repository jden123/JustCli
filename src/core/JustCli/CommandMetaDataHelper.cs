using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JustCli.Attributes;
using JustCli.Dto;

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

        public static List<ArgumentInfo> GetCommandArgumentInfos(Type commandType)
        {
            var commandArgumentInfos = new List<ArgumentInfo>();
            foreach (var propertyInfo in commandType.GetProperties())
            {
                var attribute = propertyInfo.GetCustomAttributes(typeof(CommandArgumentAttribute), true).FirstOrDefault();
                if (attribute == null)
                {
                    continue;
                }

                var commandArgumentAttribute = (CommandArgumentAttribute) attribute;

                commandArgumentInfos.Add(new ArgumentInfo()
                {
                    ShortName = commandArgumentAttribute.ShortName, 
                    LongName = commandArgumentAttribute.LongName, 
                    Description = commandArgumentAttribute.Description, 
                    DefaultValue = commandArgumentAttribute.DefaultValue,
                    ArgumentType = propertyInfo.PropertyType
                });
            }

            return commandArgumentInfos;
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
                Description = commandArgumentAttribute.Description,
                DefaultValue = commandArgumentAttribute.DefaultValue,
            };
        }

        public static bool SetOutputProperty(object command, IOutput output)
        {
            var outputProperties = GetOutputProperty(command.GetType());
            if (outputProperties.Length == 0)
            {
                return false;
            }

            foreach (var outputProperty in outputProperties)
            {
                outputProperty.SetValue(command, output, null);
            }

            return true;
        }

        private static PropertyInfo[] GetOutputProperty(Type commandType)
        {
            return commandType
                .GetProperties()
                .Where(p => p.PropertyType == typeof(IOutput) && 
                            p.GetCustomAttributes(typeof (CommandOutputAttribute), true).Length > 0)
                .ToArray();
        }
    }
}