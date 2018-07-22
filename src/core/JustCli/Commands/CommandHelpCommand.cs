using System;
using System.Text;
using System.Threading.Tasks;

namespace JustCli.Commands
{
   public class CommandHelpCommand : ICommand
   {
       public Type CommandType { get; set; }
       public IOutput Output { get; set; }

       public CommandHelpCommand(Type commandType, IOutput output)
       {
           CommandType = commandType;
           Output = output;
       }

       public Task<int> Execute()
       {
           var helpStringBuilder = new StringBuilder();

           var commandInfo = CommandMetaDataHelper.GetCommandInfo(CommandType);
           helpStringBuilder.Append(commandInfo.Name);
           if (!string.IsNullOrWhiteSpace(commandInfo.Description))
           {
               helpStringBuilder.AppendFormat(" - {0}", commandInfo.Description);
           }

           Output.WriteInfo(helpStringBuilder.ToString());

           var commandArgumentPropertyInfos = CommandMetaDataHelper.GetCommandArgumentInfos(CommandType);

           if (commandArgumentPropertyInfos.Count > 0)
           {
               Output.WriteInfo("Options:");
           }

           foreach (var argumentInfo in commandArgumentPropertyInfos)
           {
               var likeTabSeparator = "  ";

               var propertyHelpStringBuilder = new StringBuilder();
               propertyHelpStringBuilder.AppendFormat("{0}-{1}", likeTabSeparator, argumentInfo.ShortName);

               if (!string.IsNullOrWhiteSpace(argumentInfo.LongName))
               {
                   propertyHelpStringBuilder.AppendFormat("{0}--{1}", likeTabSeparator, argumentInfo.LongName);
               }

               propertyHelpStringBuilder.AppendFormat("{0}[{1}]", likeTabSeparator, argumentInfo.ArgumentType.Name.ToLower());

               if (!string.IsNullOrWhiteSpace(argumentInfo.Description))
               {
                   propertyHelpStringBuilder.AppendFormat("{0}{1}", likeTabSeparator, argumentInfo.Description);
               }

               if (argumentInfo.DefaultValue != null)
               {
                   propertyHelpStringBuilder.AppendFormat(" [default: {0}]", argumentInfo.DefaultValue);
               }

               Output.WriteInfo(propertyHelpStringBuilder.ToString());
           }

           return ReturnCode.Success.ToAsync();
       }
    }
}