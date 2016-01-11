using System;

namespace JustCli
{
    public class CommandActivator
    {
        public static ICommand CreateCommand(Type commandType, string[] args)
        {
            var commandArgumentPropertyInfos = CommandMetaDataHelper.GetCommandArgumentPropertyInfos(commandType);

            var command = (ICommand)Activator.CreateInstance(commandType);

            var commandContext = new CommandContext(args);

            foreach (var commandArgumentPropertyInfo in commandArgumentPropertyInfos)
            {
                var commandArgumentAttribute = CommandMetaDataHelper.GetArgumentInfo(commandArgumentPropertyInfo);

                var value = commandContext.GetArgValue(
                                commandArgumentAttribute, 
                                commandArgumentPropertyInfo.PropertyType);

                commandArgumentPropertyInfo.SetValue(command, value, null);
            }

            return command;
        }
    }
}