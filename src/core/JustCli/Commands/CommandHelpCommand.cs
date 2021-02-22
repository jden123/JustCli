using System;
using System.Text;
using JustCli.Attributes;

namespace JustCli.Commands
{
   public class CommandHelpCommand : ICommand
   {
      private const string LikeTabSeparator = "  ";
      public Type CommandType { get; set; }
       public IOutput Output { get; set; }

       public CommandHelpCommand(Type commandType, IOutput output)
       {
           CommandType = commandType;
           Output = output;
       }

       public int Execute()
       {
           var helpStringBuilder = new StringBuilder();

           var commandInfo = CommandMetaDataHelper.GetCommandInfo(CommandType);
           helpStringBuilder.Append(commandInfo.Name);
           if (!string.IsNullOrWhiteSpace(commandInfo.Description))
           {
               helpStringBuilder.AppendFormat(" - {0}", commandInfo.Description);
           }

           if (!string.IsNullOrWhiteSpace(commandInfo.LongDescription))
           {
              helpStringBuilder.AppendLine();

              var longDescriptionLines = commandInfo.LongDescription.Split(
                 new[] { Environment.NewLine },
                 StringSplitOptions.None);

              foreach (var longDescriptionLine in longDescriptionLines)
              {
                 helpStringBuilder.AppendLine($"{LikeTabSeparator}{longDescriptionLine}");
              }
           }

           Output.WriteInfo(helpStringBuilder.ToString().TrimEnd(Environment.NewLine.ToCharArray()));

           var commandArgumentPropertyInfos = CommandMetaDataHelper.GetCommandArgumentInfos(CommandType);

           if (commandArgumentPropertyInfos.Count > 0)
           {
               Output.WriteInfo("Options:");
           }

           foreach (var argumentInfo in commandArgumentPropertyInfos)
           {
              var propertyHelpStringBuilder = new StringBuilder();
               propertyHelpStringBuilder.AppendFormat("{0}-{1}", LikeTabSeparator, argumentInfo.ShortName);

               if (!string.IsNullOrWhiteSpace(argumentInfo.LongName))
               {
                   propertyHelpStringBuilder.AppendFormat("{0}--{1}", LikeTabSeparator, argumentInfo.LongName);
               }

               propertyHelpStringBuilder.AppendFormat("{0}[{1}]", LikeTabSeparator, argumentInfo.ArgumentType.Name.ToLower());

               if (!string.IsNullOrWhiteSpace(argumentInfo.Description))
               {
                   propertyHelpStringBuilder.AppendFormat("{0}{1}", LikeTabSeparator, argumentInfo.Description);
               }

               if (argumentInfo.DefaultValue != null)
               {
                   propertyHelpStringBuilder.AppendFormat(" [default: {0}]", argumentInfo.DefaultValue);
               }

               Output.WriteInfo(propertyHelpStringBuilder.ToString());
           }

           return ReturnCode.Success;
       }
   }
}
