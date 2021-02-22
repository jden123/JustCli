using System;

namespace JustCli.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; set; }
        public string CommandDescription { get; set; }
        public string CommandLongDescription { get; set; }
        public int Order { get; set; }

        public CommandAttribute(string commandName) : this(commandName, string.Empty)
        {
        }

        public CommandAttribute(
           string commandName,
           string commandDescription,
           int order = int.MaxValue)
           : this(commandName, commandDescription, string.Empty, order)
        {
        }

        public CommandAttribute(
           string commandName,
           string commandDescription,
           string commandLongDescription,
           int order = int.MaxValue)
        {
            CommandName = commandName;
            CommandDescription = commandDescription;
            CommandLongDescription = commandLongDescription;
            Order = order;
        }
    }
}
