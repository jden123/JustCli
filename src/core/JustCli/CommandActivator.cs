using System;
using System.Collections.Generic;

namespace JustCli
{
    public class CommandActivator
    {
        public static object CreateCommand(
            Type commandType, 
            string[] args, 
            IOutput output, 
            IEnumerable<IArgValueSource> additionalArgValueSources = null)
        {
            var commandArgumentPropertyInfos = CommandMetaDataHelper.GetCommandArgumentPropertyInfos(commandType);

            var command = Activator.CreateInstance(commandType);

            CommandMetaDataHelper.SetOutputProperty(command, output);

            var commandContext = new CommandContext(args, additionalArgValueSources);

            var errors = new List<string>();
            foreach (var commandArgumentPropertyInfo in commandArgumentPropertyInfos)
            {
                try
                {
                    var commandArgumentAttribute = CommandMetaDataHelper.GetArgumentInfo(commandArgumentPropertyInfo);
                    var value = commandContext.GetArgValue(commandArgumentAttribute);
                    commandArgumentPropertyInfo.SetValue(command, value, null);
                }
                catch (Exception e)
                {
                    errors.Add(e.Message);
                }
            }

            if (errors.Count > 0)
            {
                throw new Exception(string.Join(Environment.NewLine, errors));
            }

            return command;
        }
    }
}