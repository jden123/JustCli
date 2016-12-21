using System;

namespace JustCli.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; set; }
        public string CommandDescription { get; set; }
        public int Order { get; set; }

        public CommandAttribute(string commandName) : this(commandName, string.Empty)
        {
        }

        public CommandAttribute(string commandName, string commandDescription) : this(commandName, commandDescription, int.MaxValue)
        {
        }

        public CommandAttribute(string commandName, string commandDescription, int order)
        {
            CommandName = commandName;
            CommandDescription = commandDescription;
            Order = order;
        }
    }
}