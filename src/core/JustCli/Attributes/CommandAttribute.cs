using System;

namespace JustCli.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; set; }
        public string CommandDescription { get; set; }

        public CommandAttribute(string commandName) : this(commandName, string.Empty)
        {
        }

        public CommandAttribute(string commandName, string commandDescription)
        {
            CommandName = commandName;
            CommandDescription = commandDescription;
        }
    }
}